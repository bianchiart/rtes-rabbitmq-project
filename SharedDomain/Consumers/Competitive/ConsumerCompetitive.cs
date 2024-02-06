using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedDomain.BenchmarkUtils;
using SharedDomain.BenchmarkUtils.Models;
using SharedDomain.ConfigurationUtils;
using System.Text;

namespace SharedDomain.Consumers.Competitive
{
    public class ConsumerCompetitive
    {
        private List<BenchmarkData> _packetsData;
        private string _queueName;
        private ushort _qosPrefetchLevelMultiple;
        private int _consumerDelay;
        private string _consumerCompetitiveLogsFileWindows;
        private string _consumerCompetitiveLogsFileUnix;
        private int _consumerIndex;

        public ConsumerCompetitive(Configuration configuration)
        {
            _queueName = configuration.QueueName;
            _consumerCompetitiveLogsFileUnix = configuration.CompetitiveConsumersLogsFileUnix;
            _consumerCompetitiveLogsFileWindows = configuration.CompetitiveConsumersLogsFileWindows;
            _qosPrefetchLevelMultiple = configuration.QoSPrefetchLevelMultiple;
            _consumerDelay = configuration.ConsumerDelayMilliseconds;

            _packetsData = new List<BenchmarkData>();
        }

        public void InitializeConsumer(IModel channel)
        {
            _consumerIndex = new Random().Next(100000);
            channel.QueueDeclare(
                queue: _queueName,
                durable: false,
                exclusive: false,
                autoDelete: true,
                arguments: null);

            channel.BasicQos(0, _qosPrefetchLevelMultiple, true);

            InstantiateConsumer(channel);
            Console.WriteLine($"Number of consumers on queue {_queueName} : {channel.ConsumerCount(_queueName)}");
        }

        private void InstantiateConsumer(IModel channel)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += ConsumeMethod;

            channel.BasicConsume(
                queue: _queueName,
                autoAck: true,
                consumer: consumer);
        }

        private void ConsumeMethod(object? model, BasicDeliverEventArgs eventArgs)
        {
            var benchmarkData = ObtainMessageData(eventArgs);

            _packetsData.Add(benchmarkData);

            ExecuteDelay();
            WriteMessageOnConsole(benchmarkData);
        }

        private void ExecuteDelay()
        {
            if (_consumerDelay > 0)
            {
                Thread.Sleep(_consumerDelay);
            }
        }

        private BenchmarkData ObtainMessageData(BasicDeliverEventArgs eventArgs)
        {
            var receivedTime = DateTime.UtcNow.TimeOfDay;
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body).Split(',');
            var sentTime = TimeSpan.Parse(message[2]);
            var delay = receivedTime - sentTime;

            return new BenchmarkData(
                int.Parse(message[0]),
                message[1],
                sentTime,
                receivedTime,
                delay);
        }

        private void WriteMessageOnConsole(BenchmarkData benchmarkData)
        {
            Console.WriteLine($"Consumer : {_consumerIndex} | Sent : {benchmarkData.SentTime} | " +
                $"Received : {benchmarkData.ReceivedTime} " +
                $"| Msg number  : {benchmarkData.MessageNumber}");
        }

        public void WriteRunLog()
        {
            var statistics = StatisticsCalculator.Calculate(
                _packetsData, 1);
            WriteStatisticsOnFile.Write(
                    statistics,
                    _consumerCompetitiveLogsFileWindows + _consumerIndex.ToString(),
                    _consumerCompetitiveLogsFileUnix + _consumerIndex.ToString());
            _packetsData.Clear();
        }
    }
}
