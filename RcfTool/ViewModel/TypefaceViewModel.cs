using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Homeworld2.RCF;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media.Imaging;

namespace RcfTool.ViewModel
{
    public class TypefaceViewModel : ViewModelBase
    {
        private const string PngFilter = "PNG images (.png)|*.png";
        private readonly Typeface _typeface;

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

        private GlyphViewModel _selectedGlyph;
        public GlyphViewModel SelectedGlyph
        {
            get { return _selectedGlyph; }
            set
            {
                if (_selectedGlyph != value)
                {
                    RaisePropertyChanging(() => SelectedGlyph);
                    _selectedGlyph = value;
                    RaisePropertyChanged(() => SelectedGlyph);
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

        private readonly ObservableCollection<GlyphViewModel> _glyphs = new ObservableCollection<GlyphViewModel>();
        public ObservableCollection<GlyphViewModel> Glyphs
        {
            get { return _glyphs; }
        }

        private RelayCommand _importCommand;

        /// <summary>
        /// Gets the ImportCommand.
        /// </summary>
        public RelayCommand ImportCommand
        {
            get
            {
                return _importCommand
                    ?? (_importCommand = new RelayCommand(ExecuteImportCommand));
            }
        }

        private void ExecuteImportCommand()
        {
            var image = new Image();
            var imageViewModel = new ImageViewModel(image);

            var dlg = new OpenFileDialog { Filter = PngFilter };

            if (dlg.ShowDialog() == true)
            {
                using (var stream = dlg.OpenFile())
                {
                    BitmapDecoder decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    var frame = decoder.Frames[0];

                    imageViewModel.Bitmap = frame;
                }
            }
            _typeface.Images.Add(image);
            _images.Add(imageViewModel);
        }

        private RelayCommand _addGlyphCommand;

        /// <summary>
        /// Gets the AddGlyphCommand.
        /// </summary>
        public RelayCommand AddGlyphCommand
        {
            get
            {
                return _addGlyphCommand
                    ?? (_addGlyphCommand = new RelayCommand(ExecuteAddGlyphCommand));
            }
        }

        private void ExecuteAddGlyphCommand()
        {
            var glyph = new Glyph();
            _typeface.Glyphs.Add(glyph);

            var vm = new GlyphViewModel(glyph, this);
            _glyphs.Add(vm);
            SelectedGlyph = vm;
        }

        private RelayCommand<ImageViewModel> _exportCommand;

        /// <summary>
        /// Gets the ExportCommand.
        /// </summary>
        public RelayCommand<ImageViewModel> ExportCommand
        {
            get
            {
                return _exportCommand
                    ?? (_exportCommand = new RelayCommand<ImageViewModel>(ExecuteExportCommand));
            }
        }

        private void ExecuteExportCommand(ImageViewModel image)
        {
            var dlg = new SaveFileDialog { Filter = PngFilter };

            if (dlg.ShowDialog() == true)
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image.Bitmap));

                using (var stream = dlg.OpenFile())
                {
                    encoder.Save(stream);
                }
            }
        }

        private RelayCommand<ImageViewModel> _replaceCommand;

        /// <summary>
        /// Gets the ReplaceCommand.
        /// </summary>
        public RelayCommand<ImageViewModel> ReplaceCommand
        {
            get
            {
                return _replaceCommand
                    ?? (_replaceCommand = new RelayCommand<ImageViewModel>(ExecuteReplaceCommand));
            }
        }

        private void ExecuteReplaceCommand(ImageViewModel image)
        {
            var dlg = new OpenFileDialog { Filter = PngFilter };

            if (dlg.ShowDialog() == true)
            {
                using (Stream stream = dlg.OpenFile())
                {
                    BitmapDecoder decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    var frame = decoder.Frames[0];

                    image.Bitmap = frame;
                }
            }
        }

        public TypefaceViewModel(Typeface typeface)
        {
            _typeface = typeface;

            _images.Clear();
            foreach (var image in _typeface.Images)
            {
                _images.Add(new ImageViewModel(image));
            }

            _glyphs.Clear();
            foreach (var glyph in _typeface.Glyphs)
            {
                _glyphs.Add(new GlyphViewModel(glyph, this));
            }
        }
    }
}
