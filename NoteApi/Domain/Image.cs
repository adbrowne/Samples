using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;

namespace NoteApi.Domain
{
    public class Image : IEntity
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
    }

    public class ImageNote : IEntity
    {
        public virtual Guid Id { get; set; }
        public virtual string Text { get; set; }
        public virtual long X { get; set; }
        public virtual long Y { get; set; }
    }
}