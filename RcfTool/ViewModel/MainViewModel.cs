using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Homeworld2.RCF;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;

namespace RcfTool.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private RCF _font = new RCF();

        public string Name
        {
            get { return _font.Name; }
            set
            {
                if (_font.Name != value)
                {
                    RaisePropertyChanging(() => Name);
                    _font.Name = value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }

        public int Version
        {
            get { return _font.Version; }
            set
            {
                if (_font.Version != value)
                {
                    RaisePropertyChanging(() => Version);
                    _font.Version = value;
                    RaisePropertyChanged(() => Version);
                }
            }
        }

        public string Charset
        {
            get { return _font.Charset; }
            set
            {
                if (_font.Charset != value)
                {
                    RaisePropertyChanging(() => Charset);
                    _font.Charset = value;
                    RaisePropertyChanged(() => Charset);
                }
            }
        }

        private TypefaceViewModel _selectedTypeface = new TypefaceViewModel();
        public TypefaceViewModel SelectedTypeface
        {
            get { return _selectedTypeface; }
            set
            {
                if (_selectedTypeface != value)
                {
                    RaisePropertyChanging(() => SelectedTypeface);
                    _selectedTypeface = value;
                    RaisePropertyChanged(() => SelectedTypeface);
                }
            }
        }

        private ObservableCollection<TypefaceViewModel> _typefaces = new ObservableCollection<TypefaceViewModel>();
        public ObservableCollection<TypefaceViewModel> Typefaces
        {
            get { return _typefaces; }
            set
            {
                if (_typefaces != value)
                {
                    RaisePropertyChanging(() => Typefaces);
                    _typefaces = value;
                    RaisePropertyChanged(() => Typefaces);
                }
            }
        }

        private RelayCommand _openCommand;

        /// <summary>
        /// Gets the OpenCommand.
        /// </summary>
        public RelayCommand OpenCommand
        {
            get
            {
                return _openCommand
                    ?? (_openCommand = new RelayCommand(ExecuteOpenCommand));
            }
        }

        private void ExecuteOpenCommand()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Relic fonts (.rcf)|*.rcf";

            if (dlg.ShowDialog() == true)
            {
                using (Stream stream = dlg.OpenFile())
                {
                    _font.Read(stream);

                    RaisePropertyChanged(() => Name);
                    RaisePropertyChanged(() => Version);
                    RaisePropertyChanged(() => Charset);

                    _typefaces.Clear();

                    foreach (Typeface typeface in _font.Typefaces)
                    {
                        TypefaceViewModel vm = new TypefaceViewModel();
                        vm.Typeface = typeface;
                        _typefaces.Add(vm);
                    }
                }
            }
        }

        private RelayCommand _saveCommand;

        /// <summary>
        /// Gets the SaveCommand.
        /// </summary>
        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand
                    ?? (_saveCommand = new RelayCommand(ExecuteSaveCommand));
            }
        }

        private void ExecuteSaveCommand()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Relic fonts (.rcf)|*.rcf";

            if (dlg.ShowDialog() == true)
            {
                using (Stream stream = dlg.OpenFile())
                {
                    _font.Write(stream);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
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