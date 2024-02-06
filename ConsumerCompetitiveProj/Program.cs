using RabbitMQ.Client;
using SharedDomain.ConfigurationUtils;
using SharedDomain.Consumers.Competitive;

namespace ConsumerCompetitiveProj
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

            var consumerCompetitive = new ConsumerCompetitive(configuration);

            Console.WriteLine("Initializing consumers...");
            consumerCompetitive.InitializeConsumer(channel);

            Console.WriteLine($"Competitive consumers waiting for messages...");

            do
            {
                Console.WriteLine("Enter Y if you want to wait another run, else N, statistics will be calculated either way");
                var line = Console.ReadLine();
                
                consumerCompetitive.WriteRunLog();
                
                if (line.Equals("Y") || line.Equals("y"))
                {
                    continue;
                }
                else
                {
                    break;
                }
            }
            while (true);

            Console.WriteLine("Closing channel...");
            channel.Close();

            Console.WriteLine("Closing connection...");
            connection.Close();

            Console.WriteLine("Exiting...");
        }
    }
}








