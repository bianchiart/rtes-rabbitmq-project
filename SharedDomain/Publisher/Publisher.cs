using RabbitMQ.Client;
using SharedDomain.ConfigurationUtils;
using System.Text;

namespace SharedDomain.Publisher
{
    public class Publisher
    {
        private int _numberOfMessagesPerRun;
        private string _queueName;
        private int _numberOfRuns;
        private int _cooldownSeconds;
        private int _totalMessagesToSend;
        private int _interMessageDelayMilliseconds;
        public Publisher(Configuration configuration)
        {
            _numberOfMessagesPerRun = configuration.NumberOfMessagesPerRun;
            _queueName = configuration.QueueName;
            _numberOfRuns = configuration.NumberOfRuns;
            _cooldownSeconds = configuration.CooldownSeconds;
            _totalMessagesToSend = _numberOfMessagesPerRun * _numberOfRuns;
            _interMessageDelayMilliseconds = configuration.PublisherInterMessageDelayMilliseconds;
        }

        public void Execute(IModel channel)
        {
            channel.QueueDeclare(
                queue: _queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            SendMessages(channel);
        }

        private void SendMessages(IModel channel)
        {
            Console.WriteLine("Publisher is ready. Press [enter] when you want to start sending messages.");
            Console.ReadLine();

            for (int i = 1; i <= _totalMessagesToSend; i++)
            {
                var dateTimeNow = DateTime.UtcNow.TimeOfDay;
                var body = Encoding.UTF8.GetBytes(CreateMessageToSend(i, dateTimeNow));
                channel.BasicPublish(
                    exchange: string.Empty,
                    routingKey: _queueName,
                    basicProperties: null,
                    body: body);

                Console.WriteLine($"{dateTimeNow} : Sent message number {i}");
                
                ExecuteCooldowns(i);
            }

        }
        private string CreateMessageToSend(int messageIndex, TimeSpan now)
        {
            return $"{messageIndex},message,{now}";
        }

        private void ExecuteCooldowns(int messageIndex)
        {
            if( _interMessageDelayMilliseconds > 0 )
            {
                Thread.Sleep(_interMessageDelayMilliseconds);
            }

            if (messageIndex % _numberOfMessagesPerRun == 0)
            {
                if (_cooldownSeconds > 0)
                {
                    Thread.Sleep(_cooldownSeconds * 1000);
                }
            }
        }
    }
}
