using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using Homeworld2.IFF;
using System.Windows.Media;

namespace Homeworld2.RCF
{
    public class Glyph
    {
        private char _character;
        private int _imageIndex;
        private int _leftMargin;
        private int _topMargin;
        private int _width;
        private int _height;

        private float _tmp1;
        private float _widthInPoints;
        private float _floatWidth;
        private float _tmp2;
        private float _heightInPoints;
        private float _floatHeight;
        private byte _temp;

        private readonly Typeface _typeface;

        public char Character
        {
            get { return _character; }
            set { _character = value; }
        }

        public int ImageIndex
        {
            get { return _imageIndex; }
            set
            {
                _imageIndex = value;
                GenerateCropRect();
            }
        }

        public int LeftMargin
        {
            get { return _leftMargin; }
            set
            {
                _leftMargin = value;
                GenerateCropRect();
            }
        }

        public int TopMargin
        {
            get { return _topMargin; }
            set
            {
                _topMargin = value;
                GenerateCropRect();
            }
        }

        public int Width
        {
            get { return _width; }
            set
            {
                _width = value;
                GenerateCropRect();
            }
        }

        public int Height
        {
            get { return _height; }
            set
            {
                _height = value;
                GenerateCropRect();
            }
        }

        public float WidthInPoints
        {
            get { return _widthInPoints; }
            set { _widthInPoints = value; }
        }

        public float FloatWidth
        {
            get { return _floatWidth; }
            set { _floatWidth = value; }
        }

        public float HeightInPoints
        {
            get { return _heightInPoints; }
            set { _heightInPoints = value; }
        }

        public float FloatHeight
        {
            get { return _floatHeight; }
            set { _floatHeight = value; }
        }

        public Int32Rect CropRect { get; set; }

        private void GenerateCropRect()
        {
            CropRect = new Int32Rect(_leftMargin, _topMargin, _width, _height);
            _croppedBitmap = new CroppedBitmap(_typeface.Images[_imageIndex].Bitmap, CropRect);
        }

        public BitmapSource ImageBitmap
        {
            get { return Image.Bitmap; }
        }

        private Image Image
        {
            get { return _typeface.Images[_imageIndex]; }
        }

        private CroppedBitmap _croppedBitmap;

        public BitmapSource GlyphBitmap
        {
            get { return _croppedBitmap; }
            set
            {
                if (!Equals(_croppedBitmap, value) &&
                    ((value.PixelWidth == _croppedBitmap.PixelWidth) && (value.PixelHeight == _croppedBitmap.PixelHeight)))
                {
                    if (value.Format != PixelFormats.Gray8)
                    {
                        value = new FormatConvertedBitmap(value, PixelFormats.Gray8, BitmapPalettes.Gray256, 0);
                    }

                    var data = new byte[value.PixelWidth * value.PixelHeight];
                    value.CopyPixels(data, value.PixelWidth, 0);

                    Image.ModifyBitmap(CropRect, data, value.PixelWidth);
                    GenerateCropRect();
                }
            }
        }

        public Glyph(Typeface typeface)
        {
            _typeface = typeface;
        }

        public static Glyph Read(IFFReader iff)
        {
            var glyph = new Glyph();
            glyph._character = Encoding.Unicode.GetChars(iff.ReadBytes(2))[0];
            glyph._imageIndex = iff.ReadInt32();
            glyph._leftMargin = iff.ReadInt32();
            glyph._topMargin = iff.ReadInt32();
            glyph._width = iff.ReadInt32();
            glyph._height = iff.ReadInt32();

            glyph._tmp1 = iff.ReadSingle();
            glyph._widthInPoints = iff.ReadSingle();
            glyph._floatWidth = iff.ReadSingle();
            glyph._tmp2 = iff.ReadSingle();
            glyph._heightInPoints = iff.ReadSingle();
            glyph._floatHeight = iff.ReadSingle();
            glyph._temp = iff.ReadByte();

            glyph.GenerateCropRect();
            return glyph;
        }

        public void Write(IFFWriter iff)
        {
            iff.Write(Encoding.Unicode.GetBytes(new[] { _character }));
            iff.WriteInt32(_imageIndex);
            iff.WriteInt32(_leftMargin);
            iff.WriteInt32(_topMargin);
            iff.WriteInt32(_width);
            iff.WriteInt32(_height);

            iff.Write(_tmp1);
            iff.Write(_widthInPoints);
            iff.Write(_floatWidth);
            iff.Write(_tmp2);
            iff.Write(_heightInPoints);
            iff.Write(_floatHeight);
            iff.Write(_temp);
        }
    }
}
