using System;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using DSED.REC.Application;
using DSED.REC.DataAccesLayer;
using DSED.REC.Entity;
using DSED.REC.Entity.IDepot;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DSED.REC.Consumer_Db
{
    class Program
    {
        private const string ExchangeName = "lead-exchange";
        private const string QueueName    = "lead-db-operations";

        static void Main()
        {
            var services = new ServiceCollection()
                .AddDbContext<ApplicationDbContext>(opt =>
                    opt.UseSqlServer("Server=localhost,1433;Database=LeadsCRM;User Id=sa;Password=Solution1Pass123!;TrustServerCertificate=True"))
                .AddScoped<IValidator<LeadEntity>, LeadValidator>()
                .AddScoped<ILeadDepot, LeadDepotDepot>()
                .AddScoped<LeadServiceBL>()
                .BuildServiceProvider();

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
            channel.QueueBind(QueueName, ExchangeName, "create.lead.api");
            channel.QueueBind(QueueName, ExchangeName, "update.lead.api");

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
