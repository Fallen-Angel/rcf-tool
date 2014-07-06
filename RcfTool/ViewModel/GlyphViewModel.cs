using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Homeworld2.RCF;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;

namespace RcfTool.ViewModel
{
    public class GlyphViewModel : ViewModelBase
    {
        private readonly Glyph _glyph;

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
            get { return _glyph.ImageIndex + 1; }
            set
            {
                if (_glyph.ImageIndex != value)
                {
                    RaisePropertyChanging(() => ImageIndex);
                    RaisePropertyChanging(() => ImageBitmap);
                    RaisePropertyChanging(() => GlyphBitmap);
                    _glyph.ImageIndex = value - 1;
                    RaisePropertyChanged(() => ImageIndex);
                    RaisePropertyChanged(() => ImageBitmap);
                    RaisePropertyChanged(() => GlyphBitmap);
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
                    RaisePropertyChanging(() => ImageBitmap);
                    RaisePropertyChanging(() => GlyphBitmap);
                    _glyph.LeftMargin = value;
                    RaisePropertyChanged(() => LeftMargin);
                    RaisePropertyChanged(() => ImageBitmap);
                    RaisePropertyChanged(() => GlyphBitmap);
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
                    RaisePropertyChanging(() => ImageBitmap);
                    RaisePropertyChanging(() => GlyphBitmap);
                    _glyph.TopMargin = value;
                    RaisePropertyChanged(() => TopMargin);
                    RaisePropertyChanged(() => ImageBitmap);
                    RaisePropertyChanged(() => GlyphBitmap);
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
                    RaisePropertyChanging(() => ImageBitmap);
                    RaisePropertyChanging(() => GlyphBitmap);
                    _glyph.Width = value;
                    RaisePropertyChanged(() => Width);
                    RaisePropertyChanged(() => ImageBitmap);
                    RaisePropertyChanged(() => GlyphBitmap);
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
                    RaisePropertyChanging(() => ImageBitmap);
                    RaisePropertyChanging(() => GlyphBitmap);
                    _glyph.Height = value;
                    RaisePropertyChanged(() => Height);
                    RaisePropertyChanged(() => ImageBitmap);
                    RaisePropertyChanged(() => GlyphBitmap);
                }
            }
        }

        public float WidthInPoints
        {
            get { return _glyph.WidthInPoints; }
            set
            {
                if (_glyph.WidthInPoints != value)
                {
                    RaisePropertyChanging(() => WidthInPoints);
                    _glyph.WidthInPoints = value;
                    RaisePropertyChanged(() => WidthInPoints);
                }
            }
        }

        public float FloatWidth
        {
            get { return _glyph.FloatWidth; }
            set
            {
                if (_glyph.FloatWidth != value)
                {
                    RaisePropertyChanging(() => FloatWidth);
                    _glyph.FloatWidth = value;
                    RaisePropertyChanged(() => FloatWidth);
                }
            }
        }

        public float HeightInPoints
        {
            get { return _glyph.HeightInPoints; }
            set
            {
                if (_glyph.HeightInPoints != value)
                {
                    RaisePropertyChanging(() => HeightInPoints);
                    _glyph.HeightInPoints = value;
                    RaisePropertyChanged(() => HeightInPoints);
                }
            }
        }

        public float FloatHeight
        {
            get { return _glyph.FloatHeight; }
            set
            {
                if (_glyph.FloatHeight != value)
                {
                    RaisePropertyChanging(() => FloatHeight);
                    _glyph.FloatHeight = value;
                    RaisePropertyChanged(() => FloatHeight);
                }
            }
        }

        public BitmapSource GlyphBitmap
        {
            get { return _glyph.GlyphBitmap; }
            set
            {
                if (_glyph.GlyphBitmap != value)
                {
                    RaisePropertyChanging(() => GlyphBitmap);
                    _glyph.GlyphBitmap = value;
                    RaisePropertyChanged(() => GlyphBitmap);
                }
            }
        }

        public BitmapSource ImageBitmap
        {
            get { return _glyph.ImageBitmap; }
        }

        private RelayCommand _exportCommand;

        /// <summary>
        /// Gets the ExportCommand.
        /// </summary>
        public RelayCommand ExportCommand
        {
            get
            {
                return _exportCommand
                    ?? (_exportCommand = new RelayCommand(ExecuteExportCommand));
            }
        }

        public GlyphViewModel(Glyph glyph)
        {
            _glyph = glyph;
        }

        private void ExecuteExportCommand()
        {
            var dlg = new SaveFileDialog();
            dlg.Filter = "PNG images (.png)|*.png";

            if (dlg.ShowDialog() == true)
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(_glyph.GlyphBitmap));

                using (Stream stream = dlg.OpenFile())
                {
                    encoder.Save(stream);
                }
            }
        }

        private RelayCommand _replaceCommand;

        /// <summary>
        /// Gets the ReplaceCommand.
        /// </summary>
        public RelayCommand ReplaceCommand
        {
            get
            {
                return _replaceCommand
                    ?? (_replaceCommand = new RelayCommand(ExecuteReplaceCommand));
            }
        }

        private void ExecuteReplaceCommand()
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "PNG images (.png)|*.png";

            if (dlg.ShowDialog() == true)
            {
                using (Stream stream = dlg.OpenFile())
                {
                    BitmapDecoder decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    BitmapFrame frame = decoder.Frames[0];

                    _glyph.GlyphBitmap = frame;
                }
            }
        }
    }
}
