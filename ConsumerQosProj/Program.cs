using RabbitMQ.Client;
using SharedDomain.ConfigurationUtils;
using SharedDomain.Consumers.QoS;

namespace ConsumerQosProj
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

            var consumerQoS = new ConsumerQoS(configuration);
            
            Console.WriteLine("Initializing consumer...");
            consumerQoS.InitializeConsumer(channel);

            Console.WriteLine($"Consumer QoS waiting for messages...");
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


