using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RcfTool.ViewModel
{
    public class GlyphViewModel : ViewModelBase
    {
        private char _character = 'd';
        public char Character
        {
            get { return _character; }
            set
            {
                if (_character != value)
                {
                    RaisePropertyChanging(() => Character);
                    _character = value;
                    RaisePropertyChanged(() => Character);
                }
            }
        }
    }
}
