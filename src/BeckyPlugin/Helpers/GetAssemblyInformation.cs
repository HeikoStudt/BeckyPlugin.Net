using System;
using System.Linq;
using System.Reflection;

namespace BeckyPlugin.Helpers
{
    public static class GetAssemblyInformation
    {
        public static readonly Assembly Reference = typeof(GetAssemblyInformation).Assembly;
        public static readonly Version Version = Reference.GetName().Version;

        public static string Description
        {
            get
            {
                var descriptionAttribute = Reference
                    .GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)
                    .OfType<AssemblyDescriptionAttribute>()
                    .FirstOrDefault();

                return descriptionAttribute?.Description;
            }
        }

        public static string Vendor
        {
            get
            {
                var companyAttribute = Reference
                       .GetCustomAttributes(typeof(AssemblyCompanyAttribute), false)
                       .OfType<AssemblyCompanyAttribute>()
                       .FirstOrDefault();

                return companyAttribute?.Company;
            }
        }

        public static string Title
        {
            get
            {
                var titleAttribute = Reference
                       .GetCustomAttributes(typeof(AssemblyTitleAttribute), false)
                       .OfType<AssemblyTitleAttribute>()
                       .FirstOrDefault();

                return titleAttribute?.Title;
            }
        }
    }
}
