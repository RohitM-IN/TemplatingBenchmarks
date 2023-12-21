using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;

namespace TemplatingBenchmarks.Config
{
    public class AntiVirusFriendlyConfig : ManualConfig
    {
        public AntiVirusFriendlyConfig()
        {
            AddJob(Job.ShortRun
                .WithToolchain(InProcessNoEmitToolchain.Instance));
        }
    }
}
