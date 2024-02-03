using System.Text;
using RabbitMQ.Client;
using SharedDomain;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
var configuration = ConfigurationFactory.GetConfiguration();

channel.QueueDeclare(queue: configuration.QueueName,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

var totalMessagesToSend = configuration.NumberOfMessagesPerRun * configuration.NumberOfRuns;
for (int i = 1; i <= totalMessagesToSend; i++)
{
    string message = $"{i},msg,{DateTime.Now.TimeOfDay}";
    var body = Encoding.UTF8.GetBytes(message);
    channel.BasicPublish(exchange: string.Empty,
                     routingKey: configuration.QueueName,
                     basicProperties: null,
                     body: body);
    Console.WriteLine($"Sent {message}");

    if (i % configuration.NumberOfMessagesPerRun == 0)
    {
        if(configuration.CooldownSeconds > 0)
        {
            Thread.Sleep(configuration.CooldownSeconds * 1000);
        }
    }
}

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();

