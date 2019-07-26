using BencodeNET.Objects;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SimplyShare.Core
{
    public static class BencodeConvert
    {
        public static string Serialize(object @object)
        {
            var objectDictionary = SerializeClass(@object);

            return objectDictionary?.EncodeAsString();
        }

        internal static BDictionary SerializeClass(object @object)
        {
            if (@object == null)
            {
                return null;
            }

            var properties = @object.GetType().GetProperties().ToList();
            var objectDictionary = new BDictionary();

            var propertyNameValuePairs = properties
                .Select(prop => SerializeProperty(prop, @object))
                .Where(pair => pair.Value != null);

            return new BDictionary(propertyNameValuePairs);
        }

        internal static KeyValuePair<BString, IBObject> SerializeProperty(PropertyInfo property, object @object)
        {
            var name = property.GetCustomAttribute<BencodeNameAttribute>()?.Name;
            if (string.IsNullOrWhiteSpace(name))
                name = property.Name.ToLowerInvariant();

            var key = new BString(name);
            var value = GetBObjectFromValue(property.GetValue(@object));

            return new KeyValuePair<BString, IBObject>(key, value);
        }

        internal static BObject GetBObjectFromValue<T>(T value)
        {
            if (value == null)
                return null;
            var type = value.GetType();
            if (type.Equals(typeof(string)))
                return CreateBString(value as string);
            if (type.Equals(typeof(DateTime)))
                return CreateBString(new DateTimeOffset((DateTime)(object)value).ToUnixTimeSeconds().ToString());
            if (type.Equals(typeof(int)))
                return CreateBNumber(Convert.ToInt32(value));
            if (type.Equals(typeof(long)))
                return CreateBNumber(Convert.ToInt64(value));
            if (typeof(ICollection).IsAssignableFrom(type))
                return CreateBList(((IEnumerable)value).OfType<object>().Select(obj => (IBObject)GetBObjectFromValue(obj)));
            if (type.IsClass)
                return SerializeClass(value);
            return null;
        }

        private static TValue GetValueFromProperty<TValue>(PropertyInfo propertyInfo, object @object) => (TValue)propertyInfo.GetValue(@object);

        private static BString CreateBString(string value) => new BString(value);

        private static BNumber CreateBNumber(long value) => new BNumber(value);

        private static BNumber CreateBNumber(int value) => new BNumber(value);

        private static BList CreateBList(IEnumerable<IBObject> value) => new BList(value);

        private class BencodePropertyCacheItem
        {
            public Dictionary<PropertyInfo, Func<PropertyInfo, object, BObject>> PropertyBencodeMap { get; set; }
            public BencodePropertyCacheItem()
            {
                PropertyBencodeMap = new Dictionary<PropertyInfo, Func<PropertyInfo, object, BObject>>();
            }
        }
    }
}
