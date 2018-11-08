using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using SmoothDemo.Agent.Hubs;
using SmoothDemo.Agent.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
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

        public HubConnection Connection { get; set; }

        public List<Models.Action> Actions { get; set; }
        public List<string> Tokens { get; set; }
        public int ActionIndex { get; set; }
        public string ScriptFileName { get; set; }

        private string _statusMessage;

        private MediaPlayer _mediaPlayer = new MediaPlayer();

        private List<string> _log;

        public List<string> Log
        {
            get { return _log; }
            set { _log = value; }
        }

        public MediaPlayer MediaPlayer
        {
            get { return _mediaPlayer; }
            set { _mediaPlayer = value; }
        }

        //public ControlHub Hub { get; set; }


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


        public async Task Init()
        {
            Connection = new HubConnectionBuilder()
           .WithUrl("https://smoothdemowebremote.azurewebsites.net/ChatHub")
           .Build();

            //Connection.ServerTimeout = new TimeSpan(0, 0, 4);
            Connection.Closed += Connection_Closed;

            

            // Connection = new HubConnectionBuilder()
            //.WithUrl("https://localhost:44316//ChatHub")
            //.Build();

            Connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                StatusMessage = message;
                var parms = message.Split(':');
                var parm = 0;

                if(parms.Length == 2)
                {
                    parm = int.Parse(parms[1]);
                }
                var dispatcher = Application.Current.Dispatcher;
                dispatcher.Invoke(() => App.MainViewModel.HandleCommand(new Command() { Name = parms[0], Parameter = parm }));


                //this.Dispatcher.Invoke(() =>
                //{
                //    var newMessage = $"{user}: {message}";
                //    messagesList.Items.Add(newMessage);
                //});
            });

            Connection.On("RequestActionList", () =>
            {
                StatusMessage = "Action list requested at " + DateTime.Now.TimeOfDay;
                Connection.InvokeAsync("ReceiveActionList", JsonConvert.SerializeObject(App.MainViewModel.Actions));
            });

            try
            {
                await Connection.StartAsync();
                //messagesList.Items.Add("Connection started");

            }
            catch (Exception ex)
            {
                //messagesList.Items.Add(ex.Message);
            }

            Tokens = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(ConfigurationManager.AppSettings["tokensFileName"]));
            Actions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SmoothDemo.Agent.Models.Action>>(File.ReadAllText(ConfigurationManager.AppSettings["scriptFileName"]));

            //var json = new WebClient().DownloadString(ConfigurationManager.AppSettings["scriptFileName"]);
            //Actions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SmoothDemo.Agent.Models.Action>>(json);

            OS = new OSHelper();
            OS.Tokens = Tokens;
        }

        private async Task Connection_Closed(Exception arg)
        {
            StatusMessage = "Connection lost. about to reconnect.";
            await Connection.StartAsync();

            StatusMessage = "Restarted connection.";
        }

        public void Skip()
        {
            ActionIndex++;
            //Hub.SetActionIndex(ActionIndex);
        }

        public void Execute(int index)
        {
            try
            {
                OS.ExecuteAction(Actions[index]);
                //Hub.SetActionIndex(ActionIndex);
            }
            catch
            {
                OS.CloseOpenedWindows();
            }
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
                //Hub.SetActionIndex(ActionIndex);

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
                case "execute":
                    Execute(command.Parameter);
                    break;
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
                case "reload":
                    Reload();
                    break;
            }
        }

        public void Reload()
        {
            ActionIndex = 0;
            //var json = new WebClient().DownloadString(ConfigurationManager.AppSettings["scriptFileName"]);
            //Actions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SmoothDemo.Agent.Models.Action>>(json);
            Actions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SmoothDemo.Agent.Models.Action>>(File.ReadAllText(ConfigurationManager.AppSettings["scriptFileName"]));

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
            //Hub.SetActionIndex(ActionIndex);
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
