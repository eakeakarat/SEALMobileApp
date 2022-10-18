using System;
using System.Text;
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
            mqttClient.ApplicationMessageReceivedAsync += e =>
            {
                Console.WriteLine("Received application message.");
                var msg = e.ApplicationMessage.Payload;
                var stmag = Encoding.UTF8.GetString(msg);
                
                Console.WriteLine(stmag);



                //return decode(stmag);

                return Task.CompletedTask;
            };

        }
        public bool isConnect()
        {
            return mqttClient.IsConnected;
        }
        public async Task<bool> Connected()
        {
            await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
            return mqttClient.IsConnected;
        }

        public async void Subscribe()
        {

            await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("@msg/computed").Build());
        }

        private async void decode(string k)
        {
            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic("samples/temperature/living_room")
                .WithPayload("19.5")
                .Build();

            await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);

        }


    }
}
