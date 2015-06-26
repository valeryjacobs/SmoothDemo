using Microsoft.AspNet.SignalR;
using SmoothDemo.Agent.Hubs;
using SmoothDemo.Agent.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WindowsInput;

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

        public List<Models.Action> Actions { get; set; }
        public List<string> Tokens { get; set; }
        public int ActionIndex { get; set; }
        public string ScriptFileName { get; set; }

        private string _statusMessage;

        private List<string> _log;

        public List<string> Log
        {
            get { return _log; }
            set { _log = value; }
        }

        public ControlHub Hub { get; set; }


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

        private string _contentToShow;

        public string ContentToShow
        {
            get { return _contentToShow; }
            set
            {
                _contentToShow = value;
                NotifyPropertyChanged();
            }
        }


        private OSHelper _osHelper;

        public OSHelper OS
        {
            get { return _osHelper; }
            set { _osHelper = value; }
        }


        public void Init()
        {
            Tokens = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(ConfigurationManager.AppSettings["tokensFileName"]));
            Actions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SmoothDemo.Agent.Models.Action>>(File.ReadAllText(ConfigurationManager.AppSettings["scriptFileName"]));

            OS = new OSHelper();
            OS.Tokens = Tokens;
        }


        public void Skip()
        {
            ActionIndex++;
            Hub.SetActionIndex(ActionIndex);
        }

        public void Next()
        {
            try
            {
                if (ActionIndex < 0) return;

                if (ActionIndex > Actions.Count - 1) return;

                if (ActionIndex >= Actions.Count) return;

                OS.ExecuteAction(Actions[ActionIndex]);

                ActionIndex++;
                Hub.SetActionIndex(ActionIndex);

                if (Actions[ActionIndex - 1].AutoContinue)
                    Next();
            }
            catch
            {
                OS.CloseOpenedWindows();
            }
        }

        public void HandleCommand(Command command)
        {
            switch (command.Name.ToLower())
            {
                case "next":
                    Next();
                    break;
                case "previous":
                    Previous();
                    break;
                case "init":
                    Init();
                    break;
                case "restart":
                    Restart();
                    break;
                case "skip":
                    Skip();
                    break;
            }
        }

        public void Previous()
        {
            if (ActionIndex > 1)
            {
                ActionIndex -= 2;
                
                Next();
            }
        }

        public void Restart()
        {
            //Check older version if this causes issues;
            OS.CloseOpenedWindows();

            Init();

            ActionIndex = 0;
            Hub.SetActionIndex(ActionIndex);
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
