namespace ZoteroFileAttach
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SQLite;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;

    using NUnit.Framework;

    [TestFixture]
    public class OpenSqlite
    {
        private static readonly Random Rand = new Random((int)DateTime.Now.Ticks);

        private const string SqlitePath = @"C:\code\zotorolib2\zotero.sqlite";

        private const string FileRoot = @"C:\Users\andrew\Andrew Export\2012 Research";

        private const string StorageFolder = @"C:\code\zotorolib2\storage";

        [Test]
        public void GetRandomString()
        {
            var sb = RandomString();
            Debug.WriteLine(sb);
        }

        private static string RandomString()
        {
            string list = "23456789ABCDEFGHIJKMNPQRSTUVWXTZ";
            var sb = new StringBuilder();
            while (sb.Length < 8)
            {
                var randIndex = Rand.Next(list.Length - 1);
                var letter = list[randIndex];
                sb.Append(letter);
            }
            return sb.ToString();
        }

        [Test]
        public void ListFiles()
        {
            foreach (var entry in this.GetFileRef(FileRoot))
            {
                Debug.WriteLine(entry);
            }
        }

        private Dictionary<string,string> GetFileRef(string directory)
        {
            var list = GetFileList(directory);
            return list.ToDictionary(x => Path.GetFileName(x), x => x);
        } 

        private IEnumerable<string> GetFileList(string directory)
        {
            foreach (var file in Directory.GetFiles(directory))
            {
                yield return file;
            }
            foreach (var subDirectory in Directory.GetDirectories(directory))
            {
                foreach (var file in GetFileList(subDirectory))
                {
                    yield return file;
                } 
            }
        }

        [Test]
        public void OpenFile()
        {
            using (var conn = new SQLiteConnection(@"Data Source=C:\code\zotorolib2\zotero.sqlite;Version=3"))
            {
                conn.Open();
                var pdfRefs = new Dictionary<int, string>();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText =
                        @"select itemData.itemId, itemDataValues.value from itemData inner join itemDataValues on itemData.valueid = itemdatavalues.valueid
where value like '%internal-pdf%'";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pdfRefs.Add(reader.GetInt32(0), reader.GetString(1));
                        }
                    }
                }
                long nextItemId = GetNextId(conn, "items", "itemId");
                long nextDataItemId = GetNextId(conn, "itemDataValues", "valueId");
                var fileRef = this.GetFileRef(FileRoot);
                foreach (var pdfRef in pdfRefs)
                {
                    var valueId = pdfRef.Key;
                    var parentItemId = pdfRef.Key;
                    var separatedFileNames = pdfRef.Value.Replace("internal-pdf://", "|");
                    var fileNames = separatedFileNames.Split('|');
                    foreach (var splitValue in fileNames)
                    {
                        var trimmedValue = splitValue.Trim();
                        if (!string.IsNullOrEmpty(trimmedValue))
                        {
                            var lastSlashIndex = trimmedValue.LastIndexOf("/");
                            var fileName = trimmedValue.Substring(lastSlashIndex + 1, trimmedValue.Length - lastSlashIndex - 1).Replace("%3B", ";");
                            var filePath = fileRef[fileName];
                            var randomId = RandomString();
                            var randomDirectory = Path.Combine(StorageFolder, randomId);
                            Directory.CreateDirectory(randomDirectory);
                            var fileTarget = Path.Combine(StorageFolder, randomId, fileName);
                            File.Copy(filePath, fileTarget);
                            var sql =
                                string.Format(@"INSERT INTO items VALUES({0},14,'2012-04-25 04:05:49','2012-04-25 04:06:01','2012-04-25 04:06:01',NULL,'{4}');
INSERT INTO itemDataValues VALUES({2},'{3}');
INSERT INTO itemData VALUES({0},110,{2});
INSERT INTO itemAttachments VALUES({0},{1},0,'application/pdf',NULL,'storage:{3}',NULL,0,NULL,NULL);",
                                nextItemId,
                                parentItemId,
                                nextDataItemId,
                                fileName,
                                randomId);

                            Debug.WriteLine(sql);
                            nextItemId++;
                            nextDataItemId++;
                        }
                    }
                }
            }
        }

        private static long GetNextId(SQLiteConnection conn, string tableName, string idFieldName)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "select Max(" + idFieldName + ") from " + tableName;
                return ((long)cmd.ExecuteScalar()) + 1;
            }
        }
    }
}
