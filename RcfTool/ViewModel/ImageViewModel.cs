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
using System.Windows.Media.Imaging;

namespace RcfTool.ViewModel
{
    public class ImageViewModel : ViewModelBase
    {
        private readonly Image _image;

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
            get { return _image.Bitmap; }
            set
            {
                if (_image.Bitmap != value)
                {
                    RaisePropertyChanging(() => Bitmap);
                    _image.Bitmap = value;
                    RaisePropertyChanged(() => Bitmap);
                }
            }
        }

        public ImageViewModel(Image image)
        {
            _image = image;
        }
    }
}
