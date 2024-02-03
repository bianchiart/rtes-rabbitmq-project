using SharedDomain.BenchmarkUtils.Models;

namespace SharedDomain.BenchmarkUtils
{
    public static class WriteStatisticsOnFile
    {
        public static void Write(StatisticsData data, string windowsFilePath, string unixFilePath)
        {
            var actualPath = GetEnvironmentFilePath(windowsFilePath, unixFilePath);
            File.AppendAllText(actualPath, data.ToString() + Environment.NewLine);
            Console.WriteLine(data.ToString());
        }

        public static void Write(RunsStatisticsData data, string windowsFilePath, string unixFilePath)
        {
            var actualPath = GetEnvironmentFilePath(windowsFilePath, unixFilePath);
            File.AppendAllText(actualPath, data.ToString() + Environment.NewLine);
            Console.WriteLine(data.ToString());
        }

        private static string GetEnvironmentFilePath(string windowsFilePath, string unixFilePath)
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                return unixFilePath;
            }
            else
            {
                return windowsFilePath;
            }
        }
    }
}
