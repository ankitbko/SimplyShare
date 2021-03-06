﻿using Microsoft.Extensions.Logging;
using SimplyShare.Core;
using SimplyShare.Core.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SimplyShare.Common
{
    public static class Extensions
    {
        public static string GetSHA1Hash(this Info info)
        {
            var bencoded = BencodeConvert.Serialize(info);
            using (var sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(bencoded));
                return Encoding.UTF8.GetString(hash);
            }
        }

        public static EventId GenerateEventId(this object @object) => new EventId(@object.GetHashCode());
    }
}
