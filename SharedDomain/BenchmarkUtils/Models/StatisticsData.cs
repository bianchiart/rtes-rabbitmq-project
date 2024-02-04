namespace SharedDomain.BenchmarkUtils.Models
{
    public class StatisticsData
    {
        public int RunIndex;
        public double Throughput;
        public TimeSpan TimePeriodOfBenchmark;
        public double Jitter;

        private StatisticsData(int runIndex, double throughput, TimeSpan timePeriodOfBenchmark, double jitter)
        {
            RunIndex = runIndex;
            Throughput = throughput;
            TimePeriodOfBenchmark = timePeriodOfBenchmark;
            Jitter = jitter;
        }

        public static StatisticsData Create(int runIndex, double throughput, TimeSpan timePeriodOfBenchmark, double jitter)
        {
            return new StatisticsData(runIndex, throughput, timePeriodOfBenchmark, jitter);
        }

        public override string ToString()
        {
            return $"Run number: {RunIndex} {Environment.NewLine}" +
                $"Time period {TimePeriodOfBenchmark.Seconds} s {TimePeriodOfBenchmark.Milliseconds} ms {Environment.NewLine}" +
                $"Throughput : {Throughput} msg/ms{Environment.NewLine}" +
                $"Jitter : {Jitter} ms";
        }
    }
}
