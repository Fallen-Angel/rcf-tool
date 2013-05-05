using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RcfTool.ViewModel
{
    public class TypefaceViewModel : ViewModelBase
    {
        private string _name = "Microgramma 24";
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

        private string _attributes = "dadad";
        public string Attributes
        {
            get { return _attributes; }
            set
            {
                if (_attributes != value)
                {
                    RaisePropertyChanging(() => Attributes);
                    _attributes = value;
                    RaisePropertyChanged(() => Attributes);
                }
            }
        }

        private ObservableCollection<ImageViewModel> _images = new ObservableCollection<ImageViewModel>();
        public ObservableCollection<ImageViewModel> Images
        {
            get { return _images; }
            set
            {
                if (_images != value)
                {
                    RaisePropertyChanging(() => Images);
                    _images = value;
                    RaisePropertyChanged(() => Images);
                }
            }
        }

        private ObservableCollection<GlyphViewModel> _glyphs = new ObservableCollection<GlyphViewModel>();
        public ObservableCollection<GlyphViewModel> Glyphs
        {
            get { return _glyphs; }
            set
            {
                if (_glyphs != value)
                {
                    RaisePropertyChanging(() => Glyphs);
                    _glyphs = value;
                    RaisePropertyChanged(() => Glyphs);
                }
            }
        }

        public TypefaceViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
            _images.Add(new ImageViewModel());
            _images.Add(new ImageViewModel());
            _images.Add(new ImageViewModel());

            _glyphs.Add(new GlyphViewModel());
            _glyphs.Add(new GlyphViewModel());
            _glyphs.Add(new GlyphViewModel());
        }
    }
}
