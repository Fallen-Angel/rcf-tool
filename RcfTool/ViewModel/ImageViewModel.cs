using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Homeworld2.RCF;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RcfTool.ViewModel
{
    public class ImageViewModel : ViewModelBase
    {
        private readonly Image _image;
        private WriteableBitmap _bitmap;

        public string Name
        {
            get { return _image.Name; }
            set
            {
                if (_image.Name != value)
                {
                    RaisePropertyChanging(() => Name);
                    _image.Name = value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }

        public int Version
        {
            get { return _image.Version; }
            set
            {
                if (_image.Version != value)
                {
                    RaisePropertyChanging(() => Version);
                    _image.Version = value;
                    RaisePropertyChanged(() => Version);
                }
            }
        }

        public BitmapSource Bitmap
        {
            get
            {
                if (_bitmap == null)
                {
                    _bitmap = new WriteableBitmap(BitmapSource.Create(_image.Width, _image.Height, 96, 96, PixelFormats.Gray8, BitmapPalettes.Gray256, _image.Data, _image.Width));
                }
                return _bitmap;
            }
            set
            {
                if (_bitmap != value)
                {
                    RaisePropertyChanging(() => Bitmap);
                    if (value.Format != PixelFormats.Gray8)
                    {
                        value = new FormatConvertedBitmap(value, PixelFormats.Gray8, BitmapPalettes.Gray256, 0);
                    }
                    SetNewBitmap(value);
                    RaisePropertyChanged(() => Bitmap);
                }
            }
        }

        private void SetNewBitmap(BitmapSource value)
        {
            _bitmap = new WriteableBitmap(value);

            UpdateImageData();
        }

        private void UpdateImageData()
        {
            var width = _bitmap.PixelWidth;
            var height = _bitmap.PixelHeight;

            int bytesPerPixel = (_bitmap.Format.BitsPerPixel + 7)/8;
            int stride = 4*((width*bytesPerPixel + 3)/4);

            var data = new byte[width*height];
            _bitmap.CopyPixels(data, stride, 0);
            _image.ModifyBitmapData(width, height, data);
        }

        public void ModifyBitmap(Int32Rect sourceRect, byte[] pixels, int stride)
        {
            _bitmap.WritePixels(sourceRect, pixels, stride, 0);

            UpdateImageData();
        }

        public ImageViewModel(Image image)
        {
            _image = image;
        }
    }
}
