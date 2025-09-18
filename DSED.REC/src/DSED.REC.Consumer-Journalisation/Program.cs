using System;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using DSED.REC.Entity;


namespace DSED.REC.Consumer_Journalisation
{
    class Program
    {
        private const string ExchangeName = "lead-exchange";
        private const string QueueName    = "lead-journalization";
        private const string LogDirectory = "LeadJournaux";

        static void Main()
        {
            Directory.CreateDirectory(LogDirectory);

            var factory    = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "admin",
                Password = "Solution1Pass123!",
                VirtualHost = "/",
                Port   = 5672,
            };
            using var connection = factory.CreateConnection();
            using var channel   = connection.CreateModel();

            channel.ExchangeDeclare(ExchangeName, ExchangeType.Topic, durable: true, autoDelete: false);
            channel.QueueDeclare(QueueName, durable: true, exclusive: false, autoDelete: false);
            channel.QueueBind(QueueName, ExchangeName, "*.lead.api");
            
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                try
                {
                    var body    = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    var parts = ea.RoutingKey.Split('.');
                    var verb  = parts[0];

                    var timestamp = DateTime.Now;
                    var fileName  = $"{timestamp:yyyy-MM-dd--HH-mm-ss}-{verb}.json";
                    var filePath  = Path.Combine(LogDirectory, fileName);

                    File.WriteAllText(filePath, message, Encoding.UTF8);

                    Console.WriteLine($"Message journalisé : {fileName}");
                    channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur : {ex.Message}");
                    channel.BasicNack(ea.DeliveryTag, false, true);
                }
            };

            channel.BasicConsume(QueueName, autoAck: false, consumer);
            Console.WriteLine("Consumer simple en écoute. [Entrée] pour quitter.");
            Console.ReadLine();
        }
    }
}
