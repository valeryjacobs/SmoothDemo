using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Devices.Gpio;

namespace SmoothDemo.UniversalSwitcher.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private HubConnection hubConnection;
        private IHubProxy proxy;

        private const int SWITCHA_PIN = 5;
        private const int SWITCHB_PIN = 6;
        private GpioPin pinA;
        private GpioPin pinB;
        private GpioPinValue pinValueA, pinValueB;
        GpioController gpio;

        public MainViewModel()
        {
            gpio = GpioController.GetDefault();
            pinA = gpio.OpenPin(SWITCHA_PIN);
            pinB = gpio.OpenPin(SWITCHB_PIN);
            pinValueA = GpioPinValue.Low;
            pinValueB = GpioPinValue.Low;
            pinA.Write(pinValueA);
            pinB.Write(pinValueB);
            pinA.SetDriveMode(GpioPinDriveMode.Output);
            pinB.SetDriveMode(GpioPinDriveMode.Output);
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

        private bool _swithA;

        public bool SwitchA
        {
            get { return _swithA; }
            set
            {
                _swithA = value;
                pinValueA = value ? GpioPinValue.High : GpioPinValue.Low;
                pinA.Write(pinValueA);
                NotifyPropertyChanged();
            }
        }
        private bool _switchB;

        public bool SwitchB
        {
            get { return _switchB; }
            set
            {
                _switchB = value;
                pinValueB = value ? GpioPinValue.High : GpioPinValue.Low;
                pinB.Write(pinValueB);
                NotifyPropertyChanged();
            }
        }

        public async void Init()
        {

            Connected = false;
            Status = "Connecting...";
            hubConnection = new HubConnection(AgentAddress);

            proxy = hubConnection.CreateHubProxy("ControlHub");

            proxy.On<int>("toggleSwitch", (_switch) =>
             {
                 if (_switch == 1)
                 {
                     SwitchA = !SwitchA;

                     Status = "Switch A set to " + SwitchA.ToString();
                 }
                 else if (_switch == 2)
                 {
                     SwitchB = !SwitchB;

                     Status = "Switch B set to " + SwitchB.ToString();

                 }
             });

           

            // Show an error if there is no GPIO controller
            if (gpio == null)
            {
                pinA = null;
                pinB = null;
                Status = "There is no GPIO controller on this device.";
                return;
            }

           

            Status = "GPIO pin initialized correctly.";

            try
            {
                Status = "About to start hub connection...";

                await hubConnection.Start();
                Status = "Connected to hub with id " + hubConnection.ConnectionId;

                Connected = true;

            }
            catch (Exception ex)
            {
                if (ex is System.Net.Http.HttpRequestException)
                {
                    Status = "Hub not online or not reachable. Check connection settings";
                }
            }

        }
    }
}
