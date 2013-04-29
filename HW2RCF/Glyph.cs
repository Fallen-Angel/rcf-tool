using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using Homeworld2.IFF;

namespace Homeworld2.RCF
{
    public class Glyph
    {
        private char character;
        private int imageIndex;
        private int left;
        private int top;
        private int width;
        private int height;

        private float tmp1;
        private float widthPointsJ;
        private float widthJ;
        private float tmp2;
        private float heightPointsJ;
        private float heightJ;
        private byte temp;
        private Typeface typeface;

        public char Character
        {
            get { return character; }
            set { character = value; }
        }

        public int ImageIndex
        {
            get { return imageIndex; }
            set { imageIndex = value; }
        }

        public int Left
        {
            get { return left; }
            set { left = value; }
        }

        public int Top
        {
            get { return top; }
            set { top = value; }
        }

        public int Width
        {
            get { return width; }
            set
            {
                width = value;
            }
        }

        public int Height
        {
            get { return height; }
            set
            {
                height = value;
                Int32Rect rect = croppedBitmap.SourceRect;
                rect.Height = value;
                croppedBitmap.SourceRect = rect;
            }
        }

        public float WidthPointsJ
        {
            get { return widthPointsJ; }
            set { widthPointsJ = value; }
        }

        public float WidthJ
        {
            get { return widthJ; }
            set { widthJ = value; }
        }

        public float HeightPointsJ
        {
            get { return heightPointsJ; }
            set { heightPointsJ = value; }
        }

        public float HeightJ
        {
            get { return heightJ; }
            set { heightJ = value; }
        }

        public Int32Rect CropRect
        {
            get { return new Int32Rect(left, top, width, height); }
        }

        public BitmapSource ImageBitmap
        {
            get { return typeface.Images[imageIndex].Bitmap; }
        }

        private CroppedBitmap croppedBitmap;

        public CroppedBitmap GlyphBitmap
        {
            get
            {
                Height = 13; return croppedBitmap;
            }
        }

        public Glyph(Typeface typeface)
        {
            this.typeface = typeface;
        }

        public void Read(IFFReader iff)
        {
            character = Encoding.Unicode.GetChars(iff.ReadBytes(2))[0];
            imageIndex = iff.ReadInt32();
            left = iff.ReadInt32();
            top = iff.ReadInt32();
            width = iff.ReadInt32();
            height = iff.ReadInt32();

            tmp1 = iff.ReadSingle();
            widthPointsJ = iff.ReadSingle();
            widthJ = iff.ReadSingle();
            tmp2 = iff.ReadSingle();
            heightPointsJ = iff.ReadSingle();
            heightJ = iff.ReadSingle();
            temp = iff.ReadByte();

            croppedBitmap = new CroppedBitmap(typeface.Images[imageIndex].Bitmap, CropRect);
        }

        public void Write(IFFWriter iff)
        {
            char[] characterArray = new char[1];
            characterArray[0] = character;
            iff.Write(Encoding.Unicode.GetBytes(characterArray));
            iff.WriteInt32(imageIndex);
            iff.WriteInt32(left);
            iff.WriteInt32(top);
            iff.WriteInt32(width);
            iff.WriteInt32(height);

            iff.Write(tmp1);
            iff.Write(widthPointsJ);
            iff.Write(widthJ);
            iff.Write(tmp2);
            iff.Write(heightPointsJ);
            iff.Write(heightJ);
            iff.Write(temp);
        }

        public override string ToString()
        {
            return string.Format("Character {1}", character);
        }
    }
}
