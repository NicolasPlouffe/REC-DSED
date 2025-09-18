using System;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using DSED.REC.DataAccesLayer;
using DSED.REC.Entity;


namespace DSED.REC.Consumer_Journalisation
{
    class Program
    {
        private const string ExchangeName = "lead-exchange";
        private const string QueueName    = "lead-db-operations";
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
                    var json    = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var lead    = JsonSerializer.Deserialize<LeadEntity>(json);
                    var routing = ea.RoutingKey;

                    using var scope      = services.CreateScope();
                    var leadService      = scope.ServiceProvider.GetRequiredService<LeadServiceBL>();

                    if (routing == "create.lead.api" && lead != null)
                        leadService.CreateLead(lead).GetAwaiter().GetResult();
                    else if (routing == "update.lead.api" && lead != null)
                        leadService.UpdateLead(lead).GetAwaiter().GetResult();
                    else
                        throw new InvalidOperationException($"Routing key invalide : {routing}");

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
