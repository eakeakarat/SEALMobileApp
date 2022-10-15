using System;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Packets;

namespace SEALMobile.Models
{
    public class MqttManagement
    {
        MqttFactory mqttFactory = new MqttFactory();
        IMqttClient mqttClient;
        MqttClientOptions mqttClientOptions;
        public MqttManagement()
        {

            mqttClient = mqttFactory.CreateMqttClient();
            mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer("mqtt.ntscloud.cc")
                .WithCredentials("cyblion", "password")
                .WithCleanSession()
                .Build();
            


        }
        public bool isConnect()
        {
            return mqttClient.IsConnected;
        }
        public async Task<bool> Connected()
        {
            await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);


            Console.WriteLine(mqttClient.IsConnected.ToString());

            return mqttClient.IsConnected;

        }

        public async void Subscribe()
        {

            var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                .WithTopicFilter(f => { f.WithTopic("@msg/computed"); })
                .Build();

            var response = await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

            var k = await mqttClient.SubscribeAsync("@msg/computed");

            while (response.ReasonString == "")
            {
                response = await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

            }

            Console.WriteLine("MQTT client subscribed to topic.");
            Console.WriteLine(response.ToString());
            Console.WriteLine(k.ReasonString);

        }


    }
}
