namespace SharedDomain.BenchmarkUtils
{
    public static class StatisticsCalculator
    {
        public static StatisticsData Calculate(List<BenchmarkData> packetsData)
        {
            var globalTimePeriod = GetGlobalTimePeriod(packetsData);

            return StatisticsData.Create(
                throughput: CalculateThroughput(packetsData, globalTimePeriod),
                timePeriodOfBenchmark: globalTimePeriod,
                jitter: CalculateJitter(packetsData));
        }

        public static RunsStatisticsData Calculate(List<StatisticsData> runsData) 
        {
            return RunsStatisticsData.Create(
                averageThroughput: runsData.Sum(x => x.Throughput) / runsData.Count,
                averageJitter: runsData.Sum(x => x.Jitter) / runsData.Count,
                maxThroughput: runsData.Max(x => x.Throughput),
                maxJitter: runsData.Max(x => x.Jitter),
                minThroughput: runsData.Min(x => x.Throughput),
                minJitter: runsData.Min(x => x.Jitter));
        }

        private static TimeSpan GetGlobalTimePeriod(List<BenchmarkData> packetsData)
        {
            var orderedPackets = packetsData.OrderBy(p => p.MessageNumber).ToList();
            var firstPacket = orderedPackets.First();
            var lastPacket = orderedPackets.Last();
            return lastPacket.ReceivedTime - firstPacket.SentTime;
        }

        private static double CalculateThroughput(List<BenchmarkData> packetsData, TimeSpan timePeriod)
        {
            return packetsData.Count / timePeriod.TotalMilliseconds;
        }

        private static double CalculateJitter(List<BenchmarkData> packetsData)
        {
            var averageDelay = packetsData.Sum(p => p.PacketDelay.TotalMilliseconds) / packetsData.Count;
            double meanDelay = 0;
            foreach (var packet in packetsData)
            {
                meanDelay += Math.Pow(packet.PacketDelay.TotalMilliseconds - averageDelay, 2);
            }

            return Math.Sqrt(meanDelay / packetsData.Count);
        }
    }


}
