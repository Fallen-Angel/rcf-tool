﻿using System.Windows.Media;
using System.Windows.Media.Imaging;
using Homeworld2.IFF;

namespace Homeworld2.RCF
{
    public class Image
    {
        private string name;
        private int version;
        private int width;
        private int height;
        private int dataSize;
        private byte[] data;
        private BitmapSource bitmap;

        public string Name
        {
            get { return name; }
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
        }

        public BitmapSource Bitmap
        {
            get
            {
                if (bitmap == null)
                {
                    bitmap = BitmapSource.Create(width, height, 96, 96, PixelFormats.Gray8, BitmapPalettes.Gray256, data, width);
                }
                return bitmap;
            }
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

        public void Read(IFFReader iff)
        {
            iff.AddHandler("NAME", ChunkType.Default, ReadNAMEChunk);
            iff.AddHandler("ATTR", ChunkType.Default, ReadATTRChunk);
            iff.AddHandler("DATA", ChunkType.Default, ReadDATAChunk);
            iff.Parse();
        }

        public void Write(IFFWriter iff)
        {
            iff.Push("NAME");
            iff.Write(name);
            iff.Pop();

            iff.Push("ATTR");
            iff.WriteInt32(version);
            iff.WriteInt32(width);
            iff.WriteInt32(height);
            iff.WriteInt32(dataSize);
            iff.Pop();

            iff.Push("DATA");
            iff.Write(data);
            iff.Pop();
        }
    }
}
