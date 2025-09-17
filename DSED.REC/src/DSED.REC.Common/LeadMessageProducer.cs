using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using DSED.REC.Entity;

namespace DSED.REC.Common;

public class LeadMessageProducer
{
    public string ExchangeName { get; } = "Lead";
    public string ApplicationName { get;} = "DSED.REC.LeadCRM";

    private readonly ConnectionFactory _factory;
    
    public LeadMessageProducer(string exchangeName, string applicationName)
    {
        this.ExchangeName = exchangeName ?? throw new ArgumentNullException(nameof(exchangeName));
        this.ApplicationName = applicationName ?? throw new ArgumentNullException(nameof(applicationName));
        _factory = new ConnectionFactory() { HostName = "localhost" };
    }

    public void PublishCreate(LeadEntity lead)
    {
        Publish(lead,"PostLead","create.lead.api");
    }

    public void PublishUpdate(LeadEntity lead)
    {
        Publish(lead,"PutLead","update.lead.api");
    }
    
    public void Publish(LeadEntity lead, string httpVerb, string routingKey)
    {
        string json = JsonSerializer.Serialize(lead);
        byte[] dataBytes = Encoding.UTF8.GetBytes(json);
        var enveloppe = new MessageEnveloppe
        {
            HttpVerb = httpVerb,
            DatasEntityEncoded = dataBytes, 
            TimeCreation = DateTime.UtcNow,
            EnityType = nameof(LeadEntity),
            Application = ApplicationName
        };
        
        string jsonEnveloppe = JsonSerializer.Serialize(enveloppe);
        var body = Encoding.UTF8.GetBytes(jsonEnveloppe);

        using (IConnection connection = _factory.CreateConnection())
        using (IModel channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(
                exchange: ExchangeName,
                type: "topic", 
                durable: true,
                autoDelete: false);
            
            channel.BasicPublish(
                exchange: ExchangeName ,
                routingKey: routingKey,
                basicProperties: null,
                body: body);
        }
    }
}