using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using SimplyShare.Common;
using SimplyShare.Common.Models;
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

        public static SharingContext Create(ShareRequest shareRequest)
        {
            var context = new SharingContext();

            context.MetaInfo = shareRequest.MetaInfo;
            context.User = shareRequest.User;
            context.SharingConfiguration = shareRequest.SharingConfiguration;
            context.InfoHash = shareRequest.MetaInfo?.Info?.GetSHA1Hash();
            context.CreatedOn = DateTime.UtcNow;

            if (context.SharingConfiguration == null)
                context.SharingConfiguration = new SharingConfiguration();

            return context;
        }
    }
}
