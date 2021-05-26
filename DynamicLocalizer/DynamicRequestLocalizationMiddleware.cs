using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;

namespace DynamicLocalizer
{
    /// <summary>
    /// dynamic RequestLocalizationMiddleware
    /// </summary>
    public class DynamicRequestLocalizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RequestLocalizationOptions _options;

        /// <summary>
        /// Creates a new <see cref="RequestLocalizationMiddleware"/>.
        /// </summary>
        /// <param name="next">The <see cref="RequestDelegate"/> representing the next middleware in the pipeline.</param>
        /// <param name="options">The <see cref="RequestLocalizationOptions"/> representing the options for the
        /// <see cref="DynamicRequestLocalizationMiddleware"/>.</param>
        public DynamicRequestLocalizationMiddleware(RequestDelegate next, IOptions<RequestLocalizationOptions> options)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _options = options?.Value ?? new RequestLocalizationOptions();
        }

        /// <summary>
        /// Invokes the logic of the middleware.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/>.</param>
        /// <returns>A <see cref="Task"/> that completes when the middleware has completed processing.</returns>
        public async Task Invoke(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var requestCulture = _options.DefaultRequestCulture;

            IRequestCultureProvider winningProvider = null;

            if (_options.RequestCultureProviders != null)
            {
                foreach (var provider in _options.RequestCultureProviders)
                {
                    var providerResultCulture = await provider.DetermineProviderCultureResult(context);
                    if (providerResultCulture == null)
                    {
                        continue;
                    }

                    var cultures = providerResultCulture.Cultures;
                    var uiCultures = providerResultCulture.UICultures;

                    var cultureInfo = new CultureInfo(cultures.FirstOrDefault().Value);
                    var uiCultureInfo = new CultureInfo(uiCultures.FirstOrDefault().Value);

                    var result = new RequestCulture(cultureInfo, uiCultureInfo);
                    
                    requestCulture = result;
                    winningProvider = provider;
                    break;
                }
            }

            context.Features.Set<IRequestCultureFeature>(new RequestCultureFeature(requestCulture, winningProvider));

            SetCurrentThreadCulture(requestCulture);

            await _next(context);
        }

        /// <summary>
        /// set current culture
        /// </summary>
        /// <param name="requestCulture"></param>
        private static void SetCurrentThreadCulture(RequestCulture requestCulture)
        {
            CultureInfo.CurrentCulture = requestCulture.Culture;
            CultureInfo.CurrentUICulture = requestCulture.UICulture;
        }
    }
}