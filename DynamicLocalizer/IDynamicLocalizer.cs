using Microsoft.Extensions.Localization;

namespace DynamicLocalizer
{
    public interface IDynamicLocalizer
    {
        LocalizedString this[string name]
        {
            get;
        }

        LocalizedString this[string name, params object[] arguments]
        {
            get;
        }

        void ReloadResource();
    }
}