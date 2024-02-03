using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDomain.BenchmarkUtils
{
    public class StatisticsData
    {
        public double Throughput;
        public TimeSpan TimePeriodOfBenchmark;
        public double Jitter;

        private StatisticsData(double throughput, TimeSpan timePeriodOfBenchmark, double jitter)
        {
            Throughput = throughput;
            TimePeriodOfBenchmark = timePeriodOfBenchmark;
            Jitter = jitter;
        }

        public static StatisticsData Create(double throughput, TimeSpan timePeriodOfBenchmark, double jitter)
        {
            return new StatisticsData(throughput, timePeriodOfBenchmark, jitter);
        }

        public override string ToString()
        {
            return $"On a time period of {TimePeriodOfBenchmark}, throughput was {Throughput} msg/ms and jitter was {Jitter} ms" + Environment.NewLine;
        }
    }
}
