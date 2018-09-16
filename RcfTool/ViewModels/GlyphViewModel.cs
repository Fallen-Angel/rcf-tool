using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Homeworld2.RCF;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Win32;

namespace RcfTool.ViewModels
{
    public class GlyphViewModel : BindableBase
    {
        private const string PngFilter = "PNG images (.png)|*.png";

        public Int32Rect CropRect { get; set; }

        private void GenerateCropRect()
        {
            CropRect = new Int32Rect(_glyph.LeftMargin, _glyph.TopMargin, _glyph.Width, _glyph.Height);
            _croppedBitmap = new CroppedBitmap(Image.Bitmap, CropRect);
        }

        private ImageViewModel Image => _typefaceViewModel.Images[_glyph.ImageIndex];

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
                    _glyph.Character = value;
                    OnPropertyChanged(nameof(Character));
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
                    _glyph.ImageIndex = value - 1;
                    GenerateCropRect();
                    OnPropertyChanged(nameof(ImageIndex));
                    OnPropertyChanged(nameof(ImageBitmap));
                    OnPropertyChanged(nameof(GlyphBitmap));
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
                    _glyph.LeftMargin = value;
                    GenerateCropRect();
                    OnPropertyChanged(nameof(LeftMargin));
                    OnPropertyChanged(nameof(ImageBitmap));
                    OnPropertyChanged(nameof(GlyphBitmap));
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
                    _glyph.TopMargin = value;
                    GenerateCropRect();
                    OnPropertyChanged(nameof(TopMargin));
                    OnPropertyChanged(nameof(ImageBitmap));
                    OnPropertyChanged(nameof(GlyphBitmap));
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
                    _glyph.Width = value;
                    GenerateCropRect();
                    OnPropertyChanged(nameof(Width));
                    OnPropertyChanged(nameof(ImageBitmap));
                    OnPropertyChanged(nameof(GlyphBitmap));
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
                    _glyph.Height = value;
                    GenerateCropRect();
                    OnPropertyChanged(nameof(Height));
                    OnPropertyChanged(nameof(ImageBitmap));
                    OnPropertyChanged(nameof(GlyphBitmap));
                }
            }
        }

        public float WidthInPoints
        {
            get { return _glyph.BitmapRight; }
            set
            {
                if (_glyph.BitmapRight != value)
                {
                    _glyph.BitmapRight = value;
                    OnPropertyChanged(nameof(WidthInPoints));
                }
            }
        }

        public float FloatWidth
        {
            get { return _glyph.AdvanceX; }
            set
            {
                if (_glyph.AdvanceX != value)
                {
                    _glyph.AdvanceX = value;
                    OnPropertyChanged(nameof(FloatWidth));
                }
            }
        }

        public float HeightInPoints
        {
            get { return _glyph.Baseline; }
            set
            {
                if (_glyph.Baseline != value)
                {
                    _glyph.Baseline = value;
                    OnPropertyChanged(nameof(HeightInPoints));
                }
            }
        }

        public float FloatHeight
        {
            get { return _glyph.BitmapBottom; }
            set
            {
                if (_glyph.BitmapBottom != value)
                {
                    _glyph.BitmapBottom = value;
                    OnPropertyChanged(nameof(FloatHeight));
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

                    OnPropertyChanged(nameof(GlyphBitmap));
                }
            }
        }

        public BitmapSource ImageBitmap => Image.Bitmap;

        private DelegateCommand _exportCommand;

        /// <summary>
        /// Gets the ExportCommand.
        /// </summary>
        public ICommand ExportCommand => _exportCommand ?? (_exportCommand = new DelegateCommand(ExecuteExportCommand));

        public GlyphViewModel(Glyph glyph, TypefaceViewModel typefaceViewModel)
        {
            _glyph = glyph;
            _typefaceViewModel = typefaceViewModel;
            GenerateCropRect();
        }

        private void ExecuteExportCommand()
        {
            var dlg = new SaveFileDialog { Filter = PngFilter };

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

        private DelegateCommand _replaceCommand;

        /// <summary>
        /// Gets the ReplaceCommand.
        /// </summary>
        public ICommand ReplaceCommand => _replaceCommand ?? (_replaceCommand = new DelegateCommand(ExecuteReplaceCommand));

        private void ExecuteReplaceCommand()
        {
            var dlg = new OpenFileDialog { Filter = PngFilter };

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
