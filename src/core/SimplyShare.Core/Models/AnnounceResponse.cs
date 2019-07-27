using System;
using System.Collections.Generic;
using System.Text;

namespace SimplyShare.Core.Models
{
    public class AnnounceResponse
    {
        public string FailureReason { get; set; }

        public string WarningMessage { get; set; }

        public int Interval { get; set; }

        public int? MinimumInterval { get; set; }

        public string TrackerId { get; set; }

        public int Complete { get; set; }

        public int Incomplete { get; set; }

        public string Peers { get; set; }
    }
}
