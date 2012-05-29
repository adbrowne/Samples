using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Json.Net.RoundtripCollectionOfInterfaces
{
    using System.Collections.ObjectModel;

    using NUnit.Framework;

    using Newtonsoft.Json;

    [TestFixture]
    public class Class1
    {
        [Test]
        public void Blah()
        {
            var foo = new Foo { Name = "Andrew" };

            var serialized = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(foo));

            var result = JsonConvert.DeserializeObject<Foo>(Encoding.UTF8.GetString(serialized));

            Assert.That(result.Name, Is.EqualTo("Andrew"));
        }

        [Test]
        public void Blah2()
        {
            var foo = new Foo
            {
                Name = "Andrew",
                Friends = { 
                    new Friend{Name =  "Matt"},
                    new SubFriend
                        {
                            Name =  "Nick",
                            Age = 28
                        },
                    }
            };
            var settings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

            var serializeObject = JsonConvert.SerializeObject(foo, Formatting.Indented, settings);

            var bytes = Encoding.UTF8.GetBytes(serializeObject);

            var result = JsonConvert.DeserializeObject<Foo>(Encoding.UTF8.GetString(bytes), new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });

            Assert.That(result.Friends.Count, Is.EqualTo(2));

            var nick = result.Friends.Single(x => x.Name == "Nick");
            Assert.That(nick, Is.TypeOf<SubFriend>());
            
            var matt = result.Friends.Single(x => x.Name == "Matt");
            Assert.That(matt, Is.TypeOf<Friend>());
        }
    }

    public class Foo
    {
        public Foo()
        {
            Friends = new Collection<IFriend>();
        }

        public string Name { get; set; }

        public Collection<IFriend> Friends { get; set; }
    }

    public interface IFriend
    {
        string Name { get; }
    }

    public class Friend : IFriend
    {
        public string Name { get; set; }
    }

    public class SubFriend : Friend
    {
        public int Age { get; set; }
    }
}
