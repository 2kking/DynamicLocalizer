using System;
using System.Collections.Generic;
using System.Globalization;

namespace DynamicLocalizer
{
    /// <summary>
    /// option
    /// </summary>
    public class DynamicLocalizerOption
    {
        /// <summary>
        /// load resource: key value pair
        /// </summary>
        public Func<Dictionary<string, string>> LoadResource = () => new Dictionary<string, string>();
        /// <summary>
        /// format culture info: use CultureInfo.ToString() as default
        /// </summary>
        public Func<CultureInfo, string> FormatCulture = e => e.ToString();
        /// <summary>
        /// default culture: use zh_CN as default
        /// </summary>
        public string DefaultCulture = "zh_CN";
    }
}