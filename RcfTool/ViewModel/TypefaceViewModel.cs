using GalaSoft.MvvmLight;
using Homeworld2.RCF;
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
        private Typeface _typeface;
        public Typeface Typeface
        {
            get { return _typeface; }
            set
            {
                if (_typeface != value)
                {
                    RaisePropertyChanging(() => Typeface);
                    RaisePropertyChanging(() => Name);
                    RaisePropertyChanging(() => Attributes);
                    _typeface = value;
                    RaisePropertyChanged(() => Typeface);
                    RaisePropertyChanged(() => Name);
                    RaisePropertyChanged(() => Attributes);

                    _images.Clear();
                    foreach (Image img in _typeface.Images)
                    {
                        ImageViewModel vm = new ImageViewModel();
                        vm.Image = img;
                        _images.Add(vm);
                    }
                }
            }
        }

        public string Name
        {
            get { return _typeface.Name; }
            set
            {
                if (_typeface.Name != value)
                {
                    RaisePropertyChanging(() => Name);
                    _typeface.Name = value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }

        public string Attributes
        {
            get { return _typeface.Attributes; }
            set
            {
                if (_typeface.Attributes != value)
                {
                    RaisePropertyChanging(() => Attributes);
                    _typeface.Attributes = value;
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
