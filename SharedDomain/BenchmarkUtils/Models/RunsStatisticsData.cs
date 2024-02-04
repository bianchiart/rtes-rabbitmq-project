namespace SharedDomain.BenchmarkUtils.Models
{
    public class RunsStatisticsData
    {
        public double AverageThroughput { get; set; }
        public double AverageJitter { get; set; }
        public double ThroughputVariation { get; set; }
        public double MaxThroughput { get; set; }
        public double MaxJitter { get; set; }
        public double MinThroughput { get; set; }
        public double MinJitter { get; set; }

        private RunsStatisticsData(
            double averageThroughput,
            double averageJitter,
            double throughputVariation,
            double maxThroughput,
            double maxJitter,
            double minThroughput,
            double minJitter)
        {
            AverageThroughput = averageThroughput;
            AverageJitter = averageJitter;
            ThroughputVariation = throughputVariation;
            MaxThroughput = maxThroughput;
            MaxJitter = maxJitter;
            MinThroughput = minThroughput;
            MinJitter = minJitter;
        }

        public static RunsStatisticsData Create(
            double averageThroughput,
            double averageJitter,
            double throughputVariation,
            double maxThroughput,
            double maxJitter,
            double minThroughput,
            double minJitter)
        {
            return new RunsStatisticsData(
                averageThroughput,
                averageJitter,
                throughputVariation,
                maxThroughput,
                maxJitter,
                minThroughput,
                minJitter);
        }
        public override string ToString()
        {
            return $"Runs Completed.{Environment.NewLine}" +
                $"Avg throughput : {AverageThroughput} msg/ms {Environment.NewLine}" +
                $"Avg jitter : {AverageJitter} ms {Environment.NewLine}" +
                $"Throughput variation: {ThroughputVariation} msg/ms {Environment.NewLine}" +
                $"Max throughput : {MaxThroughput} msg/ms {Environment.NewLine}" +
                $"Min jitter was {MinJitter} ms {Environment.NewLine}" +
                $"Min throughput was {MinThroughput} msg/ms {Environment.NewLine}" +
                $"Max jitter was {MaxJitter} ms {Environment.NewLine}"+
                $"------------------------------------------------";
                
        }
    }
}
