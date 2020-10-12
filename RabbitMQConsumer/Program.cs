using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared;

namespace RabbitMQConsumer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                HostName = RabbitMQSettings.HOSTNAME,
                UserName = RabbitMQSettings.USERNAME,
                Password = RabbitMQSettings.PASSWORD
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        RabbitMQSettings.QUEUE,
                        true,
                        false,
                        false,
                        null);

                    // Not to give more then one message to worker at time
                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" [x] Received {0}", message);

                        // If error message is returned to the queue 
                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    };

                    channel.BasicConsume(
                        Shared.RabbitMQSettings.QUEUE,
                        autoAck: true,
                        consumer);

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
        }
    }
}