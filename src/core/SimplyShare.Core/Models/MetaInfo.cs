using System;
using System.Collections.Generic;

namespace SimplyShare.Core
{
    public class MetaInfo
    {
        public Info Info { get; set; }

        public string Announce { get; set; }

        [BencodeName("announce_list")]
        public List<string> AnnounceList { get; set; }

        [BencodeName("creation date")]
        public DateTime? CreationDate { get; set; }

        public string Comment { get; set; }

        [BencodeName("created by")]
        public string CreatedBy { get; set; }

        public string Encoding { get; set; }

        public MetaInfo(
            Info info,
            string announce,
            List<string> announce_list = null,
            DateTime? creationDate = null,
            string comment = null,
            string createdBy = null,
            string encoding = null)
        {
            Info = info ?? throw new ArgumentNullException(nameof(info));
            Announce = announce ?? throw new ArgumentNullException(nameof(announce));
        }
    }

    public abstract class Info
    {
        [BencodeName("piece length")]
        public long PieceLength { get; set; }
        public string Pieces { get; set; }
        public bool? Private { get; set; }

        protected Info(long pieceLength, string pieces)
        {
            PieceLength = pieceLength;
            Pieces = pieces ?? throw new ArgumentNullException(nameof(pieces));
        }
    }

    public class SingleFileInfo : Info
    {
        public string Name { get; set; }
        public long Length { get; set; }

        public SingleFileInfo(long pieceLength, string pieces, string name, long length)
            : base(pieceLength, pieces)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Length = length;
        }
    }

    public class MultipleFileInfo : Info
    {
        public string Name { get; set; }
        public List<File> Files { get; set; }

        public MultipleFileInfo(long pieceLength, string pieces, string name, List<File> files)
            : base(pieceLength, pieces)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Files = files ?? throw new ArgumentNullException(nameof(files));
        }
    }

    public class File
    {
        public long Length { get; set; }
        public List<string> Path { get; set; }

        public File(long length, List<string> path)
        {
            Length = length;
            Path = path ?? throw new ArgumentNullException(nameof(path));
        }
    }
}
