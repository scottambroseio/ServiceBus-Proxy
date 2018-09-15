using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace ServiceBusProxy.Producer
{
    public class Program
    {
        private static readonly string ConnectionString =
            Environment.GetEnvironmentVariable("SBConnectionString");

        private static readonly string Topic =
            Environment.GetEnvironmentVariable("SBTopic");

        public static async Task Main(string[] args)
        {
            Console.WriteLine("Producer started");

            var topicClient = new TopicClient(ConnectionString, Topic);

            Console.WriteLine("Sending messages");

            foreach (var message in ProduceMessages())
            {
                await topicClient.SendAsync(message);
            }

            Console.WriteLine("Closing client");

            await topicClient.CloseAsync();

            Console.WriteLine("Exiting");
        }

        private static IEnumerable<Message> ProduceMessages()
        {
            yield return new Message(Encoding.UTF8.GetBytes("Message 1"))
            {
                SessionId = "1",
                ContentType = "plain/text",
                MessageId = Guid.NewGuid().ToString(),
                TimeToLive = TimeSpan.FromDays(14)
            };

            yield return new Message(Encoding.UTF8.GetBytes("Message 2"))
            {
                SessionId = "1",
                ContentType = "plain/text",
                MessageId = Guid.NewGuid().ToString(),
                TimeToLive = TimeSpan.FromDays(14)
            };

            yield return new Message(Encoding.UTF8.GetBytes("Message 3"))
            {
                SessionId = "1",
                ContentType = "plain/text",
                MessageId = Guid.NewGuid().ToString(),
                TimeToLive = TimeSpan.FromDays(14)
            };

            yield return new Message(Encoding.UTF8.GetBytes("Message 1"))
            {
                SessionId = "2",
                ContentType = "plain/text",
                MessageId = Guid.NewGuid().ToString(),
                TimeToLive = TimeSpan.FromDays(14)
            };

            yield return new Message(Encoding.UTF8.GetBytes("Message 2"))
            {
                SessionId = "2",
                ContentType = "plain/text",
                MessageId = Guid.NewGuid().ToString(),
                TimeToLive = TimeSpan.FromDays(14)
            };

            yield return new Message(Encoding.UTF8.GetBytes("Message 3"))
            {
                SessionId = "2",
                ContentType = "plain/text",
                MessageId = Guid.NewGuid().ToString(),
                TimeToLive = TimeSpan.FromDays(14)
            };

            yield return new Message(Encoding.UTF8.GetBytes("Message 1"))
            {
                SessionId = "3",
                ContentType = "plain/text",
                MessageId = Guid.NewGuid().ToString(),
                TimeToLive = TimeSpan.FromDays(14)
            };

            yield return new Message(Encoding.UTF8.GetBytes("Message 2"))
            {
                SessionId = "3",
                ContentType = "plain/text",
                MessageId = Guid.NewGuid().ToString(),
                TimeToLive = TimeSpan.FromDays(14)
            };

            yield return new Message(Encoding.UTF8.GetBytes("Message 3"))
            {
                SessionId = "3",
                ContentType = "plain/text",
                MessageId = Guid.NewGuid().ToString(),
                TimeToLive = TimeSpan.FromDays(14)
            };
        }
    }
}