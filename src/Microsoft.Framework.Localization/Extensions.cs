using Microsoft.Framework.Localization;

namespace Microsoft.Framework.DependencyInjection
{
    public static class LocalizationServiceCollectionExtensions
    {
        public static IServiceCollection AddLocalization(this IServiceCollection services)
        {
            services.AddSingleton<ILocalizerFactory, LocalizerFactory>();
            services.AddTransient(typeof(ILocalizer<>), typeof(Localizer<>));
            return services;
        }
    }
}