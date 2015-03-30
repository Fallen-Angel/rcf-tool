using System.Collections.ObjectModel;
using System.Windows.Input;
using Homeworld2.RCF;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Win32;

namespace RcfTool.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private const string RcfFilter = "Relic fonts (.rcf)|*.rcf";
        private readonly RCF _font = new RCF();

        public string Name
        {
            get { return _font.Name; }
            set
            {
                if (_font.Name != value)
                {
                    _font.Name = value;
                    OnPropertyChanged(nameof(Name));
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
                    _font.Version = value;
                    OnPropertyChanged(nameof(Version));
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
                    _font.Charset = value;
                    OnPropertyChanged(nameof(Charset));
                }
            }
        }

        private TypefaceViewModel _selectedTypeface;
        public TypefaceViewModel SelectedTypeface
        {
            get { return _selectedTypeface; }
            set
            {
                if (_selectedTypeface != value)
                {
                    _selectedTypeface = value;
                    OnPropertyChanged(nameof(SelectedTypeface));
                }
            }
        }

        public ObservableCollection<TypefaceViewModel> Typefaces { get; } = new ObservableCollection<TypefaceViewModel>();

        private DelegateCommand _openCommand;

        /// <summary>
        /// Gets the OpenCommand.
        /// </summary>
        public ICommand OpenCommand => _openCommand ?? (_openCommand = new DelegateCommand(ExecuteOpenCommand));

        private void ExecuteOpenCommand()
        {
            var dlg = new OpenFileDialog { Filter = RcfFilter };

            if (dlg.ShowDialog() == true)
            {
                using (var stream = dlg.OpenFile())
                {
                    _font.Read(stream);
                    
                    OnPropertyChanged(nameof(Name));
                    OnPropertyChanged(nameof(Version));
                    OnPropertyChanged(nameof(Charset));

                    Typefaces.Clear();

                    foreach (var typeface in _font.Typefaces)
                    {

                        Typefaces.Add(new TypefaceViewModel(typeface));
                    }
                }
            }
        }

        private DelegateCommand _saveCommand;

        /// <summary>
        /// Gets the SaveCommand.
        /// </summary>
        public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(ExecuteSaveCommand));

        private void ExecuteSaveCommand()
        {
            var dlg = new SaveFileDialog { Filter = RcfFilter };

            if (dlg.ShowDialog() == true)
            {
                using (var stream = dlg.OpenFile())
                {
                    _font.Write(stream);
                }
            }
        }
    }
}