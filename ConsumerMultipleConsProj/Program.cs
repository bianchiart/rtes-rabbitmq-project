using RabbitMQ.Client;
using SharedDomain.ConfigurationUtils;
using SharedDomain.Consumers.Exchange;

namespace ConsumerTTLProj
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

            var consumer = new ConsumerForExchange(configuration);
            
            consumer.InitializeConsumer(channel);

            Console.WriteLine($"Consumers in exchange waiting for messages...");
            
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
