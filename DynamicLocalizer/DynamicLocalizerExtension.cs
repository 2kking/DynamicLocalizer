using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicLocalizer
{
    public static class DynamicLocalizerExtension
    {
        /// <summary>
        /// add DynamicLocalizer
        /// </summary>
        /// <param name="services"></param>
        /// <param name="option"></param>
        public static void AddDynamicLocalizer(this IServiceCollection services,
            DynamicLocalizerOption option)
        {
            services.AddSingleton<IDynamicLocalizer>(e => new DynamicLocalizer(option));
        }
        
        /// <summary>
        /// use DynamicLocalizer
        /// </summary>
        /// <param name="app"></param>
        public static void UseDynamicLocalizer(this IApplicationBuilder app)
        {
            app.UseMiddleware<DynamicRequestLocalizationMiddleware>();
        }
    }
}