using System.Collections.Generic;
using System.IO;
using System.Text;
using Homeworld2.IFF;

namespace Homeworld2.RCF
{
    public class RCF
    {
        public const string ChunkFont = "FONT";
        public const string ChunkName = "NAME";
        public const string ChunkVersion = "VERS";
        public const string ChunkAttributes = "ATTR";
        public const string ChunkCharset = "CSET";
        public const string ChunkTypeface = "TYPE";

        private string name;
        private int version;
        private byte[] attributes;
        private int charCount;
        private string charset;

        private List<Typeface> typefaces = new List<Typeface>();

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Version
        {
            get { return version; }
            set { version = value; }
        }

        public int CharsetCount
        {
            get { return charCount; }
        }

        public string Charset
        {
            get { return charset; }
            set { charset = value; }
        }

        public List<Typeface> Typefaces
        {
            get { return typefaces; }
        }

        private void ReadFONTChunk(IFFReader iff, ChunkAttributes attr)
        {
            iff.AddHandler(ChunkName, ChunkType.Default, ReadNAMEChunk);
            iff.AddHandler(ChunkVersion, ChunkType.Default, ReadVERSChunk);
            iff.AddHandler(ChunkAttributes, ChunkType.Default, ReadATTRChunk);
            iff.AddHandler(ChunkCharset, ChunkType.Default, ReadCSETChunk);

            iff.AddHandler(ChunkTypeface, ChunkType.Form, ReadTYPEChunk);

            iff.Parse();
        }

        private void ReadVERSChunk(IFFReader iff, ChunkAttributes attr)
        {
            version = iff.ReadInt32();
        }

        private void ReadATTRChunk(IFFReader iff, ChunkAttributes attr)
        {
            attributes = iff.ReadBytes(attr.Size);
        }

        private void ReadNAMEChunk(IFFReader iff, ChunkAttributes attr)
        {
            name = iff.ReadString();
        }

        private void ReadCSETChunk(IFFReader iff, ChunkAttributes attr)
        {
            charCount = iff.ReadInt32();
            charset = Encoding.Unicode.GetString(iff.ReadBytes(2 * charCount));
        }

        private void ReadTYPEChunk(IFFReader iff, ChunkAttributes attr)
        {
            Typeface type = new Typeface();
            typefaces.Add(type);
            type.Read(iff);
        }

        public void Read(Stream stream)
        {
            typefaces.Clear();

            IFFReader iff = new IFFReader(stream);
            iff.AddHandler(ChunkFont, ChunkType.Form, ReadFONTChunk);
            iff.Parse();
        }

        public void Write(Stream stream)
        {
            IFFWriter iff = new IFFWriter(stream);
            iff.Push(ChunkFont, ChunkType.Form);

            iff.Push(ChunkName);
            iff.Write(name);
            iff.Pop();

            iff.Push(ChunkVersion);
            iff.WriteInt32(version);
            iff.Pop();

            iff.Push(ChunkAttributes);
            iff.Write(attributes);
            iff.Pop();

            iff.Push(ChunkCharset);
            iff.WriteInt32(charCount);
            byte[] cset = Encoding.Unicode.GetBytes(charset);
            iff.Write(cset);
            iff.Pop();

            for (int i = 0; i < typefaces.Count; ++i)
            {
                iff.Push(ChunkTypeface, ChunkType.Form);
                typefaces[i].Write(iff);
                iff.Pop();
            }

            iff.Pop();
        }
    }
}
