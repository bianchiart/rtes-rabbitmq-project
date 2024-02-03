using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedDomain.BenchmarkUtils;
using SharedDomain.ConfigurationUtils;
using System.Text;

namespace SharedDomain.Consumers.QoS
{
    public class ConsumerQoS
    {
        private List<BenchmarkData> _packetsData;
        private List<StatisticsData> _statisticsData;
        private int _numberOfMessagesPerRun;
        private string _queueName;
        private int _numberOfRuns;
        private int _totalMessagesToReceive;
        private ushort _qosPrefetchLevel;
        private string _consumerQosLogsFileWindows;
        private string _consumerQosLogsFileUnix;

        public ConsumerQoS(Configuration configuration)
        {
            _numberOfMessagesPerRun = configuration.NumberOfMessagesPerRun;
            _queueName = configuration.QueueName;
            _numberOfRuns = configuration.NumberOfRuns;
            _totalMessagesToReceive = _numberOfMessagesPerRun * _numberOfRuns;
            _consumerQosLogsFileUnix = configuration.ConsumerQosLogsFileUnix;
            _consumerQosLogsFileWindows = configuration.ConsumerQosLogsFileWindows;

            _packetsData = new List<BenchmarkData>();
            _statisticsData = new List<StatisticsData>();
        }

        public void InitializeConsumer(IModel channel)
        {
            channel.QueueDeclare(
                queue: _queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            channel.BasicQos(0, (ushort)_qosPrefetchLevel, false);
            
            var consumer = new EventingBasicConsumer(channel);
            
            consumer.Received += Consume;

            channel.BasicConsume(
                queue: _queueName,
                autoAck: true,
                consumer: consumer);
        }

        private void Consume(object? model, BasicDeliverEventArgs eventArgs)
        {
            var benchmarkData = ObtainMessageData(eventArgs);

            _packetsData.Add(benchmarkData);

            WriteMessageOnConsole(benchmarkData);
            WriteSingleRunLog(benchmarkData);
            WriteMultipleRunsLog(benchmarkData);
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
            Console.WriteLine($"Sent" +
                $" on {benchmarkData.SentTime} received on {benchmarkData.ReceivedTime} " +
                $"msg number {benchmarkData.MessageNumber}");
        }

        private void WriteSingleRunLog(BenchmarkData benchmarkData)
        {
            if (benchmarkData.MessageNumber % _numberOfMessagesPerRun == 0)
            {
                var statistics = StatisticsCalculator.Calculate(_packetsData);
                WriteStatisticsOnFile.Write(
                    statistics,
                    _consumerQosLogsFileWindows,
                    _consumerQosLogsFileUnix);
                _statisticsData.Add(statistics);
                _packetsData.Clear();
            }
        }

        private void WriteMultipleRunsLog(BenchmarkData benchmarkData)
        {
            if (benchmarkData.MessageNumber == _totalMessagesToReceive)
            {
                var runsStatistics = StatisticsCalculator.Calculate(_statisticsData);
                WriteStatisticsOnFile.Write(
                    runsStatistics,
                    _consumerQosLogsFileWindows,
                    _consumerQosLogsFileUnix);
                _statisticsData.Clear();
            }
        }
    }
}
