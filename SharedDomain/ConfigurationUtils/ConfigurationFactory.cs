﻿using System.Text.Json;

namespace SharedDomain.ConfigurationUtils
{
    public class Configuration
    {
        public string QueueName { get; private set; }
        public int NumberOfMessagesPerRun { get; private set; }
        public int NumberOfRuns { get; private set; }
        public ushort QosPrefetchLevel { get; private set; }
        public ushort QoSPrefetchLevelMultiple { get; private set; }
        public int TimeToLiveMilliseconds { get; private set; }
        public int CooldownSeconds { get; private set; }
        public int PublisherInterMessageDelayMilliseconds { get; private set; }
        public int ConsumerDelayMilliseconds { get; private set; }
        public int NumberOfCompetitiveConsumers { get; private set; }
        public string ConsumerQosLogsFileWindows { get; private set; }
        public string CompetitiveConsumersLogsFileWindows { get; private set; }
        public string ConsumerQosLogsFileUnix { get; private set; }
        public string CompetitiveConsumersLogsFileUnix { get; private set; }
        public string ConsumerTTLLogsFileWindows { get; private set; }
        public string ConsumerTTLLogsFileUnix { get; private set; }
        public string RabbitMQHostName { get; private set; }

        public Configuration(
            string queueName,
            int numberOfMessagesPerRun,
            int numberOfRuns,
            ushort qosPrefetchLevel,
            ushort qosPrefetchLevelMultiple,
            int cooldownSeconds,
            int publisherInterMessageDelayMilliseconds,
            int consumerDelayMilliseconds,
            int numberOfCompetitiveConsumers,
            int timeToLiveMilliseconds,
            string consumerQosLogsFileWindows,
            string competitiveConsumersLogsFileWindows,
            string consumerQosLogsFileUnix,
            string competitiveConsumersLogsFileUnix,
            string consumerTTLLogsFileWindows,
            string consumerTTLLogsFileUnix,
            string rabbitMQHostName)
        {
            QueueName = queueName;
            NumberOfMessagesPerRun = numberOfMessagesPerRun;
            NumberOfRuns = numberOfRuns;
            QosPrefetchLevel = qosPrefetchLevel;
            QoSPrefetchLevelMultiple = qosPrefetchLevelMultiple;
            CooldownSeconds = cooldownSeconds;
            PublisherInterMessageDelayMilliseconds = publisherInterMessageDelayMilliseconds;
            NumberOfCompetitiveConsumers = numberOfCompetitiveConsumers;
            ConsumerDelayMilliseconds = consumerDelayMilliseconds;
            ConsumerQosLogsFileWindows = consumerQosLogsFileWindows;
            CompetitiveConsumersLogsFileWindows = competitiveConsumersLogsFileWindows;
            ConsumerQosLogsFileUnix = consumerQosLogsFileUnix;
            CompetitiveConsumersLogsFileUnix = competitiveConsumersLogsFileUnix;
            RabbitMQHostName = rabbitMQHostName;
            TimeToLiveMilliseconds = timeToLiveMilliseconds;
            ConsumerTTLLogsFileWindows = consumerTTLLogsFileWindows;
            ConsumerTTLLogsFileUnix = consumerTTLLogsFileUnix;
        }

        public void PrintConfigurationSettings()
        {
            Console.WriteLine($"Settings: {Environment.NewLine}" +
                $"QueueName : {QueueName} {Environment.NewLine}" +
                $"Number of messages per run : {NumberOfMessagesPerRun}{Environment.NewLine}" +
                $"Number of runs : {NumberOfRuns} {Environment.NewLine}" +
                $"QoS prefetch count for single consumer case: {QosPrefetchLevel} {Environment.NewLine}" +
                $"QoS prefetch count for multiple consumer case: {QoSPrefetchLevelMultiple} {Environment.NewLine}" +
                $"Inter run cooldown seconds : {CooldownSeconds} {Environment.NewLine}" +
                $"Publisher intermessage delay ms : {PublisherInterMessageDelayMilliseconds} {Environment.NewLine}" +
                $"Consumer delay ms : {ConsumerDelayMilliseconds} {Environment.NewLine}" +
                $"Number of competitive consumers: {NumberOfCompetitiveConsumers} {Environment.NewLine}" +
                $"Messages time to live milliseconds: {TimeToLiveMilliseconds} {Environment.NewLine}" +
                $"Rabbit host : {RabbitMQHostName}");

        }
    }

    public static class ConfigurationFactory
    {
        public static Configuration GetConfiguration()
        {
            return JsonSerializer.Deserialize<Configuration>(
                File.ReadAllText("GlobalSettings.json"));
        }
    }
}
