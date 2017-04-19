using System;
using System.Linq;
using System.Reflection;
using BeckyTypes.PluginListener;

namespace BeckyTypes.Helpers {
    public static class GetAssemblyInformation {
        public static string GetDescription(Assembly reference) {
            var descriptionAttribute = reference
                .GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)
                .OfType<AssemblyDescriptionAttribute>()
                .FirstOrDefault();

            return descriptionAttribute?.Description;
        }

        public static string GetVendor(Assembly reference) {
            var companyAttribute = reference
                   .GetCustomAttributes(typeof(AssemblyCompanyAttribute), false)
                   .OfType<AssemblyCompanyAttribute>()
                   .FirstOrDefault();

            return companyAttribute?.Company;
        }

        public static string GetTitle(Assembly reference) {
            var titleAttribute = reference
                   .GetCustomAttributes(typeof(AssemblyTitleAttribute), false)
                   .OfType<AssemblyTitleAttribute>()
                   .FirstOrDefault();

            return titleAttribute?.Title;
        }

        public static IPluginInfo GetPluginInfo(Assembly reference) {
            Version Version = reference.GetName().Version;

            return new PluginInfo {
                PluginName = GetTitle(reference),
                Vendor = GetVendor(reference),
                Version = Version.ToString(),
                Description = GetDescription(reference),
            };
        }
    }
}
