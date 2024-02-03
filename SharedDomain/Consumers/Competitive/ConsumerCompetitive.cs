﻿using RabbitMQ.Client;
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
        private List<StatisticsData> _statisticsData;
        private int _numberOfMessagesPerRun;
        private string _queueName;
        private int _numberOfRuns;
        private int _totalMessagesToReceive;
        private int _numberOfCompetitiveConsumers;
        private string _consumerQosLogsFileWindows;
        private string _consumerQosLogsFileUnix;

        public ConsumerCompetitive(Configuration configuration)
        {
            _numberOfMessagesPerRun = configuration.NumberOfMessagesPerRun;
            _queueName = configuration.QueueName;
            _numberOfRuns = configuration.NumberOfRuns;
            _numberOfCompetitiveConsumers = configuration.NumberOfCompetitiveConsumers;
            _totalMessagesToReceive = _numberOfMessagesPerRun * _numberOfRuns;
            _consumerQosLogsFileUnix = configuration.ConsumerQosLogsFileUnix;
            _consumerQosLogsFileWindows = configuration.ConsumerQosLogsFileWindows;

            _packetsData = new List<BenchmarkData>();
            _statisticsData = new List<StatisticsData>();
        }

        public void InitializeConsumers(IModel channel)
        {
            channel.QueueDeclare(
                queue: _queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            channel.BasicQos(0, 1, false);

            InstantiateConsumers(channel);
        }

        private void InstantiateConsumers(IModel channel) 
        {
            var consumers = new List<EventingBasicConsumer>();

            for (int i = 0; i < _numberOfCompetitiveConsumers; i++)
            {
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += ConsumeMethod;
                consumers.Add(consumer);
            }

            foreach (var consumer in consumers)
            {
                channel.BasicConsume(
                    queue: _queueName,
                    autoAck: true,
                    consumer: consumer);
            }
        }

        private void ConsumeMethod(object? model, BasicDeliverEventArgs eventArgs)
        {
            var receivedTime = DateTime.UtcNow.TimeOfDay;
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body).Split(',');
            var sentTime = TimeSpan.Parse(message[2]);
            var delay = receivedTime - sentTime;
            var benchmarkData = new BenchmarkData(
                int.Parse(message[0]),
                message[1],
                sentTime,
                receivedTime,
                delay);

            _packetsData.Add(benchmarkData);
            Console.WriteLine($"Consumer {eventArgs.ConsumerTag} | Sent " +
                $"| {benchmarkData.SentTime} | Received {benchmarkData.ReceivedTime} " +
                $"| Msg number {benchmarkData.MessageNumber}");

            WriteSingleRunLog(benchmarkData);
            WriteMultipleRunsLog(benchmarkData);
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