using Microsoft.AspNet.SignalR;
using SmoothDemo.Agent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SmoothDemo.Agent.Hubs
{
    public class ControlHub : Hub
    {
        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(name, message);
        }

        public void Ping(string id)
        {
            Clients.All.ping(id);
        }

        public void HandleCommand(Command command)
        {
            var dispatcher = Application.Current.Dispatcher;
            dispatcher.Invoke(() => App.MainViewModel.HandleCommand(command));
        }

        public void RequestActionList()
        {
            Clients.All.updateActionList(App.MainViewModel.Actions);
        }

        public void SetActionIndex(int actionIndex)
        {
            Clients.All.setActionIndex(actionIndex);
        }

        public override Task OnConnected()
        {

            App.MainViewModel.Hub = this;

            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }
    }
}
