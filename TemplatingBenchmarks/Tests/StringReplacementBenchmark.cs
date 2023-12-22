using System.Text;

using BenchmarkDotNet.Attributes;
using Fluid;
using HandlebarsDotNet;
using Mustache;
using SmartFormat;
using TemplatingBenchmarks.Config;
using TemplatingBenchmarks.Helpers;

namespace TemplatingBenchmarks.Tests
{
    [MemoryDiagnoser]
    [Config(typeof(AntiVirusFriendlyConfig))]
    public class StringReplacementBenchmark
    {
        private readonly string _StringReplacementRawText = "Hello, {{name}}!";
        private readonly object _StringReplacementData = new { name = "John" };
        private readonly string _SmartFormatTemplate = "Hello, {name}!";
        private StringBuilder builder;

        #region DotLiquid

        private DotLiquid.Template _DotLiquidTemplate;

        [GlobalSetup(Target = nameof(DotLiquidBenchmark))]
        public void DotLiquidSetup()
        {
            _DotLiquidTemplate = DotLiquid.Template.Parse(this._StringReplacementRawText);
        }

        [Benchmark]
        public string DotLiquidBenchmark()
        {
            return _DotLiquidTemplate.Render(DotLiquid.Hash.FromAnonymousObject(_StringReplacementData));
        }

        #endregion

        #region Fluid

        private IFluidTemplate _FluidTemplate;
        private TemplateContext _FluidContext;

        [GlobalSetup(Target = nameof(FluidBenchmark))]
        public void FluidSetup()
        {
            var _parser = new FluidParser();
            _FluidTemplate = _parser.Parse(_StringReplacementRawText);
            _FluidContext = new TemplateContext(_StringReplacementData);
        }

        [Benchmark]
        public string FluidBenchmark()
        {
            return _FluidTemplate.Render(_FluidContext);
        }
        #endregion

        #region Handlebars.net

        private HandlebarsTemplate<object, object> _HandlebarsTemplate;

        [GlobalSetup(Target = nameof(HandlebarsNetBenchmark))]
        public void HandlebarsSetup()
        {
            _HandlebarsTemplate = Handlebars.Compile(_StringReplacementRawText);
        }

        [Benchmark]
        public string HandlebarsNetBenchmark()
        {
            return _HandlebarsTemplate(_StringReplacementData);
        }

        #endregion

        #region Mustache-Sharp

        private Generator _MustacheTemplate;

        [GlobalSetup(Target = nameof(MustacheSharpBenchmark))]
        public void MustacheSharpSetup()
        {
            FormatCompiler compiler = new FormatCompiler();

            _MustacheTemplate = compiler.Compile(_StringReplacementRawText);
        }


        [Benchmark]
        public string MustacheSharpBenchmark()
        {
            return _MustacheTemplate.Render(_StringReplacementData);
        }

        #endregion

        #region Scriban

        private Scriban.Template _ScribanTemplate;

        [GlobalSetup(Target = nameof(ScribanBenchmark))]
        public void ScribanSetup()
        {
            _ScribanTemplate = Scriban.Template.Parse(_StringReplacementRawText);
        }

        [Benchmark()]   
        public string ScribanBenchmark()
        {
            return _ScribanTemplate.Render(_StringReplacementData);
        }
        #endregion

        #region SmartFormat.Net


        [Benchmark]
        public string SmartFormatBenchmark()
        {
            return Smart.Format(_SmartFormatTemplate, _StringReplacementData);
        }

        #endregion

        #region Custom

        [GlobalSetup(Target = nameof(CustomBenchmark))]
        public void customSetup()
        {
            builder = new StringBuilder();
        }

        [Benchmark]
        public string CustomBenchmark()
        {
            builder.Clear();
            builder.Append(_StringReplacementRawText);

            CustomTemplatingHelper.ReplacePlaceholder(ref builder,"{{name}}", (_StringReplacementData as dynamic).name);

            return builder.ToString();
        }
        #endregion

    }
}
