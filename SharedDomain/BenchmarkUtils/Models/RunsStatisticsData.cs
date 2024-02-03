namespace SharedDomain.BenchmarkUtils.Models
{
    public class RunsStatisticsData
    {
        public double AverageThroughput { get; set; }
        public double AverageJitter { get; set; }
        public double MaxThroughput { get; set; }
        public double MaxJitter { get; set; }
        public double MinThroughput { get; set; }
        public double MinJitter { get; set; }

        private RunsStatisticsData(
            double averageThroughput,
            double averageJitter,
            double maxThroughput,
            double maxJitter,
            double minThroughput,
            double minJitter)
        {
            AverageThroughput = averageThroughput;
            AverageJitter = averageJitter;
            MaxThroughput = maxThroughput;
            MaxJitter = maxJitter;
            MinThroughput = minThroughput;
            MinJitter = minJitter;
        }

        public static RunsStatisticsData Create(
            double averageThroughput,
            double averageJitter,
            double maxThroughput,
            double maxJitter,
            double minThroughput,
            double minJitter)
        {
            return new RunsStatisticsData(
                averageThroughput,
                averageJitter,
                maxThroughput,
                maxJitter,
                minThroughput,
                minJitter);
        }
        public override string ToString()
        {
            return $"In the period of the tests, avg throughput was {AverageThroughput} msg/ms and avg jitter was {AverageJitter} ms. {Environment.NewLine}" +
                $"Max throughput was {MaxThroughput} msg/ms and min jitter was {MinJitter} ms. {Environment.NewLine}" +
                $"Min throughput was {MinThroughput} msg/ms and max jitter was {MaxJitter} ms. {Environment.NewLine}";
        }
    }
}
