using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;

namespace SmoothDemo.UniversalClient.Models
{
    public class Action : INotifyPropertyChanged
    {
        public string Type { get; set; }
        public object Content { get; set; }
        public bool AutoContinue { get; set; }
        public string Description { get; set; }
        private bool _current;

        public bool Current
        {
            get { return _current; }
            set { _current = value;
                NotifyPropertyChanged();
            }
        }
        

        public event PropertyChangedEventHandler PropertyChanged;
        private async void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                });
            }
        }
    }
}
