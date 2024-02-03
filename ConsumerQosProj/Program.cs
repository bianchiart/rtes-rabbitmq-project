using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedDomain;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
var configuration = ConfigurationFactory.GetConfiguration();

channel.QueueDeclare(queue: configuration.QueueName,
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

channel.BasicQos(0, (ushort)configuration.QosPrefetchLevel, false);

Console.WriteLine(" [*] Waiting for messages.");

var packetsData = new List<BenchmarkData>();
var statisticsData = new List<StatisticsData>();
var consumer = new EventingBasicConsumer(channel);
consumer.Received += ConsumeMethod;

channel.BasicConsume(queue: configuration.QueueName,
                     autoAck: true,
                     consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();

void ConsumeMethod(object? model, BasicDeliverEventArgs ea)
{
    var receivedTime = DateTime.Now.TimeOfDay;
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body).Split(',');
    var sentTime = TimeSpan.Parse(message[2]);
    var delay = receivedTime - sentTime;
    var benchmarkData = new BenchmarkData(
        int.Parse(message[0]),
        message[1],
        sentTime,
        receivedTime,
        delay);

    packetsData.Add(benchmarkData);
    Console.WriteLine($"Sent" +
        $" on {benchmarkData.SentTime} received on {benchmarkData.ReceivedTime} " +
        $"msg number {benchmarkData.MessageNumber}, msg: {benchmarkData.Message}");

    if (benchmarkData.MessageNumber % configuration.NumberOfMessagesPerRun == 0)
    {
        var statistics = StatisticsCalculator.Calculate(packetsData);
        WriteStatisticsOnFile.Write(
            statistics, 
            configuration.ConsumerQosLogsFileWindows, 
            configuration.ConsumerQosLogsFileUnix);
        statisticsData.Add(statistics);
        packetsData.Clear();
    }

    if(benchmarkData.MessageNumber == configuration.NumberOfRuns * configuration.NumberOfMessagesPerRun)
    {
        var runsStatistics = StatisticsCalculator.Calculate(statisticsData);
        WriteStatisticsOnFile.Write(
            runsStatistics,
            configuration.ConsumerQosLogsFileWindows,
            configuration.ConsumerQosLogsFileUnix);
    }
}