using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using MQTTnet;
using MQTTnet.Client;
using SEALMobile.Models;

using Xamarin.Forms;

namespace SEALMobile.Views
{
    public partial class DashboardPage : ContentPage
    {

        public DashboardPage()
        {
            InitializeComponent();

            connect();
        }

        private async void connect()
        {
            var mqttFactory = new MqttFactory();
            IMqttClient mqttClient = mqttFactory.CreateMqttClient();
            var mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer("mqtt.ntscloud.cc",1883)
                .WithCredentials("cyblion", "password")
                .WithCleanSession()
                .Build();

            await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

            if (mqttClient.IsConnected)
            {
                Console.WriteLine("Connect");
                //var k = mqttClient.SubscribeAsync("@msg/computed");
                var k = await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("@msg/computed").Build());

                mqtt_label.BindingContext = k;

            }

            //mqttClient.UseApplicationMessageReceivedHandler(e =>
            //{
            //    try
            //    {
            //        string topic = e.ApplicationMessage.Topic;
            //        if (string.IsNullOrWhiteSpace(topic) == false)
            //        {
            //            string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            //            Console.WriteLine($"Topic: {topic}. Message Received: {payload}");
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message, ex);
            //    }
            //});
            //await mqttClient.StartAsync(options);


            





        }

    }
}
