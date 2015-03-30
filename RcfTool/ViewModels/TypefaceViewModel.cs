using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Homeworld2.RCF;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Win32;

namespace RcfTool.ViewModels
{
    public class TypefaceViewModel : BindableBase
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
                    _typeface.Name = value;
                    OnPropertyChanged(nameof(Name));
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
                    _typeface.Attributes = value;
                    OnPropertyChanged(nameof(Attributes));
                }
            }
        }

        private GlyphViewModel _selectedGlyph;
        public GlyphViewModel SelectedGlyph
        {
            get { return _selectedGlyph; }
            set { SetProperty(ref _selectedGlyph, value); }
        }

        public ObservableCollection<ImageViewModel> Images { get; } = new ObservableCollection<ImageViewModel>();

        public ObservableCollection<GlyphViewModel> Glyphs { get; } = new ObservableCollection<GlyphViewModel>();

        private DelegateCommand _importCommand;

        /// <summary>
        /// Gets the ImportCommand.
        /// </summary>
        public ICommand ImportCommand => _importCommand ?? (_importCommand = new DelegateCommand(ExecuteImportCommand));

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
            Images.Add(imageViewModel);
        }

        private DelegateCommand _addGlyphCommand;

        /// <summary>
        /// Gets the AddGlyphCommand.
        /// </summary>
        public ICommand AddGlyphCommand => _addGlyphCommand ?? (_addGlyphCommand = new DelegateCommand(ExecuteAddGlyphCommand));

        private void ExecuteAddGlyphCommand()
        {
            var glyph = new Glyph();
            _typeface.Glyphs.Add(glyph);

            var vm = new GlyphViewModel(glyph, this);
            Glyphs.Add(vm);
            SelectedGlyph = vm;
        }

        private DelegateCommand<ImageViewModel> _exportCommand;

        /// <summary>
        /// Gets the ExportCommand.
        /// </summary>
        public DelegateCommand<ImageViewModel> ExportCommand => _exportCommand ?? (_exportCommand = new DelegateCommand<ImageViewModel>(ExecuteExportCommand));

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

        private DelegateCommand<ImageViewModel> _replaceCommand;

        /// <summary>
        /// Gets the ReplaceCommand.
        /// </summary>
        public DelegateCommand<ImageViewModel> ReplaceCommand => _replaceCommand ?? (_replaceCommand = new DelegateCommand<ImageViewModel>(ExecuteReplaceCommand));

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

            Images.Clear();
            foreach (var image in _typeface.Images)
            {
                Images.Add(new ImageViewModel(image));
            }

            Glyphs.Clear();
            foreach (var glyph in _typeface.Glyphs)
            {
                Glyphs.Add(new GlyphViewModel(glyph, this));
            }
        }
    }
}
