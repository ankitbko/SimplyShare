using SimplyShare.Core;
using SimplyShare.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SimplyShare.Common.Models
{
    public class ShareRequest
    {
        [Required]
        public MetaInfo MetaInfo { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        public SharingConfiguration SharingConfiguration { get; set; }
    }
}
