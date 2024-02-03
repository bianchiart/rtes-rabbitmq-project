namespace SharedDomain.BenchmarkUtils
{
    public class BenchmarkData
    {
        public long MessageNumber;
        public string Message;
        public TimeSpan SentTime;
        public TimeSpan ReceivedTime;
        public TimeSpan PacketDelay;
        public BenchmarkData(
            long messageNumber, 
            string message, 
            TimeSpan sentTime, 
            TimeSpan receivedTime,
            TimeSpan packetDelay)
        {
            MessageNumber = messageNumber;
            Message = message;
            SentTime = sentTime;
            ReceivedTime = receivedTime;
            PacketDelay = packetDelay;
        }

    }
}
