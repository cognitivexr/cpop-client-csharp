using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;

namespace cpop_client
{

    public class CpopSubscriber
    {
        public async void Subscribe()
        {
            var factory = new MqttFactory();
            var mqttClient = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("Client1")
                .WithTcpServer("localhost")
                .WithCleanSession()
                .Build();

            mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
                Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
                Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
                Console.WriteLine();

                var message = new MqttApplicationMessageBuilder()
                    .WithTopic("cpop")
                    .WithPayload(Encoding.UTF8.GetString(e.ApplicationMessage.Payload))
                    .Build();
                Task.Run(() => mqttClient.PublishAsync(message));
            });
            
            await mqttClient.ConnectAsync(options, CancellationToken.None);
            await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("cpop").Build());

        }

    }
}
