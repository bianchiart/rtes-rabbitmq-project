using RabbitMQ.Client;
using SharedDomain.ConfigurationUtils;
using SharedDomain.Consumers.Competitive;

namespace ConsumerCompetitiveProj
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = ConfigurationFactory.GetConfiguration();
            var factory = new ConnectionFactory { HostName = configuration.RabbitMQHostName };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            var consumerCompetitive = new ConsumerCompetitive(configuration);

            Console.WriteLine("Initializing consumers...");
            consumerCompetitive.InitializeConsumers(channel);

            Console.WriteLine($"Competitive consumers waiting for messages...");
            Console.WriteLine("Press key [enter] to exit.");
            Console.ReadLine();

            Console.WriteLine("Closing channel...");
            channel.Close();

            Console.WriteLine("Closing connection...");
            connection.Close();

            Console.WriteLine("Exiting...");
        } 
    } 
}


    





