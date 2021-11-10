using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace odtwarzacz
{
    class AudioPlayerLoadedBehavior : INotifyPropertyChanged
    {
        private MediaState _loadedBehavior;
        public MediaState LoadedBehavior
        {
            get
            {
                return _loadedBehavior;
            }
            set
            {
                _loadedBehavior = value;

                raiseEventThatPropertyChanged("LoadedBehavior");
            }
        }

        public void raiseEventThatPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("LoadedBehavior"));
        }

        public event PropertyChangedEventHandler PropertyChanged;


    }
}
