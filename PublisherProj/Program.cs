using RabbitMQ.Client;
using SharedDomain.ConfigurationUtils;
using SharedDomain.Publisher;

namespace PublisherProj
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            var configuration = ConfigurationFactory.GetConfiguration();
            var factory = new ConnectionFactory { HostName = configuration.RabbitMQHostName };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            
            configuration.PrintConfigurationSettings();

            var publisher = new Publisher(configuration);
            publisher.Execute(channel);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}


