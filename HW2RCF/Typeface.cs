using System.Collections.Generic;
using Homeworld2.IFF;

namespace Homeworld2.RCF
{
    public class Typeface
    {
        private const string ChunkName = "NAME";
        private const string ChunkAttributes = "ATTR";
        private const string ChunkImage = "IMAG";
        private const string ChunkGlyph = "GLPH";

        private string _name;
        private string _attributes;

        private readonly List<Image> _images = new List<Image>();
        private readonly List<Glyph> _glyphs = new List<Glyph>();

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }

        public List<Image> Images
        {
            get { return _images; }
        }

        public List<Glyph> Glyphs
        {
            get { return _glyphs; }
        }

        private void ReadNAMEChunk(IFFReader iff, ChunkAttributes attr)
        {
            _name = iff.ReadString();
        }

        private void ReadATTRChunk(IFFReader iff, ChunkAttributes attr)
        {
            _attributes = iff.ReadString();
        }

        private void ReadIMAGChunk(IFFReader iff, ChunkAttributes attr)
        {
            _images.Add(Image.Read(iff));
        }

        private void ReadGLPHChunk(IFFReader iff, ChunkAttributes attr)
        {
            _glyphs.Add(Glyph.Read(iff));
        }

        public static Typeface Read(IFFReader iff)
        {
            var typeface = new Typeface();
            iff.AddHandler(ChunkName, ChunkType.Default, typeface.ReadNAMEChunk);
            iff.AddHandler(ChunkAttributes, ChunkType.Default, typeface.ReadATTRChunk);

            iff.AddHandler(ChunkImage, ChunkType.Form, typeface.ReadIMAGChunk);
            iff.AddHandler(ChunkGlyph, ChunkType.Default, typeface.ReadGLPHChunk);

            iff.Parse();
            return typeface;
        }

        public void Write(IFFWriter iff)
        {
            iff.Push(ChunkName);
            iff.Write(_name);
            iff.Pop();

            iff.Push(ChunkAttributes);
            iff.Write(_attributes);
            iff.Pop();

            foreach (var image in _images)
            {
                iff.Push(ChunkImage, ChunkType.Form);
                image.Write(iff);
                iff.Pop();
            }

            foreach (var glyph in _glyphs)
            {
                iff.Push(ChunkGlyph);
                glyph.Write(iff);
                iff.Pop();
            }
        }
    }
}
