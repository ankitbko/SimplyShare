using FakeItEasy;
using SimplyShare.Common.Models;
using SimplyShare.Core;
using SimplyShare.Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimplyShare.Tracker.Test
{
    public static class ShareRequestFactory
    {
        public static ShareRequest CreateCompleteRequestForSingleFile()
        {
            return new ShareRequest
            {
                MetaInfo = new MetaInfo(
                    new SingleFileInfo(
                        500, 
                        string.Join("", Enumerable.Range(0,20).Select(_ => "a")), 
                        "name", 
                        5000),
                    "http://announce"),
                User = new User
                {
                    Id = "userid",
                    SecretHash = "secret",
                    UserAddress = new UserAddress
                    {
                        Addresses = new List<Address>
                        {
                            new Address() { Host = "192.0.0.1", Port = 5770, Type = AddressType.Internal }
                        }
                    }
                },
                SharingConfiguration = new SharingConfiguration
                {
                    Expiry = TimeSpan.FromDays(5),
                    SharingScope = Core.SharingScope.Internal
                }
            };

        }
    }
}
