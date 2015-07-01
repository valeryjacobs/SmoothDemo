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

        private string _agentAddress;

        public string AgentAddress
        {
            get { return _agentAddress; }
            set
            {
                _agentAddress = value;
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

        private Windows.UI.Xaml.Visibility _popupVisible;

        public Windows.UI.Xaml.Visibility PopupVisible
        {
            get { return _popupVisible; }
            set
            {
                _popupVisible = value;
                NotifyPropertyChanged();
            }
        }

        private string _status;

        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;

                if (_status == null || _status == "")
                {
                    PopupVisible = Windows.UI.Xaml.Visibility.Collapsed;
                }
                else
                {
                    PopupVisible = Windows.UI.Xaml.Visibility.Visible;
                }

                NotifyPropertyChanged();
            }
        }

        private bool _connected;

        public bool Connected
        {
            get { return _connected; }
            set
            {
                _connected = value;
                NotifyPropertyChanged();
            }
        }

        public async void Init()
        {
            Connected = false;
            Status = "Connecting...";
            hubConnection = new HubConnection(AgentAddress);
            
            proxy = hubConnection.CreateHubProxy("ControlHub");

            proxy.On<List<Models.Action>>("updateActionList", (list) =>
            {
                ActionList = new ObservableCollection<Models.Action>(list);
                Status = string.Format("Action list updated with {0} items.", ActionList.Count);
            });

            proxy.On<int>("updateActionIndex", (index) =>
            {
                CurrentAction = ActionList[index];
            });

            try
            {
                Status = "About to start hub connection...";

                await hubConnection.Start();
                Status = "Connected to hub with id " + hubConnection.ConnectionId;

                Connected = true;

                RequestActionList();
            }
            catch (Exception ex)
            {
                if (ex is System.Net.Http.HttpRequestException)
                {
                    Status = "Hub not online or not reachable. Check connection settings";
                }
            }

        }

        public void RequestActionList()
        {
            Invoke("RequestActionList");
        }

        private void Invoke(string invokeFunction)
        {
            if (hubConnection.State == ConnectionState.Disconnected)
            {
                hubConnection = new HubConnection(AgentAddress);
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
            proxy.Invoke("HandleCommand", new Command() { Name = commandName, Parameters = parameters });
        }

        public void ToggleSwitch(int switchId)
        {
            if (hubConnection.State == ConnectionState.Disconnected)
            {
                hubConnection = new HubConnection(AgentAddress);
                proxy = hubConnection.CreateHubProxy("ControlHub");
            }

            proxy.Invoke("ToggleSwitch", switchId);
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
