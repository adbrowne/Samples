using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;

namespace NoteApi.Domain
{
    public class Image
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
    }

    public class ImageMap : ClassMap<Image>
    {
        public ImageMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            Table("tblImage");
        }
    }
}