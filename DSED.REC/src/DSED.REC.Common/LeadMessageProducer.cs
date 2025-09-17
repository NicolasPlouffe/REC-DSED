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
    public string ApplicationName { get;} = "Lead";

    private readonly ConnectionFactory _factory;
    
    public LeadMessageProducer(string exchangeName, string applicationName)
    {
        this.ExchangeName = exchangeName ?? throw new ArgumentNullException(nameof(exchangeName));
        this.ApplicationName = applicationName ?? throw new ArgumentNullException(nameof(applicationName));
        _factory = new ConnectionFactory() { HostName = "localhost" };
    }

    public void Publish(LeadEntity lead, string httpVerb)
    {
        string json = JsonSerializer.Serialize(lead);
        
        var enveloppe = new MessageEnveloppe
        {
            HttpVerb = httpVerb,
            DatasEntityEncoded = Encoding.UTF8.GetBytes(json), 
            TimeCreation = DateTime.UtcNow,
            EnityType = nameof(LeadEntity),
            Application = "DSED.REC.LeadCRM"
        };
        
        string jsonEnveloppe = JsonSerializer.Serialize(enveloppe);
        var body = Encoding.UTF8.GetBytes(jsonEnveloppe);
        
    }
    
}