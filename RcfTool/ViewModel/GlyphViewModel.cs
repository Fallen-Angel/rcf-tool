using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Homeworld2.RCF;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Typeface = Homeworld2.RCF.Typeface;

namespace RcfTool.ViewModel
{
    public class GlyphViewModel : ViewModelBase
    {
        public Int32Rect CropRect { get; set; }

        private void GenerateCropRect()
        {
            CropRect = new Int32Rect(_glyph.LeftMargin, _glyph.TopMargin, _glyph.Width, _glyph.Height);
            _croppedBitmap = new CroppedBitmap(Image.Bitmap, CropRect);
        }

        private ImageViewModel Image
        {
            get { return _typefaceViewModel.Images[_glyph.ImageIndex]; }
        }

        private CroppedBitmap _croppedBitmap;

        private readonly Glyph _glyph;
        private readonly TypefaceViewModel _typefaceViewModel;

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
                    GenerateCropRect();
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
                    GenerateCropRect();
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
                    GenerateCropRect();
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
                    GenerateCropRect();
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
                    GenerateCropRect();
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
            get { return _croppedBitmap; }
            set
            {
                if (_croppedBitmap != value)
                {
                    RaisePropertyChanging(() => GlyphBitmap);


                    if (!Equals(_croppedBitmap, value) &&
                        ((value.PixelWidth == _croppedBitmap.PixelWidth) &&
                         (value.PixelHeight == _croppedBitmap.PixelHeight)))
                    {
                        if (value.Format != PixelFormats.Gray8)
                        {
                            value = new FormatConvertedBitmap(value, PixelFormats.Gray8, BitmapPalettes.Gray256, 0);
                        }

                        var data = new byte[value.PixelWidth * value.PixelHeight];
                        value.CopyPixels(data, value.PixelWidth, 0);

                        Image.ModifyBitmap(CropRect, data, value.PixelWidth);
                        GenerateCropRect();
                    }


                    RaisePropertyChanged(() => GlyphBitmap);
                }
            }
        }

        public BitmapSource ImageBitmap
        {
            get { return Image.Bitmap; }
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

        public GlyphViewModel(Glyph glyph, TypefaceViewModel typefaceViewModel)
        {
            _glyph = glyph;
            _typefaceViewModel = typefaceViewModel;
            GenerateCropRect();
        }

        private void ExecuteExportCommand()
        {
            var dlg = new SaveFileDialog { Filter = "PNG images (.png)|*.png" };

            if (dlg.ShowDialog() == true)
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(GlyphBitmap));

                using (var stream = dlg.OpenFile())
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
            var dlg = new OpenFileDialog { Filter = "PNG images (.png)|*.png" };

            if (dlg.ShowDialog() == true)
            {
                using (var stream = dlg.OpenFile())
                {
                    BitmapDecoder decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    var frame = decoder.Frames[0];

                    GlyphBitmap = frame;
                }
            }
        }
    }
}
