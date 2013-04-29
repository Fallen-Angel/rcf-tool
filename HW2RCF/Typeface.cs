using System.Collections.Generic;
using Homeworld2.IFF;

namespace Homeworld2.RCF
{
    public class Typeface
    {
        public const string ChunkName = "NAME";
        public const string ChunkAttributes = "ATTR";
        public const string ChunkImage = "IMAG";
        public const string ChunkGlyph = "GLPH";

        private string name;
        private string attributes;

        private List<Image> images = new List<Image>();
        private List<Glyph> glyphs = new List<Glyph>();

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Attributes
        {
            get { return attributes; }
            set { attributes = value; }
        }

        public List<Image> Images
        {
            get { return images; }
        }

        public List<Glyph> Glyphs
        {
            get { return glyphs; }
        }

        private void ReadNAMEChunk(IFFReader iff, ChunkAttributes attr)
        {
            name = iff.ReadString();
        }

        private void ReadATTRChunk(IFFReader iff, ChunkAttributes attr)
        {
            attributes = iff.ReadString();
        }

        private void ReadIMAGChunk(IFFReader iff, ChunkAttributes attr)
        {
            Image image = new Image();
            images.Add(image);
            image.Read(iff);
        }

        private void ReadGLPHChunk(IFFReader iff, ChunkAttributes attr)
        {
            Glyph glyph = new Glyph(this);
            glyph.Read(iff);
            glyphs.Add(glyph);
        }

        public void Read(IFFReader iff)
        {
            iff.AddHandler(ChunkName, ChunkType.Default, ReadNAMEChunk);
            iff.AddHandler(ChunkAttributes, ChunkType.Default, ReadATTRChunk);

            iff.AddHandler(ChunkImage, ChunkType.Form, ReadIMAGChunk);
            iff.AddHandler(ChunkGlyph, ChunkType.Default, ReadGLPHChunk);

            iff.Parse();
        }

        public void Write(IFFWriter iff)
        {
            iff.Push(ChunkName);
            iff.Write(name);
            iff.Pop();

            iff.Push(ChunkAttributes);
            iff.Write(attributes);
            iff.Pop();

            for (int i = 0; i < images.Count; ++i)
            {
                iff.Push(ChunkImage, ChunkType.Form);
                images[i].Write(iff);
                iff.Pop();
            }

            for (int i = 0; i < glyphs.Count; ++i)
            {
                iff.Push(ChunkGlyph);
                glyphs[i].Write(iff);
                iff.Pop();
            }
        }

        public override string ToString()
        {
            return name;
        }
    }
}
