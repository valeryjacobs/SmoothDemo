using MQTTnet;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SmoothDemo.Remote
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MqttFactory factory;
        IMqttClient mqttClient;

        public MainPage()
        {
            this.InitializeComponent();


            InitAsync();

        }

        private async Task InitAsync()
        {
            factory = new MqttFactory();
            mqttClient = factory.CreateMqttClient();



            var options = new MqttClientOptionsBuilder()
                .WithClientId("Remote")
                .WithTcpServer("m23.cloudmqtt.com" , 31672)
                .WithCredentials("abmszykl", "cNJDTuaDIfKC")
                .WithCleanSession()
                .Build();

            try
            {
                await mqttClient.ConnectAsync(options);

              
            }
            catch (Exception ex)
            {


            }

            mqttClient.Connected += async (s, e) =>
            {
               

               
                Debug.WriteLine("### CONNECTED WITH SERVER ###");

                // Subscribe to a topic
               // await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("my/topic").Build());

                Debug.WriteLine("### SUBSCRIBED ###");

                var message = new MqttApplicationMessageBuilder()
                    .WithTopic("/test")
                    .Build();

                await mqttClient.PublishAsync(message);
            };

            mqttClient.ApplicationMessageReceived += (s, e) =>
            {
                Debug.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
                Debug.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
                Debug.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                Debug.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                Debug.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
                Debug.WriteLine("");
            };

        }


    }
}
