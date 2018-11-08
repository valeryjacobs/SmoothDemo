using MQTTnet;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Init();

            Console.ReadLine();
        }

        static private async Task Init()
        {
            var factory = new MqttFactory();
            var mqttClient = factory.CreateMqttClient();

            try
            {
                var options = new MqttClientOptionsBuilder()
                    .WithClientId("Client1")
                    .WithTcpServer("m23.cloudmqtt.com", 11672)
                    .WithCredentials("abmszykl", "cNJDTuaDIfKC")
                    .WithTls()
                    .WithCleanSession()
                    .Build();

                await mqttClient.ConnectAsync(options);
            }
            catch(Exception ex)
            {

            }


            mqttClient.Connected += async (s, e) =>
            {
                Console.WriteLine("### CONNECTED WITH SERVER ###");

                // Subscribe to a topic
                await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("my/topic").Build());

                Console.WriteLine("### SUBSCRIBED ###");
            };


        }
    }
}
