using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Microsoft.Framework.Localization
{
    // TODO:
    // - Remove reliance on Assembly, base APIs should just be string/scope based
    // - base API should be stream based (can't do on coreclr)
    // - higher level nice APIs should be extension methods
    // - not convinced we need the ILocalizer.SelectCulture
    // - Add IViewLocalizer that can be injected into views (should do this by default as 'Resources' property)
    // - Enumerate to download all resources 

    public interface ILocalizerFactory
    {
        ILocalizer Create(Type resourceSource);

        ILocalizer Create(string baseName, string location);
    }

    public interface ILocalizer<TResourceSource> : ILocalizer
    {
    }

    public interface ILocalizer
    {
        ILocalizer WithCulture(CultureInfo culture);

        LocalizedString Get(string key);

        LocalizedString Get(string key, params object[] arguments);

        LocalizedString this[string key] { get; }

        LocalizedString this[string key, params object[] arguments] { get; }
    }

    public struct LocalizedString
    {
        public LocalizedString(string key, string value, bool resourceNotFound)
        {
            Key = key;
            Value = value;
            ResourceNotFound = resourceNotFound;
        }

        public static implicit operator string (LocalizedString localizedString) => localizedString.Value;

        public string Key { get; private set; }

        public string Value { get; private set; }

        public bool ResourceNotFound { get; private set; }

        public override string ToString() => Value;
    }
}
