using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using SharedDomain.BenchmarkUtils;
using SharedDomain.BenchmarkUtils.Models;
using SharedDomain.ConfigurationUtils;
using System.Text;

namespace SharedDomain.Consumers.TTL
{
    public class ConsumerTTL
    {
        private List<BenchmarkData> _packetsData;
        private List<StatisticsData> _statisticsData;
        private int _numberOfMessagesPerRun;
        private string _queueName;
        private int _numberOfRuns;
        private ushort _qosPrefetchLevel;
        private int _totalMessagesToReceive;
        private string _consumerTTLLogsFileWindows;
        private string _consumerTTLLogsFileUnix;
        private int _messagesTimeToLive;
        private int _consumerDelay;

        public ConsumerTTL(Configuration configuration)
        {
            _numberOfMessagesPerRun = configuration.NumberOfMessagesPerRun;
            _queueName = configuration.QueueName;
            _numberOfRuns = configuration.NumberOfRuns;
            _totalMessagesToReceive = _numberOfMessagesPerRun * _numberOfRuns;
            _consumerTTLLogsFileWindows = configuration.ConsumerTTLLogsFileWindows;
            _consumerTTLLogsFileUnix = configuration.ConsumerTTLLogsFileUnix;
            _qosPrefetchLevel = configuration.QosPrefetchLevel;
            _messagesTimeToLive = configuration.TimeToLiveMilliseconds;
            _consumerDelay = configuration.ConsumerDelayMilliseconds;

            _packetsData = new List<BenchmarkData>();
            _statisticsData = new List<StatisticsData>();
        }

        public void InitializeConsumer(IModel channel)
        {
            var args = new Dictionary<string, object>();
            args.Add("x-message-ttl", _messagesTimeToLive);

            channel.QueueDeclare(
                queue: _queueName,
                durable: false,
                exclusive: false,
                autoDelete: true,
                arguments: args);

            channel.BasicQos(0, _qosPrefetchLevel, true);
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
            
            ExecuteDelay();
            WriteMessageOnConsole(benchmarkData);
            WriteSingleRunLog(benchmarkData);
            WriteMultipleRunsLog(benchmarkData);
        }

        private void ExecuteDelay()
        {
            if(_consumerDelay > 0)
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
            Console.WriteLine($"Sent : {benchmarkData.SentTime} | " +
                $"Received : {benchmarkData.ReceivedTime} " +
                $"| Msg number  : {benchmarkData.MessageNumber}");
        }

        private void WriteSingleRunLog(BenchmarkData benchmarkData)
        {
            if (benchmarkData.MessageNumber % _numberOfMessagesPerRun == 0)
            {
                var statistics = StatisticsCalculator.Calculate(
                    _packetsData, (int)(benchmarkData.MessageNumber / _numberOfMessagesPerRun));
                WriteStatisticsOnFile.Write(
                    statistics,
                    _consumerTTLLogsFileWindows,
                    _consumerTTLLogsFileUnix);
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
                    _consumerTTLLogsFileWindows,
                    _consumerTTLLogsFileUnix);
                _statisticsData.Clear();
            }
        }
    }
}
