namespace SharedDomain.BenchmarkUtils.Models
{
    public class StatisticsData
    {
        public int RunIndex;
        public double Throughput;
        public TimeSpan TimePeriodOfBenchmark;
        public double Jitter;
        public int NumberOfPackets;

        private StatisticsData(int runIndex, double throughput, TimeSpan timePeriodOfBenchmark, double jitter, int numberOfPackets)
        {
            RunIndex = runIndex;
            Throughput = throughput;
            TimePeriodOfBenchmark = timePeriodOfBenchmark;
            Jitter = jitter;
            NumberOfPackets = numberOfPackets;
        }

        public static StatisticsData Create(int runIndex, double throughput, TimeSpan timePeriodOfBenchmark, double jitter, int numberOfPackets)
        {
            return new StatisticsData(runIndex, throughput, timePeriodOfBenchmark, jitter, numberOfPackets);
        }

        public override string ToString()
        {
            return $"Run number: {RunIndex} {Environment.NewLine}" +
                $"Packets received: {NumberOfPackets} {Environment.NewLine}" +
                $"Time period {TimePeriodOfBenchmark.Seconds} s {TimePeriodOfBenchmark.Milliseconds} ms {Environment.NewLine}" +
                $"Throughput : {Throughput} msg/ms{Environment.NewLine}" +
                $"Jitter : {Jitter} ms";
        }
    }
}
