using FluentAssertions;
using SimplyShare.Core.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace SimplyShare.Core.Test
{
    public class MetaInfoTest
    {
        private byte[] piece1Sha = null;
        private byte[] piece2Sha = null;
        private MultipleFileInfo multiFileInfo;
        private MetaInfo multipleFileMetainfo;

        public MetaInfoTest()
        {
            var sha1 = new SHA1Managed();
            piece1Sha = sha1.ComputeHash(Encoding.ASCII.GetBytes("abc"));
            piece2Sha = sha1.ComputeHash(Encoding.ASCII.GetBytes("cde"));
            multiFileInfo = new MultipleFileInfo(
                262144,
                Encoding.ASCII.GetString(piece1Sha) + Encoding.ASCII.GetString(piece2Sha),
                "",
                new List<File> { new File(314291, new List<string> { "file1" }), new File(24102, new List<string> { "file2" }) });
            multipleFileMetainfo = new MetaInfo(multiFileInfo, "https://simplyshare.com");
        }

        [Fact]
        public void Should_Bencode_MultipleFileMetaInfo_with_required_fields()
        {
            var expectedInfo = "4:infod";
            var expectedPieceLength = "12:piece lengthi";
            var expectedPieces = "6:pieces";
            var expectedName = "4:name0:";
            var expectedLength = "6:lengthi";
            var expectedAnnounce = "8:announce";
            var expectedPath = "4:path";

            var actual = BencodeConvert.Serialize(multipleFileMetainfo);

            actual.Should().ContainAll(
                expectedInfo,
                expectedPieceLength,
                expectedPieces,
                expectedName,
                expectedLength,
                expectedAnnounce,
                expectedPath);

            actual.Should().StartWith("d");
            actual.Should().EndWith("e");
        }
    }
}
