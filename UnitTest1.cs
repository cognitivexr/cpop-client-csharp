using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using NUnit.Framework;
using NUnit.Framework.Internal.Execution;

namespace cpop_client
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        private static byte[] GenerateCpopData()
        {
            var cpopData = new CpopData
            {
                Timestamp = 1,
                Position =
                {
                    X = 1,
                    Y = 3,
                    Z = 7
                },
                Type = "test"
            };
            MemoryStream ms = new MemoryStream();
            using (BsonWriter writer = new BsonWriter(ms))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, cpopData);
            }

            return ms.ToArray();
        }

        private static IMqttClient GenerateTestClient()
        {
            var factory = new MqttFactory();
            var mqttClient = factory.CreateMqttClient();
            return mqttClient;
        }

        [Test]
        public async Task SubscriberTest()
        {
            var cpop = new CpopSubscriber();
            cpop.Subscribe();

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("cpop")
                .WithPayload(GenerateCpopData())
                .WithRetainFlag()
                .Build();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("Test-Client")
                .WithTcpServer("localhost")
                .WithCleanSession()
                .Build();
            var client = GenerateTestClient();
            await client.ConnectAsync(options, CancellationToken.None);
            await client.PublishAsync(message, CancellationToken.None);

            Thread.Sleep(10000);
            Console.WriteLine(cpop.Queue.ToArray()[0]);
            Assert.Pass();
        }

        [Test]
        public void BsonTest()
        {
            var data = new CpopData
            {
                Timestamp = 123123,
            };

            var ms = new MemoryStream();
            using (BsonWriter writer = new BsonWriter(ms))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, data);
            }

            var base64String = Convert.ToBase64String(ms.ToArray());

            Console.WriteLine(base64String);
        }
    }
}