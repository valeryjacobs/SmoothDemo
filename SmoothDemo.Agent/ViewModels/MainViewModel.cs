using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SmoothDemo.Agent
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            Actions = new List<SmoothDemo.Agent.Models.Action>();
            Tokens = new List<string>();
            Log = new List<string>();
        }

        public List<SmoothDemo.Agent.Models.Action> Actions { get; set; }
        public List<string> Tokens { get; set; }
        public int ActionIndex { get; set; }
        public string ContentToShow { get; set; }
        public string ScriptFileName { get; set; }

        private string _statusMessage;

        private List<string> _log;

        public List<string> Log
        {
            get { return _log; }
            set { _log = value; }
        }



        public string StatusMessage
        {
            get { return _statusMessage; }
            set
            {
                _statusMessage = value;

                Log.Add(value);

                NotifyPropertyChanged();
            }
        }

        public void Init()
        {
            Tokens = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(ConfigurationManager.AppSettings["tokensFileName"]));
            Actions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SmoothDemo.Agent.Models.Action>>(File.ReadAllText(ConfigurationManager.AppSettings["scriptFileName"]));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
