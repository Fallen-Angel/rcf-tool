using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RcfTool.ViewModel
{
    public class TypefaceViewModel : ViewModelBase
    {
        private string _name = "Microgramma 24";
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

        private string _attributes = "dadad";
        public string Attributes
        {
            get { return _attributes; }
            set
            {
                if (_attributes != value)
                {
                    RaisePropertyChanging(() => Attributes);
                    _attributes = value;
                    RaisePropertyChanged(() => Attributes);
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
