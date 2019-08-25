using System;
using Xunit;
using SimplyShare.Core;
using FluentAssertions;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace SimplyShare.Core.Test
{
    public class BenchodeConvertTest
    {
        private class Foo
        {
            public int? Integer { get; set; }
            public string String { get; set; }
            public long Long { get; set; }
            public DateTime? DateTime { get; set; }
        }

        private class Bar
        {
            public Foo Foo { get; set; }
            public List<int> BarIntegers { get; set; }
            public List<string> BarStrings { get; set; }
        }

        private class Baz
        {
            [BencodeName("First Name")]
            public string FirstName { get; set; }

        }

        [Fact]
        public void Should_convert_Class_with_primitive_fields()
        {
            var dummyDate = new DateTime(2002, 10, 15);
            var test = new Foo() { DateTime = dummyDate, Integer = 10, Long = 100000000000, String = "bar" };
            var dummyDateFromEpoch = (new DateTimeOffset(dummyDate)).ToUnixTimeSeconds();
            
            var expectedDateTime = $"8:datetime10:{dummyDateFromEpoch}";
            var expectedInteger = "7:integeri10e";
            var expectedLong = "4:longi100000000000e";
            var expectedString = "6:string3:bar";

            string actual = BencodeConvert.Serialize(test);

            actual.Should().ContainAll(expectedDateTime, expectedInteger, expectedLong, expectedString);
            actual.Should().StartWith("d");
            actual.Should().EndWith("e");
        }

        [Fact]
        public void Should_respect_BencodeNameAttribute()
        {
            var baz = new Baz() { FirstName = "abc" };
            var expected = "d10:First Name3:abce";

            var actual = BencodeConvert.Serialize(baz);

            actual.Should().Be(expected);
        }

        [Fact]
        public void Should_not_convert_null_fields()
        {
            var dummyDate = new DateTime(2002, 10, 15);
            var test = new Foo() { DateTime = dummyDate, Integer = 10, Long = 100000000000, String = null };
            var dummyDateFromEpoch = (new DateTimeOffset(dummyDate)).ToUnixTimeSeconds();

            var expectedDateTime = $"8:datetime10:{dummyDateFromEpoch}";
            var expectedInteger = "7:integeri10e";
            var expectedLong = "4:longi100000000000e";
            var notExpectedString = "6:string";

            string actual = BencodeConvert.Serialize(test);

            actual.Should().ContainAll(expectedDateTime, expectedInteger, expectedLong);
            actual.Should().NotContain(notExpectedString);
            actual.Should().StartWith("d");
            actual.Should().EndWith("e");
        }

        [Fact]
        public void Should_convert_Class_with_complex_fields()
        {
            var dummyDate = new DateTime(2002, 10, 15);
            var foo = new Foo() { DateTime = dummyDate, Integer = 10, Long = 100000000000, String = "bar" };
            var bar = new Bar() { BarIntegers = new List<int> { 20, 30 }, Foo = foo, BarStrings = new List<string> { "s1", "s2" } };
            var dummyDateFromEpoch = (new DateTimeOffset(dummyDate)).ToUnixTimeSeconds();

            var expectedFooDateTime = $"8:datetime10:{dummyDateFromEpoch}";
            var expectedFooInteger = "7:integeri10e";
            var expectedFooLong = "4:longi100000000000e";
            var expectedFooString = "6:string3:bar";
            var expectedBarIntegers = "11:barintegersli20ei30ee";
            var expectedBarStrings = "10:barstringsl2:s12:s2e";
            var expectedFooDictionary = "3:food";

            string actual = BencodeConvert.Serialize(bar);

            actual.Should().ContainAll(
                expectedFooDateTime, 
                expectedFooInteger, 
                expectedFooLong, 
                expectedFooString,
                expectedBarIntegers,
                expectedBarStrings,
                expectedFooDictionary);
            actual.Should().StartWith("d");
            actual.Should().EndWith("e");
        }

        [Fact]
        public void Should_return_Null_for_Null_Object()
        {
            object test = null;

            var actual = BencodeConvert.Serialize(test);

            actual.Should().BeNull();
        }
    }
}
