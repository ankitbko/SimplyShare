using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplyShare.Tracker.Models
{
    public class SharingOption
    {
        public TimeSpan MaxExpiry { get; set; }
        public TimeSpan MinExpiry { get; set; }
        public TimeSpan DefaultExpiry { get; set; }
    }
}
