using System;

namespace SimplyShare.Core
{
    public class SharingConfiguration
    {
        public SharingScope SharingScope { get; set; }

        public TimeSpan Expiry { get; set; }
    }
}
