using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Homeworld2.RCF;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

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

                    _glyphs.Clear();
                    foreach (Glyph glyph in _typeface.Glyphs)
                    {
                        GlyphViewModel vm = new GlyphViewModel();
                        vm.Glyph = glyph;
                        _glyphs.Add(vm);
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
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "PNG images (.png)|*.png";

            if (dlg.ShowDialog() == true)
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image.Bitmap));

                using (Stream stream = dlg.OpenFile())
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
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "PNG images (.png)|*.png";

            if (dlg.ShowDialog() == true)
            {
                using (Stream stream = dlg.OpenFile())
                {
                    BitmapDecoder decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    BitmapFrame frame = decoder.Frames[0];

                    image.Bitmap = frame;
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
        }
    }
}
