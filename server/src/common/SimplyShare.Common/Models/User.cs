﻿using SimplyShare.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SimplyShare.Core.Models
{
    public class User
    {
        public string Id { get; set; }

        public string SecretHash { get; set; }

        public UserAddress UserAddress { get; set; }
    }

    public class UserAddress
    {
        public List<Address> Addresses { get; set; }
    }

    public class Address
    {
        private IPAddress _host;

        public AddressType Type { get; set; }

        public string Host
        {
            get
            {
                return _host?.ToString();
            }
            set
            {
                _host = IPAddress.Parse(value);
            }
        }

        public int Port { get; set; }
    }

    public enum AddressType
    {
        Public,
        Internal
    }
}
