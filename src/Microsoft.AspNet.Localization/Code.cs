using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.WebUtilities.Encoders;
using Microsoft.Framework.Localization;
using System;
using System.Globalization;
using System.Reflection;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Runtime;

namespace Microsoft.AspNet.Localization
{
    public class HtmlLocalizerFactory : IHtmlLocalizerFactory
    {
        private readonly ILocalizerFactory _factory;
        private readonly IHtmlEncoder _encoder;

        public HtmlLocalizerFactory(ILocalizerFactory localizerFactory, IHtmlEncoder encoder)
        {
            _factory = localizerFactory;
            _encoder = encoder;
        }

        public virtual IHtmlLocalizer Create(Type thing)
        {
            return new HtmlLocalizer(_factory.Create(thing), _encoder);
        }

        public virtual IHtmlLocalizer Create(string baseName, string location)
        {
            var localizer = _factory.Create(baseName, location);
            return new HtmlLocalizer(localizer, _encoder);
        }
    }

    public class HtmlLocalizer : IHtmlLocalizer
    {
        private readonly ILocalizer _localizer;
        private readonly IHtmlEncoder _encoder;

        public HtmlLocalizer(ILocalizer localizer, IHtmlEncoder encoder)
        {
            _localizer = localizer;
            _encoder = encoder;
        }

        public virtual IHtmlLocalizer WithCulture(CultureInfo culture) => new HtmlLocalizer(_localizer.WithCulture(culture), _encoder);

        ILocalizer ILocalizer.WithCulture(CultureInfo culture) => WithCulture(culture);

        public virtual LocalizedString this[string key] => _localizer[key];

        public virtual LocalizedString this[string key, params object[] arguments] => _localizer[key, arguments];

        public virtual LocalizedString Get(string key) => _localizer.Get(key);

        public virtual LocalizedString Get(string key, params object[] arguments) => _localizer.Get(key, arguments);

        public virtual LocalizedHtmlString Html(string key)
        {
            return ToHtmlString(_localizer.Get(key));
        }

        public virtual LocalizedHtmlString Html(string key, params object[] arguments)
        {
            return ToHtmlString(_localizer.Get(key, EncodeArguments(arguments)));
        }

        protected LocalizedHtmlString ToHtmlString(LocalizedString result)
        {
            return new LocalizedHtmlString(result.Key, result.Value, result.ResourceNotFound);
        }

        protected object[] EncodeArguments(object[] arguments)
        {
            object[] encodedArguments = new object[arguments.Length];
            for (var index = 0; index != arguments.Length; ++index)
            {
                var argument = arguments[index];
                if (argument.GetType().GetTypeInfo().IsPrimitive ||
                    argument is HtmlString ||
                    argument is DateTime ||
                    argument is DateTimeOffset)
                {
                    encodedArguments[index] = argument;
                }
                else
                {
                    encodedArguments[index] = _encoder.HtmlEncode(argument.ToString());
                }
            }
            return encodedArguments;
        }
    }

    public class HtmlLocalizer<TResourceSource> : IHtmlLocalizer<TResourceSource>
    {
        private readonly IHtmlLocalizer _localizer;

        public HtmlLocalizer(IHtmlLocalizerFactory factory)
        {
            _localizer = factory.Create(typeof(TResourceSource));
        }

        public virtual IHtmlLocalizer WithCulture(CultureInfo culture) => _localizer.WithCulture(culture);

        ILocalizer ILocalizer.WithCulture(CultureInfo culture) => _localizer.WithCulture(culture);

        public virtual LocalizedString this[string key] => _localizer[key];

        public virtual LocalizedString this[string key, params object[] arguments] => _localizer[key, arguments];

        public virtual LocalizedString Get(string key) => _localizer.Get(key);

        public virtual LocalizedString Get(string key, params object[] arguments) => _localizer.Get(key, arguments);

        public virtual LocalizedHtmlString Html(string key) => _localizer.Html(key);

        public virtual LocalizedHtmlString Html(string key, params object[] arguments) => _localizer.Html(key, arguments);
    }

    public class ViewLocalizer : IViewLocalizer, ICanHasViewContext
    {
        private readonly IHtmlLocalizerFactory _factory;
        private readonly IApplicationEnvironment _env;
        private IHtmlLocalizer _localizer;

        public ViewLocalizer(IHtmlLocalizerFactory factory, IApplicationEnvironment env)
        {
            _factory = factory;
            _env = env;
        }

        public void Contextualize(ViewContext viewContext)
        {
            _localizer = _factory.Create(
                _env.ApplicationName + viewContext.ExecutingFilePath.Replace('/', '.'),
                _env.ApplicationName);
        }

        public virtual IHtmlLocalizer WithCulture(CultureInfo culture) => _localizer.WithCulture(culture);

        ILocalizer ILocalizer.WithCulture(CultureInfo culture) => _localizer.WithCulture(culture);

        public virtual LocalizedString this[string key] => _localizer[key];

        public virtual LocalizedString this[string key, params object[] arguments] => _localizer[key, arguments];

        public virtual LocalizedString Get(string key) => _localizer.Get(key);

        public virtual LocalizedString Get(string key, params object[] arguments) => _localizer.Get(key, arguments);

        public virtual LocalizedHtmlString Html(string key) => _localizer.Html(key);

        public virtual LocalizedHtmlString Html(string key, params object[] arguments) => _localizer.Html(key, arguments);
    }
}

