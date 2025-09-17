using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DSED.REC.Common;

public class LeadMessageProducer
{
    public string ExchangeName { get; } = "Lead";
    public string ApplicationName { get;} = "Lead";

    private readonly ConnectionFactory _factory;
    
    public LeadMessageProducer(string exchangeName, string applicationName)
    {
        this.ExchangeName = exchangeName;
        this.ApplicationName = applicationName;
        _factory = new ConnectionFactory() { HostName = "localhost" };
    }

    public void Execute()
    {
        
    }
    
}