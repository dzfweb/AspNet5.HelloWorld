using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Framework.Localization;
using System;
using System.Globalization;
using System.Reflection;

namespace Microsoft.AspNet.Localization
{
    public interface IHtmlLocalizerFactory
    {
        IHtmlLocalizer Create(Type resourceSource);
        IHtmlLocalizer Create(string baseName, string location);
    }

    public interface IHtmlLocalizer<TResourceSource> : IHtmlLocalizer
    {
    }

    public interface IViewLocalizer : IHtmlLocalizer
    {
    }

    public interface IHtmlLocalizer : ILocalizer
    {
        new IHtmlLocalizer WithCulture(CultureInfo culture);

        LocalizedHtmlString Html(string key);

        LocalizedHtmlString Html(string key, params object[] arguments);
    }

    public class LocalizedHtmlString : HtmlString
    {
        public LocalizedHtmlString(string key, string value, bool resourceNotFound) : base(value)
        {
            Key = key;
        }

        public string Key { get; private set; }

        public string Value { get { return ToString(); } }

        public bool ResourceNotFound { get; private set; }
    }
}
