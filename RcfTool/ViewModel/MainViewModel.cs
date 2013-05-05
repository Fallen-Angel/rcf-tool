using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

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
        private string _name = "Microgramma";
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    RaisePropertyChanging(() => Name);
                    _name = value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }

        private int _version = 1;
        public int Version
        {
            get { return _version; }
            set
            {
                if (_version != value)
                {
                    RaisePropertyChanging(() => Version);
                    _version = value;
                    RaisePropertyChanged(() => Version);
                }
            }
        }

        private string _charset = "abcde";
        public string Charset
        {
            get { return _charset; }
            set
            {
                if (_charset != value)
                {
                    RaisePropertyChanging(() => Charset);
                    _charset = value;
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
            _typefaces.Add(new TypefaceViewModel() { Name = "12" });
            _typefaces.Add(new TypefaceViewModel() { Name = "16" });
            _typefaces.Add(new TypefaceViewModel() { Name = "24" });
        }
    }
}