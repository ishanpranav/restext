using System;
using System.Resources;

namespace ResText
{
    internal static class Exceptions
    {
        private static readonly ResourceManager s_resourceManager = new ResourceManager(typeof(Exceptions));

        public static string AbstractDelegateString
        {
            get
            {
                return s_resourceManager.GetString(name: "AbstractDelegate") ?? throw new MissingManifestResourceException();
            }
        }

        public static string GetMissingGetAwaiterMethodString(Type type)
        {
            string? format = s_resourceManager.GetString(name: "MissingGetAwaiterMethod");

            if (format == null)
            {
                throw new MissingManifestResourceException();
            }
            else
            {
                return string.Format(format, type);
            }
        }
    }
}
