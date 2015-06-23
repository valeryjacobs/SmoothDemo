using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TestClient
{
    class Program
    {
        public static IHubProxy HubProxy { get; set; }
        const string ServerURI = "http://localhost:8080/signalr";
        public static HubConnection Connection { get; set; }

        static void Main(string[] args)
        {
            var hubConnection = new HubConnection("http://localhost:8080");
            var chat = hubConnection.CreateHubProxy("ControlHub");
            chat.On<string, string>("Send", (name, message) =>
            {
                Console.Write(name + ": ");
                Console.WriteLine(message);
            });

            hubConnection.Start().Wait();

            chat.Invoke("Notify", "Console app", hubConnection.ConnectionId);
            string msg = null;

            while ((msg = Console.ReadLine()) != null)
            {
                chat.Invoke("Send", "Console app", msg).Wait();
            }
        }


    }
}
