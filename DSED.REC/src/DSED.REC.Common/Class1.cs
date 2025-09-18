using RabbitMQ.Client;
using RabbitMQ.Client.Core;

namespace DSED.REC.Common
{
    public class Class1
    {
        public void Foo()
        {
            var factory = new ConnectionFactory();
            using var conn = factory.CreateConnection();
            using var channel = conn.CreateModel();
        }
    }
}