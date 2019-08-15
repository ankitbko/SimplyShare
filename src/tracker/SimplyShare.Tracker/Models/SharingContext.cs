using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using SimplyShare.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplyShare.Tracker.Models
{
    public class SharingContext
    {
        [BsonId(IdGenerator = typeof(GuidGenerator))]
        public Guid Id { get; set; }

        public MetaInfo MetaInfo { get; set; }

        public string InfoHash { get; set; }

        public User User { get; set; }

        public SharingConfiguration SharingConfiguration { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
