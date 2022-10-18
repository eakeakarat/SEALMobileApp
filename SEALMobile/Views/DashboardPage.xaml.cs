using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using SEALMobile.Models;


using Xamarin.Forms;

namespace SEALMobile.Views
{
    public partial class DashboardPage : ContentPage
    {
        Project project;
        SEALENY seal;
        IMqttClient mqttClient;
        MqttClientOptions mqttClientOptions;


        public DashboardPage(Project p)
        {
            InitializeComponent();
            project = p;
            seal = new SEALENY(project);

            var mqttFactory = new MqttFactory();

            mqttClient = mqttFactory.CreateMqttClient();
            mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer("mqtt.ntscloud.cc", 31883)
                .WithCredentials("cyblion", "password")
                .WithCleanSession()
                .Build();

            mqttClient.ApplicationMessageReceivedAsync += e =>
            {
                Console.WriteLine("Received application message.");
                var msg = e.ApplicationMessage.Payload;
                var stmag = Encoding.UTF8.GetString(msg);
                Console.WriteLine(stmag);

                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var path = Path.Combine(documents, "cipher(recieve).txt");

                File.WriteAllText(path, stmag);

                publishing(seal.decryptText(stmag));
                //seal.decryptText(stmag);

                return Task.CompletedTask;
            };
            test();
            connect();
        }

        async void connect()
        {

            await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

            if (mqttClient.IsConnected)
            {
                Console.WriteLine("Connect");
                //var k = mqttClient.SubscribeAsync("@msg/computed");
                await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("@msg/computed").Build());
            }
        }

        async void publishing(string text)
        {
            //await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic("@msg/decrypted")
                .WithPayload(text)
                .Build();
            await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);

            //await mqttClient.DisconnectAsync();
        }

        private void test()
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documents, "cipher.txt");

            File.WriteAllText(path,seal.encryptText());


        }
        

    }
}
