using System.Collections.Generic;
using System.IO;
using System.Text;
using Homeworld2.IFF;

namespace Homeworld2.RCF
{
    public class RCF
    {
        private byte[] _attributes;

        public string Name { get; set; }

        public int Version { get; set; }

        public int CharsetCount { get; private set; }

        public string Charset { get; set; }

        public List<Typeface> Typefaces { get; } = new List<Typeface>();

        private void ReadFONTChunk(IFFReader iff, ChunkAttributes attr)
        {
            iff.AddHandler(Chunks.Name, ChunkType.Default, ReadNAMEChunk);
            iff.AddHandler(Chunks.Version, ChunkType.Default, ReadVERSChunk);
            iff.AddHandler(Chunks.Attributes, ChunkType.Default, ReadATTRChunk);
            iff.AddHandler(Chunks.Charset, ChunkType.Default, ReadCSETChunk);

            iff.AddHandler(Chunks.Typeface, ChunkType.Form, ReadTYPEChunk);

            iff.Parse();
        }

        private void ReadVERSChunk(IFFReader iff, ChunkAttributes attr)
        {
            Version = iff.ReadInt32();
        }

        private void ReadATTRChunk(IFFReader iff, ChunkAttributes attr)
        {
            _attributes = iff.ReadBytes(attr.Size);
        }

        private void ReadNAMEChunk(IFFReader iff, ChunkAttributes attr)
        {
            Name = iff.ReadString();
        }

        private void ReadCSETChunk(IFFReader iff, ChunkAttributes attr)
        {
            CharsetCount = iff.ReadInt32();
            Charset = Encoding.Unicode.GetString(iff.ReadBytes(2 * CharsetCount));
        }

        private void ReadTYPEChunk(IFFReader iff, ChunkAttributes attr)
        {
            Typefaces.Add(Typeface.Read(iff));
        }

        public void Read(Stream stream)
        {
            Typefaces.Clear();

            var iff = new IFFReader(stream);
            iff.AddHandler(Chunks.Font, ChunkType.Form, ReadFONTChunk);
            iff.Parse();
        }

        public void Write(Stream stream)
        {
            var iff = new IFFWriter(stream);
            iff.Push(Chunks.Font, ChunkType.Form);

            iff.Push(Chunks.Name);
            iff.Write(Name);
            iff.Pop();

            iff.Push(Chunks.Version);
            iff.WriteInt32(Version);
            iff.Pop();

            iff.Push(Chunks.Attributes);
            iff.Write(_attributes);
            iff.Pop();

            iff.Push(Chunks.Charset);
            iff.WriteInt32(CharsetCount);
            iff.Write(Encoding.Unicode.GetBytes(Charset));
            iff.Pop();

            foreach (var typeface in Typefaces)
            {
                iff.Push(Chunks.Typeface, ChunkType.Form);
                typeface.Write(iff);
                iff.Pop();
            }

            iff.Pop();
        }
    }
}
