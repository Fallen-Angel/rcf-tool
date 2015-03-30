using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Homeworld2.RCF;
using Microsoft.Practices.Prism.Mvvm;

namespace RcfTool.ViewModels
{
    public class ImageViewModel : BindableBase
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
                    _image.Name = value;
                    OnPropertyChanged(nameof(Name));
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
                    _image.Version = value;
                    OnPropertyChanged(nameof(Version));
                }
            }
        }

        public BitmapSource Bitmap
        {
            get {
                return _bitmap ??
                       (_bitmap =
                           new WriteableBitmap(BitmapSource.Create(_image.Width, _image.Height, 96, 96,
                               PixelFormats.Gray8, BitmapPalettes.Gray256, _image.Data, _image.Width)));
            }
            set
            {
                if (_bitmap != value)
                {
                    if (value.Format != PixelFormats.Gray8)
                    {
                        value = new FormatConvertedBitmap(value, PixelFormats.Gray8, BitmapPalettes.Gray256, 0);
                    }
                    SetNewBitmap(value);
                    OnPropertyChanged(nameof(Bitmap));
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
