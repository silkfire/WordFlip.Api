namespace Wordsmith.WordFlip.Benchmarks
{
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Environments;
    using BenchmarkDotNet.Jobs;
    using BenchmarkDotNet.Loggers;
    using BenchmarkDotNet.Running;

    using System;
    using System.Linq;


    public static class App
    {
        public static void Main()
        {
            var summary = BenchmarkRunner.Run<WordFlipBenchmarks>(new PowerPlanModeConfig(AppDomain.CurrentDomain.BaseDirectory));
        }


        private class PowerPlanModeConfig : ManualConfig
        {
            public PowerPlanModeConfig(string artifactsPath)
            {
                ArtifactsPath = artifactsPath;
                Options = ConfigOptions.DisableLogFile;

                Add(DefaultConfig.Instance.GetColumnProviders().ToArray());
                Add(new ConsoleLogger());

                Add(Job.Default.WithPowerPlan(PowerPlan.UserPowerPlan));
            }
        }
    }
}
