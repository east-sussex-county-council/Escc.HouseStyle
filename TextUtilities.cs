using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using System.Web;

namespace EsccWebTeam.HouseStyle
{
    /// <summary>
    /// Utility methods for working with text
    /// </summary>
    public static class TextUtilities
    {
        #region Work with .resx resources
        /// <summary>
        /// Gets a localised string from the calling application's global resource file, or an internal resource file if the application does not have a global resource file. Requires ReflectionPermission.
        /// </summary>
        /// <typeparam name="ResourceFile">The type of the resource file.</typeparam>
        /// <param name="resourceKey">The resource key.</param>
        /// <returns></returns>
        public static string ResourceString<ResourceFile>(string resourceKey)
        {
            Type resourceType = typeof(ResourceFile);

            string localised = HttpContext.GetGlobalResourceObject(resourceType.Name, resourceKey) as string;

            if (localised == null)
            {
                PropertyInfo resourceManagerProperty = resourceType.GetProperty("ResourceManager", (BindingFlags.Static | BindingFlags.NonPublic));
                ResourceManager manager = (ResourceManager)resourceManagerProperty.GetValue(null, null);
                if (manager != null) localised = manager.GetString(resourceKey);
            }

            return localised;
        }

        /// <summary>
        /// Gets a localised string from the calling application's global resource file, or an internal resource file if the application does not have a global resource file
        /// </summary>
        /// <param name="resourceFileName">Name of the resource file, without the .resx extension</param>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static string ResourceString(string resourceFileName, string resourceKey, string defaultValue)
        {
            string localised = null;
            localised = HttpContext.GetGlobalResourceObject(resourceFileName, resourceKey) as string;
            if (localised == null) localised = defaultValue;
            return localised;
        }
        #endregion
    }
}
