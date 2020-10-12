using System;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using Shared;

namespace RabbitMQProducer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                HostName = RabbitMQSettings.HOSTNAME,
                UserName = RabbitMQSettings.USERNAME,
                Password = RabbitMQSettings.PASSWORD,
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var prop = channel.CreateBasicProperties();
                    prop.Persistent = true;

                    channel.QueueDeclare(
                        RabbitMQSettings.QUEUE,
                        true,
                        false,
                        false,
                        null);

                    var message = "Hello world";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish("",
                        RabbitMQSettings.QUEUE,
                        prop,
                        body);

                    Console.WriteLine("Send {0} on queue {1}", message, RabbitMQSettings.QUEUE);
                }
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}