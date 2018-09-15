using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace ServiceBusProxy.Consumer
{
    public class Program
    {
        private static readonly string ConnectionString =
            Environment.GetEnvironmentVariable("SBConnectionString");

        private static readonly string Topic =
            Environment.GetEnvironmentVariable("SBTopic");

        private static readonly string Subscription =
            Environment.GetEnvironmentVariable("SBSubscription");

        private static readonly string FunctionUrl =
            Environment.GetEnvironmentVariable("FunctionUrl");

        private static readonly int MaxConcurrentSessions =
            int.Parse(Environment.GetEnvironmentVariable("MaxConcurrentSessions"));

        private static readonly int MessageWaitTimeout =
            int.Parse(Environment.GetEnvironmentVariable("MessageWaitTimeout"));

        private static readonly HttpClient HttpClient = new HttpClient();

        public static void Main(string[] args)
        {
            Console.WriteLine("Consumer started");

            var client = new SubscriptionClient(ConnectionString, Topic, Subscription);

            Console.WriteLine("Registering handler");

            client.RegisterSessionHandler(async (session, message, cancellationToken) =>
            {
                // Using peek lock, we need to wait for a response from the function app
                // so we can complete / dead letter the message correctly
                var success = await PostToAzureFunction(message);

                if (success)
                {
                    await session.CompleteAsync(message.SystemProperties.LockToken);
                }
                else
                {
                    await session.DeadLetterAsync(message.SystemProperties.LockToken);
                }
            }, new SessionHandlerOptions(LogMessageHandlerException)
            {
                // False as manually completing on function response
                AutoComplete = false,
                MaxConcurrentSessions = MaxConcurrentSessions,
                // Wait X seconds for new messages before releasing the session handler so another can take it's place
                MessageWaitTimeout = TimeSpan.FromSeconds(MessageWaitTimeout)
            });

            Console.WriteLine("Accepting messages");

            Thread.Sleep(-1);
        }

        private static async Task<bool> PostToAzureFunction(Message message)
        {
            try
            {
                var result = await HttpClient.PostAsync(FunctionUrl,
                    new ReadOnlyMemoryContent(
                        new ReadOnlyMemory<byte>(
                            Encoding.UTF8.GetBytes(
                                JsonConvert.SerializeObject(message, Formatting.Indented)))));

                return result.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static Task LogMessageHandlerException(ExceptionReceivedEventArgs e)
        {
            Console.WriteLine("Exception: \"{0}\" {1}", e.Exception.Message, e.ExceptionReceivedContext.EntityPath);

            return Task.CompletedTask;
        }
    }
}