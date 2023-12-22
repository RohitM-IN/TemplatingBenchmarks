using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public class ConditionalBenchmark
    {
        private readonly string _SimpleConditionalTemplate = "{{#if IsAuthenticated}}Hello, {{name}}!{{else}}Hello, Guest!{{/if}}";
        private readonly string _LiquidTemplate = "Hello, {{ IsAuthenticated }} {{name}} {% else %} Guest {{% endif %}}";
        private readonly object _SimpleConditionalData = new { IsAuthenticated = true, name = "John" };
        private readonly string _SmartFormatTemplate = "Hello, {IsAuthenticated:{name}|Guest}"; 
        private readonly string _CustomTemplate = "Hello, {name}";

        #region DotLiquid

        private DotLiquid.Template _DotLiquidTemplate;

        [GlobalSetup(Target = nameof(DotLiquidBenchmark))]
        public void DotLiquidSetup()
        {
            _DotLiquidTemplate = DotLiquid.Template.Parse(this._LiquidTemplate);
        }

        [Benchmark]
        public string DotLiquidBenchmark()
        {
            return _DotLiquidTemplate.Render(DotLiquid.Hash.FromAnonymousObject(_SimpleConditionalData));
        }

        #endregion

        #region Fluid

        private IFluidTemplate _FluidTemplate;
        private TemplateContext _FluidContext;

        [GlobalSetup(Target = nameof(FluidBenchmark))]
        public void FluidSetup()
        {
            var _parser = new FluidParser();
            _FluidTemplate = _parser.Parse(_SimpleConditionalTemplate);
            _FluidContext = new TemplateContext(_SimpleConditionalData);
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
            _HandlebarsTemplate = Handlebars.Compile(_SimpleConditionalTemplate);
        }

        [Benchmark]
        public string HandlebarsNetBenchmark()
        {
            return _HandlebarsTemplate(_SimpleConditionalData);
        }

        #endregion

        #region Mustache-Sharp

        private Generator _MustacheTemplate;

        [GlobalSetup(Target = nameof(MustacheSharpBenchmark))]
        public void MustacheSharpSetup()
        {
            FormatCompiler compiler = new FormatCompiler();

            _MustacheTemplate = compiler.Compile(_SimpleConditionalTemplate);
        }


        [Benchmark]
        public string MustacheSharpBenchmark()
        {
            return _MustacheTemplate.Render(_SimpleConditionalData);
        }

        #endregion

        #region Scriban

        private Scriban.Template _ScribanTemplate;

        [GlobalSetup(Target = nameof(ScribanBenchmark))]
        public void ScribanSetup()
        {
            _ScribanTemplate = Scriban.Template.Parse(_SimpleConditionalTemplate);
        }

        [Benchmark()]
        public string ScribanBenchmark()
        {
            return _ScribanTemplate.Render(_SimpleConditionalData);
        }
        #endregion

        #region SmartFormat.Net


        [Benchmark]
        public string SmartFormatBenchmark()
        {
            return Smart.Format(_SmartFormatTemplate, _SimpleConditionalData);
        }

        #endregion

        #region Custom

        private StringBuilder builder;

        [GlobalSetup(Target =nameof(CustomBenchmark))]
        public void customSetup()
        {
            builder = new StringBuilder();
        }

        [Benchmark]
        public string CustomBenchmark()
        {
            builder.Clear();
            builder.Append(_CustomTemplate);

            string name = (_SimpleConditionalData as dynamic).name;
            bool isAuthanticated = (_SimpleConditionalData as dynamic).IsAuthenticated;

            if(isAuthanticated)
            {
                CustomTemplatingHelper.ReplacePlaceholder(ref builder, "{{name}}", name);
            }
            else
            {
                CustomTemplatingHelper.ReplacePlaceholder(ref builder, "{{name}}", "Guest");
            }

            return builder.ToString();
        }
        #endregion
    }
}
