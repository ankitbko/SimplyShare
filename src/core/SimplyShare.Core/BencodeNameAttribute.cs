using System;
using System.Collections.Generic;
using System.Text;

namespace SimplyShare.Core
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class BencodeNameAttribute: Attribute
    {
        public string Name { get; }
        public BencodeNameAttribute(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("BencodeName cannot be empty.", nameof(name));
            }

            Name = name;
        }
    }
}
