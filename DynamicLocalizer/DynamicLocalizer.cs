using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.Localization;

namespace DynamicLocalizer
{
    public class DynamicLocalizer: IDynamicLocalizer
    {
        private ConcurrentDictionary<string, string> _localizations;
        
        private readonly Func<Dictionary<string, string>> _loadResource;
        
        private readonly Func<CultureInfo, string> _formatCulture;
        
        private readonly string _defaultCulture;

        public DynamicLocalizer(DynamicLocalizerOption option)
        {
            if (option == null)
            {
                option = new DynamicLocalizerOption();
            }
            _loadResource = option.LoadResource;
            _formatCulture = option.FormatCulture;
            _defaultCulture = option.DefaultCulture;
        }
        
        public LocalizedString this[string name]
        {
            get
            {
                var text = GetText(name, out bool notSucceed);
                return new LocalizedString(name, text, notSucceed);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var text = GetText(name, out bool notSucceed);
                var value = name;
                try
                {
                    //format text
                    value = string.Format(text ?? name, arguments);
                }
                catch (Exception e)
                {
                    //ignore
                }

                return new LocalizedString(name, value, notSucceed);
            }
        }

        /// <summary>
        /// get text from resource by key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="notSucceed"></param>
        /// <returns></returns>
        private string GetText(string key, out bool notSucceed)
        {
            var culture = _formatCulture(CultureInfo.CurrentCulture);
            
            //use {key}.{culture} as unique key
            var computedKey = $"{key}.{culture}";

            //initialize resource
            if (_localizations == null)
            {
                ReloadResource();
            }

            //use currentCulture first
            if (_localizations.TryGetValue(computedKey, out var result))
            {
                notSucceed = false;
                return result;
            }

            //use default culture if failed with currentCulture
            computedKey = $"{key}.{_defaultCulture}";
            if (_localizations.TryGetValue(computedKey, out result))
            {
                notSucceed = false;
                return result;
            }

            //use key as test if failed with defaultCulture
            notSucceed = true;
            return key;
        }
        
        /// <summary>
        /// load resource
        /// </summary>
        /// <returns></returns>
        private ConcurrentDictionary<string, string> LoadResource()
        {
            try
            {
                var resource = _loadResource();
                return new ConcurrentDictionary<string, string>(resource);
            }
            catch (Exception e)
            {
                return new ConcurrentDictionary<string, string>();
            }
        }
        
        /// <summary>
        /// reload resource
        /// </summary>
        public void ReloadResource()
        {
            _localizations = LoadResource();
        }
    }
}