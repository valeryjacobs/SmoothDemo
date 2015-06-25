using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;

namespace SmoothDemo.UniversalClient.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private HubConnection hubConnection;
        private IHubProxy proxy;
        private ObservableCollection<Models.Action> _actionList;

        public ObservableCollection<Models.Action> ActionList
        {
            get { return _actionList; }
            set
            {
                _actionList = value;
                NotifyPropertyChanged();
            }
        }

        private Models.Action _currentAction;

        public Models.Action CurrentAction
        {
            get { return _currentAction; }
            set
            {
                _currentAction = value;
                NotifyPropertyChanged();
            }
        }


        public void Init()
        {
            hubConnection = new HubConnection("http://192.168.178.13:8080");
            proxy = hubConnection.CreateHubProxy("ControlHub");

            proxy.On<string, string>("broadcastMessage", (name, message) =>
            {
                Debug.WriteLine(name + ": ");
                Debug.WriteLine(message);
            });

            proxy.On<string>("ping", (id) =>
            {
                Debug.WriteLine(id);
            });

            proxy.On<List<Models.Action>>("updateActionList", (list) =>
            {
                ActionList = new ObservableCollection<Models.Action>(list);
            });

            proxy.On<int>("updateActionIndex", (index) =>
            {
                // ActionList.Where(x => x.Current == true).All(x=>x.Current = false);

                CurrentAction = ActionList[index];
            });

            hubConnection.Start().Wait();

            RequestActionList();
        }

        public void RequestActionList()
        {
            Invoke("RequestActionList");
        }

        private void Invoke(string invokeFunction)
        {
            if (hubConnection.State == ConnectionState.Disconnected)
            {
                hubConnection = new HubConnection("http://192.168.178.13:8080");
                proxy = hubConnection.CreateHubProxy("ControlHub");
            }

            proxy.Invoke(invokeFunction).Wait();
        }

        public void Ping()
        {
            proxy.Invoke("Ping", hubConnection.ConnectionId);
        }

        public void SendCommand(string commandName)
        {
            SendCommand(commandName, null);
        }

        public void SendCommand(string commandName, List<string> parameters)
        {
            proxy.Invoke("HandleCommand", new Command() { Name = "Next", Parameters = parameters });
        }

        public class Command
        {
            public string Name { get; set; }
            public List<string> Parameters { get; set; }
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
