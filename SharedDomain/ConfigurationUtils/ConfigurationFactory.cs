using System.Text.Json;

namespace SharedDomain.ConfigurationUtils
{
    public class Configuration
    {
        public string QueueName { get; private set; }
        public int NumberOfMessagesPerRun { get; private set; }
        public int NumberOfRuns { get; private set; }
        public ushort QosPrefetchLevel { get; private set; }
        public int CooldownSeconds { get; private set; }
        public int PublisherInterMessageDelayMilliseconds { get; private set; }
        public int NumberOfCompetitiveConsumers { get; private set; }
        public string ConsumerQosLogsFileWindows { get; private set; }
        public string CompetitiveConsumersLogsFileWindows { get; private set; }
        public string ConsumerQosLogsFileUnix { get; private set; }
        public string CompetitiveConsumersLogsFileUnix { get; private set; }
        public string RabbitMQHostName { get; private set; }

        public Configuration(
            string queueName,
            int numberOfMessagesPerRun,
            int numberOfRuns,
            ushort qosPrefetchLevel,
            int cooldownSeconds,
            int publisherInterMessageDelayMilliseconds,
            int numberOfCompetitiveConsumers,
            string consumerQosLogsFileWindows,
            string competitiveConsumersLogsFileWindows,
            string consumerQosLogsFileUnix,
            string competitiveConsumersLogsFileUnix,
            string rabbitMQHostName)
        {
            QueueName = queueName;
            NumberOfMessagesPerRun = numberOfMessagesPerRun;
            NumberOfRuns = numberOfRuns;
            QosPrefetchLevel = qosPrefetchLevel;
            CooldownSeconds = cooldownSeconds;
            PublisherInterMessageDelayMilliseconds = publisherInterMessageDelayMilliseconds;
            NumberOfCompetitiveConsumers = numberOfCompetitiveConsumers;
            ConsumerQosLogsFileWindows = consumerQosLogsFileWindows;
            CompetitiveConsumersLogsFileWindows = competitiveConsumersLogsFileWindows;
            ConsumerQosLogsFileUnix = consumerQosLogsFileUnix;
            CompetitiveConsumersLogsFileUnix = competitiveConsumersLogsFileUnix;
            RabbitMQHostName = rabbitMQHostName;
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
