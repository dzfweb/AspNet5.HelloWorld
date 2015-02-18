using System;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Microsoft.Framework.Localization
{
    public class LocalizerFactory : ILocalizerFactory
    {
        public virtual ILocalizer Create(Type resourceSource)
        {
            return new Localizer(new ResourceManager(resourceSource));
        }

        public virtual ILocalizer Create(string baseName, string location)
        {
            var assembly = Assembly.Load(new AssemblyName(location));
            return new Localizer(new ResourceManager(baseName, assembly));
        }
    }

    public class Localizer : ILocalizer
    {
        protected readonly ResourceManager _resources;

        public Localizer(ResourceManager resources)
        {
            _resources = resources;
        }

        public ILocalizer WithCulture(CultureInfo culture)
        {
            return culture == null ? new Localizer(_resources) : new CultureLocalizer(_resources, culture);
        }

        public virtual LocalizedString this[string key] => Get(key);

        public virtual LocalizedString this[string key, params object[] arguments] => Get(key, arguments);

        public virtual LocalizedString Get(string key)
        {
            var value = _resources.GetString(key);
            return new LocalizedString(key, value ?? key, resourceNotFound: value == null);
        }

        public virtual LocalizedString Get(string key, params object[] arguments)
        {
            var format = _resources.GetString(key);
            var value = string.Format(format ?? key, arguments);
            return new LocalizedString(key, value, resourceNotFound: format == null);
        }
    }

    public class CultureLocalizer : Localizer
    {
        protected readonly CultureInfo _culture;

        public CultureLocalizer(ResourceManager resources, CultureInfo culture) : base(resources)
        {
            _culture = culture;
        }

        public override LocalizedString Get(string key)
        {
            var value = _resources.GetString(key, _culture);
            return new LocalizedString(key, value ?? key, resourceNotFound: value == null);
        }

        public override LocalizedString Get(string key, params object[] arguments)
        {
            var format = _resources.GetString(key, _culture);
            var value = string.Format(_culture, format ?? key, arguments);
            return new LocalizedString(key, value, resourceNotFound: format == null);
        }
    }

    public class Localizer<TResourceSource> : ILocalizer<TResourceSource>
    {
        private ILocalizer _localizer;

        public Localizer(ILocalizerFactory factory)
        {
            _localizer = factory.Create(typeof(TResourceSource));
        }

        public virtual ILocalizer WithCulture(CultureInfo culture) => _localizer.WithCulture(culture);

        public virtual LocalizedString this[string key] => _localizer[key];

        public virtual LocalizedString this[string key, params object[] arguments] => _localizer[key, arguments];

        public virtual LocalizedString Get(string key) => _localizer.Get(key);

        public virtual LocalizedString Get(string key, params object[] arguments) => _localizer.Get(key, arguments);
    }
}
