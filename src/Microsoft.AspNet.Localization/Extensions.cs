using Microsoft.AspNet.Localization;
using Microsoft.AspNet.WebUtilities.Encoders;
using System.Linq;

namespace Microsoft.Framework.DependencyInjection
{
    public static class HtmlLocalizationServiceCollectionExtensions
    {
        public static IServiceCollection AddHtmlLocalization(this IServiceCollection services)
        {
            services.AddSingleton<IHtmlLocalizerFactory, HtmlLocalizerFactory>();
            services.AddTransient(typeof(IHtmlLocalizer<>), typeof(HtmlLocalizer<>));
            services.AddTransient(typeof(IViewLocalizer), typeof(ViewLocalizer));
            if (!services.Any(sd => sd.ServiceType == typeof(IHtmlEncoder)))
            {
                services.AddInstance<IHtmlEncoder>(HtmlEncoder.Default);
            }
            return services.AddLocalization();
        }
    }
}
