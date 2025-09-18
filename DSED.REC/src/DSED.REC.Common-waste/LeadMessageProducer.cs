using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;
using DSED.REC.Entity;

namespace DSED.REC.Common
{
    public class LeadMessageProducer
    {
        public string ExchangeName    { get; } = "lead-exchange";
        public string ApplicationName { get; } = "DSED.REC.LeadCRM";

        private readonly ConnectionFactory _factory;

        public LeadMessageProducer(string exchangeName, string applicationName)
        {
            ExchangeName    = exchangeName    ?? throw new ArgumentNullException(nameof(exchangeName));
            ApplicationName = applicationName ?? throw new ArgumentNullException(nameof(applicationName));
            _factory = new ConnectionFactory {
                HostName     = "localhost",
                Port         = 5672,
                UserName     = "admin",                    
                Password     = "Solution1Pass123!",
                VirtualHost  = "/"
            };
        }

        public void PublishCreate(LeadEntity lead)
            => Publish(lead, "create.lead.api");

        public void PublishUpdate(LeadEntity lead)
            => Publish(lead, "update.lead.api");

        private void Publish(LeadEntity lead, string routingKey)
        {
            var jsonBody = JsonSerializer.Serialize(lead);
            var body     = Encoding.UTF8.GetBytes(jsonBody);

            using var connection = _factory.CreateConnection();
            using var channel    = connection.CreateModel();

            channel.ExchangeDeclare(
                exchange: ExchangeName,
                type:     ExchangeType.Topic,
                durable:  true,
                autoDelete: false);

            channel.BasicPublish(
                exchange:       ExchangeName,
                routingKey:     routingKey,
                basicProperties: null,
                body:            body);
        }
    }
}