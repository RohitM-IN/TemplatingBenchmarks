using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TemplatingBenchmarks.Helpers
{
    public static class CustomTemplatingHelper
    {
        public static void ReplacePlaceholder(ref StringBuilder stringBuilder, string placeholder, string replacement)
        {
            stringBuilder.Replace(placeholder, replacement);
        }
    }
}
