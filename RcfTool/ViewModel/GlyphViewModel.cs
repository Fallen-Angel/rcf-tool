using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RcfTool.ViewModel
{
    public class GlyphViewModel : ViewModelBase
    {
        private char _character = 'd';
        public char Character
        {
            get { return _character; }
            set
            {
                if (_character != value)
                {
                    RaisePropertyChanging(() => Character);
                    _character = value;
                    RaisePropertyChanged(() => Character);
                }
            }
        }

        private int _imageIndex = 0;
        public int ImageIndex
        {
            get { return _imageIndex; }
            set
            {
                if (_imageIndex != value)
                {
                    RaisePropertyChanging(() => ImageIndex);
                    _imageIndex = value;
                    RaisePropertyChanged(() => ImageIndex);
                }
            }
        }

        private int _leftMargin = 0;
        public int LeftMargin
        {
            get { return _leftMargin; }
            set
            {
                if (_leftMargin != value)
                {
                    RaisePropertyChanging(() => LeftMargin);
                    _leftMargin = value;
                    RaisePropertyChanged(() => LeftMargin);
                }
            }
        }

        private int _topMargin = 0;
        public int TopMargin
        {
            get { return _topMargin; }
            set
            {
                if (_topMargin != value)
                {
                    RaisePropertyChanging(() => TopMargin);
                    _topMargin = value;
                    RaisePropertyChanged(() => TopMargin);
                }
            }
        }

        private int _width = 0;
        public int Width
        {
            get { return _width; }
            set
            {
                if (_width != value)
                {
                    RaisePropertyChanging(() => Width);
                    _width = value;
                    RaisePropertyChanged(() => Width);
                }
            }
        }

        private int _height = 0;
        public int Height
        {
            get { return _height; }
            set
            {
                if (_height != value)
                {
                    RaisePropertyChanging(() => Height);
                    _height = value;
                    RaisePropertyChanged(() => Height);
                }
            }
        }
    }
}
