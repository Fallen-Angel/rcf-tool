using System.Windows.Media;
using System.Windows.Media.Imaging;
using Homeworld2.IFF;
using System.Windows;

namespace Homeworld2.RCF
{
    public class Image
    {
        public const string ChunkName = "NAME";
        public const string ChunkAttributes = "ATTR";
        public const string ChunkData = "DATA";

        private string name;
        private int version;
        private int width;
        private int height;
        private int dataSize;
        private byte[] data;
        private WriteableBitmap bitmap;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public byte[] Data
        {
            get { return data; }
        }

        public int Version
        {
            get { return version; }
            set { version = value; }
        }

        public BitmapSource Bitmap
        {
            get
            {
                if (bitmap == null)
                {
                    bitmap = new WriteableBitmap(BitmapSource.Create(width, height, 96, 96, PixelFormats.Gray8, BitmapPalettes.Gray256, data, width));
                }
                return bitmap;
            }
            set
            {
                if (value != bitmap)
                {
                    if (value.Format != PixelFormats.Gray8)
                    {
                        value = new FormatConvertedBitmap(value, PixelFormats.Gray8, BitmapPalettes.Gray256, 0);
                    }
                    SetNewBitmap(value);
                }
            }
        }

        private void SetNewBitmap(BitmapSource value)
        {
            bitmap = new WriteableBitmap(value);

            width = bitmap.PixelWidth;
            height = bitmap.PixelHeight;

            int bytesPerPixel = (bitmap.Format.BitsPerPixel + 7) / 8;
            int stride = 4 * ((width * bytesPerPixel + 3) / 4);

            data = new byte[width * height];
            bitmap.CopyPixels(data, stride, 0);
            dataSize = data.Length;
        }

        public void ModifyBitmap(Int32Rect sourceRect, byte[] pixels, int stride)
        {
            bitmap.WritePixels(sourceRect, pixels, stride, 0);

            int bytesPerPixel = (bitmap.Format.BitsPerPixel + 7) / 8;
            stride = 4 * ((width * bytesPerPixel + 3) / 4);

            bitmap.CopyPixels(data, stride, 0);
        }

        private void ReadNAMEChunk(IFFReader iff, ChunkAttributes attr)
        {
            name = iff.ReadString();
        }

        private void ReadATTRChunk(IFFReader iff, ChunkAttributes attr)
        {
            version = iff.ReadInt32();
            width = iff.ReadInt32();
            height = iff.ReadInt32();
            dataSize = iff.ReadInt32();
        }

        private void ReadDATAChunk(IFFReader iff, ChunkAttributes attr)
        {
            data = iff.ReadBytes(attr.Size);
        }

        public static Image Read(IFFReader iff)
        {
            var image = new Image();
            iff.AddHandler(ChunkName, ChunkType.Default, image.ReadNAMEChunk);
            iff.AddHandler(ChunkAttributes, ChunkType.Default, image.ReadATTRChunk);
            iff.AddHandler(ChunkData, ChunkType.Default, image.ReadDATAChunk);
            iff.Parse();
            return image;
        }

        public void Write(IFFWriter iff)
        {
            iff.Push(ChunkName);
            iff.Write(name);
            iff.Pop();

            iff.Push(ChunkAttributes);
            iff.WriteInt32(version);
            iff.WriteInt32(width);
            iff.WriteInt32(height);
            iff.WriteInt32(dataSize);
            iff.Pop();

            iff.Push(ChunkData);
            iff.Write(data);
            iff.Pop();
        }
    }
}
