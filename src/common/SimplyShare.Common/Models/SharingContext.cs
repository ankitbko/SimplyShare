using SimplyShare.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplyShare.Core
{
    public class SharingContext
    {
        public Guid Id { get; set; }

        public MetaInfo MetaInfo { get; set; }

        public string MetaInfoHash { get; }

        public User User { get; set; }

        public SharingConfiguration SharingConfiguration { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
