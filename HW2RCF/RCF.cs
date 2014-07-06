using System.Collections.Generic;
using System.IO;
using System.Text;
using Homeworld2.IFF;

namespace Homeworld2.RCF
{
    public class RCF
    {
        private const string ChunkFont = "FONT";
        private const string ChunkName = "NAME";
        private const string ChunkVersion = "VERS";
        private const string ChunkAttributes = "ATTR";
        private const string ChunkCharset = "CSET";
        private const string ChunkTypeface = "TYPE";

        private string _name;
        private int _version;
        private byte[] _attributes;
        private int _charCount;
        private string _charset;

        private readonly List<Typeface> _typefaces = new List<Typeface>();

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int Version
        {
            get { return _version; }
            set { _version = value; }
        }

        public int CharsetCount
        {
            get { return _charCount; }
        }

        public string Charset
        {
            get { return _charset; }
            set { _charset = value; }
        }

        public List<Typeface> Typefaces
        {
            get { return _typefaces; }
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
            _version = iff.ReadInt32();
        }

        private void ReadATTRChunk(IFFReader iff, ChunkAttributes attr)
        {
            _attributes = iff.ReadBytes(attr.Size);
        }

        private void ReadNAMEChunk(IFFReader iff, ChunkAttributes attr)
        {
            _name = iff.ReadString();
        }

        private void ReadCSETChunk(IFFReader iff, ChunkAttributes attr)
        {
            _charCount = iff.ReadInt32();
            _charset = Encoding.Unicode.GetString(iff.ReadBytes(2 * _charCount));
        }

        private void ReadTYPEChunk(IFFReader iff, ChunkAttributes attr)
        {
            _typefaces.Add(Typeface.Read(iff));
        }

        public void Read(Stream stream)
        {
            _typefaces.Clear();

            var iff = new IFFReader(stream);
            iff.AddHandler(ChunkFont, ChunkType.Form, ReadFONTChunk);
            iff.Parse();
        }

        public void Write(Stream stream)
        {
            var iff = new IFFWriter(stream);
            iff.Push(ChunkFont, ChunkType.Form);

            iff.Push(ChunkName);
            iff.Write(_name);
            iff.Pop();

            iff.Push(ChunkVersion);
            iff.WriteInt32(_version);
            iff.Pop();

            iff.Push(ChunkAttributes);
            iff.Write(_attributes);
            iff.Pop();

            iff.Push(ChunkCharset);
            iff.WriteInt32(_charCount);
            iff.Write(Encoding.Unicode.GetBytes(_charset));
            iff.Pop();

            foreach (var typeface in _typefaces)
            {
                iff.Push(ChunkTypeface, ChunkType.Form);
                typeface.Write(iff);
                iff.Pop();
            }

            iff.Pop();
        }
    }
}
