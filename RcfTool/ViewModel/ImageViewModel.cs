using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace RcfTool.ViewModel
{
    public class ImageViewModel : ViewModelBase
    {
        private string _name = "2";
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    RaisePropertyChanging(() => Name);
                    _name = value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }

        private int _version = 1;
        public int Version
        {
            get { return _version; }
            set
            {
                if (_version != value)
                {
                    RaisePropertyChanging(() => Version);
                    _version = value;
                    RaisePropertyChanged(() => Version);
                }
            }
        }

        private BitmapSource _bitmap;
        public BitmapSource Bitmap
        {
            get { return _bitmap; }
            set
            {
                if (_bitmap != value)
                {
                    RaisePropertyChanging(() => Bitmap);
                    _bitmap = value;
                    RaisePropertyChanged(() => Bitmap);
                }
            }
        }
    }
}
