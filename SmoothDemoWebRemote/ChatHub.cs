using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SmoothDemoWebRemote.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task RequestActionList()
        {
            await Clients.All.SendAsync("RequestActionList");
        }

        public async Task ReceiveActionList(string actionList)
        {
            await Clients.All.SendAsync("ReceiveActionList",actionList);
        }
    }
}