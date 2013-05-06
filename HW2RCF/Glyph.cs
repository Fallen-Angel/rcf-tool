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
        private int leftMargin;
        private int topMargin;
        private int width;
        private int height;

        private float tmp1;
        private float widthInPoints;
        private float floatWidth;
        private float tmp2;
        private float heightInPoints;
        private float floatHeight;
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
            set
            {
                imageIndex = value;
                GenerateCropRect();
            }
        }

        public int LeftMargin
        {
            get { return leftMargin; }
            set
            {
                leftMargin = value;
                GenerateCropRect();
            }
        }

        public int TopMargin
        {
            get { return topMargin; }
            set
            {
                topMargin = value;
                GenerateCropRect();
            }
        }

        public int Width
        {
            get { return width; }
            set
            {
                width = value;
                GenerateCropRect();
            }
        }

        public int Height
        {
            get { return height; }
            set
            {
                height = value;
                GenerateCropRect();
            }
        }

        public float WidthInPoints
        {
            get { return widthInPoints; }
            set { widthInPoints = value; }
        }

        public float FloatWidth
        {
            get { return floatWidth; }
            set { floatWidth = value; }
        }

        public float HeightInPoints
        {
            get { return heightInPoints; }
            set { heightInPoints = value; }
        }

        public float FloatHeight
        {
            get { return floatHeight; }
            set { floatHeight = value; }
        }

        private Int32Rect _cropRect;
        public Int32Rect CropRect
        {
            get { return _cropRect; }
            set
            {
                if (_cropRect != value)
                {
                    _cropRect = value;
                }
            }
        }

        private void GenerateCropRect()
        {
            CropRect = new Int32Rect(leftMargin, topMargin, width, height);
            croppedBitmap = new CroppedBitmap(typeface.Images[imageIndex].Bitmap, CropRect);
        }

        public BitmapSource ImageBitmap
        {
            get { return typeface.Images[imageIndex].Bitmap; }
        }

        private CroppedBitmap croppedBitmap;

        public CroppedBitmap GlyphBitmap
        {
            get { return croppedBitmap; }
        }

        public Glyph(Typeface typeface)
        {
            this.typeface = typeface;
        }

        public void Read(IFFReader iff)
        {
            character = Encoding.Unicode.GetChars(iff.ReadBytes(2))[0];
            imageIndex = iff.ReadInt32();
            leftMargin = iff.ReadInt32();
            topMargin = iff.ReadInt32();
            width = iff.ReadInt32();
            height = iff.ReadInt32();

            tmp1 = iff.ReadSingle();
            widthInPoints = iff.ReadSingle();
            floatWidth = iff.ReadSingle();
            tmp2 = iff.ReadSingle();
            heightInPoints = iff.ReadSingle();
            floatHeight = iff.ReadSingle();
            temp = iff.ReadByte();

            GenerateCropRect();
        }

        public void Write(IFFWriter iff)
        {
            char[] characterArray = new char[1];
            characterArray[0] = character;
            iff.Write(Encoding.Unicode.GetBytes(characterArray));
            iff.WriteInt32(imageIndex);
            iff.WriteInt32(leftMargin);
            iff.WriteInt32(topMargin);
            iff.WriteInt32(width);
            iff.WriteInt32(height);

            iff.Write(tmp1);
            iff.Write(widthInPoints);
            iff.Write(floatWidth);
            iff.Write(tmp2);
            iff.Write(heightInPoints);
            iff.Write(floatHeight);
            iff.Write(temp);
        }

        public override string ToString()
        {
            return string.Format("Character {1}", character);
        }
    }
}
