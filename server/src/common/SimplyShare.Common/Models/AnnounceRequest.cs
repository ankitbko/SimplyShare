using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SimplyShare.Core.Models
{
    public class AnnounceRequest
    {
        [Required]
        public string Info_Hash { get; set; }

        [Required]
        public string Peer_Id { get; set; }

        [Required]
        public int? Port { get; set; }

        [Required]
        public long? Uploaded { get; set; }

        [Required]
        public long? Downloaded { get; set; }

        [Required]
        public long? Left { get; set; }

        public bool Compact { get; set; } = false;

        public bool? No_Peer_Id { get; set; }  

        public AnnounceEvent? Event { get; set; }

        public int? Numwant { get; set; }

        public string Key { get; set; }

        public string TrackerId { get; set; }
    }

    public enum AnnounceEvent
    {
        Started,
        Stopped,
        Completed
    }
}
