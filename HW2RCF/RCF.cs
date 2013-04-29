using System.Collections.Generic;
using System.IO;
using System.Text;
using Homeworld2.IFF;

namespace Homeworld2.RCF
{
    public class RCF
    {
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
            iff.AddHandler("NAME", ChunkType.Default, ReadNAMEChunk);
            iff.AddHandler("VERS", ChunkType.Default, ReadVERSChunk);
            iff.AddHandler("ATTR", ChunkType.Default, ReadATTRChunk);
            iff.AddHandler("CSET", ChunkType.Default, ReadCSETChunk);

            iff.AddHandler("TYPE", ChunkType.Form, ReadTYPEChunk);

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
            iff.AddHandler("FONT", ChunkType.Form, ReadFONTChunk);
            iff.Parse();
        }

        public void Write(Stream stream)
        {
            IFFWriter iff = new IFFWriter(stream);
            iff.Push("FONT", ChunkType.Form);

            iff.Push("NAME");
            iff.Write(name);
            iff.Pop();

            iff.Push("VERS");
            iff.WriteInt32(version);
            iff.Pop();

            iff.Push("ATTR");
            iff.Write(attributes);
            iff.Pop();

            iff.Push("CSET");
            iff.WriteInt32(charCount);
            byte[] cset = Encoding.Unicode.GetBytes(charset);
            iff.Write(cset);
            iff.Pop();

            for (int i = 0; i < typefaces.Count; ++i)
            {
                iff.Push("TYPE", ChunkType.Form);
                typefaces[i].Write(iff);
                iff.Pop();
            }

            iff.Pop();
        }
    }
}
