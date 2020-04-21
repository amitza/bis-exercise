using System;
using RabbitMQ.Client;
using System.Text;
using System.Threading;

namespace client
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                for (int i = 0; i < 10; i++)
                {
                    string message = "Hello World! The time is " + DateTime.Now.ToString("h:mm:ss tt");
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "",
                                     routingKey: "hello",
                                     basicProperties: null,
                                     body: body);
                    Console.WriteLine(" [x] Sent {0}", message);
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
