using GalaSoft.MvvmLight;
using Homeworld2.RCF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RcfTool.ViewModel
{
    public class GlyphViewModel : ViewModelBase
    {
        private Glyph _glyph;
        public Glyph Glyph
        {
            get { return _glyph; }
            set
            {
                if (_glyph != value)
                {
                    RaisePropertyChanging(() => Glyph);

                    RaisePropertyChanging(() => Character);
                    RaisePropertyChanging(() => ImageIndex);
                    RaisePropertyChanging(() => LeftMargin);
                    RaisePropertyChanging(() => TopMargin);
                    RaisePropertyChanging(() => Width);
                    RaisePropertyChanging(() => Height);

                    _glyph = value;

                    RaisePropertyChanged(() => Glyph);

                    RaisePropertyChanged(() => Character);
                    RaisePropertyChanged(() => ImageIndex);
                    RaisePropertyChanged(() => LeftMargin);
                    RaisePropertyChanged(() => TopMargin);
                    RaisePropertyChanged(() => Width);
                    RaisePropertyChanged(() => Height);
                }
            }
        }

        public char Character
        {
            get { return _glyph.Character; }
            set
            {
                if (_glyph.Character != value)
                {
                    RaisePropertyChanging(() => Character);
                    _glyph.Character = value;
                    RaisePropertyChanged(() => Character);
                }
            }
        }

        public int ImageIndex
        {
            get { return _glyph.ImageIndex; }
            set
            {
                if (_glyph.ImageIndex != value)
                {
                    RaisePropertyChanging(() => ImageIndex);
                    _glyph.ImageIndex = value;
                    RaisePropertyChanged(() => ImageIndex);
                }
            }
        }

        public int LeftMargin
        {
            get { return _glyph.LeftMargin; }
            set
            {
                if (_glyph.LeftMargin != value)
                {
                    RaisePropertyChanging(() => LeftMargin);
                    _glyph.LeftMargin = value;
                    RaisePropertyChanged(() => LeftMargin);
                }
            }
        }

        public int TopMargin
        {
            get { return _glyph.TopMargin; }
            set
            {
                if (_glyph.TopMargin != value)
                {
                    RaisePropertyChanging(() => TopMargin);
                    _glyph.TopMargin = value;
                    RaisePropertyChanged(() => TopMargin);
                }
            }
        }

        public int Width
        {
            get { return _glyph.Width; }
            set
            {
                if (_glyph.Width != value)
                {
                    RaisePropertyChanging(() => Width);
                    _glyph.Width = value;
                    RaisePropertyChanged(() => Width);
                }
            }
        }

        public int Height
        {
            get { return _glyph.Height; }
            set
            {
                if (_glyph.Height != value)
                {
                    RaisePropertyChanging(() => Height);
                    _glyph.Height = value;
                    RaisePropertyChanged(() => Height);
                }
            }
        }
    }
}
