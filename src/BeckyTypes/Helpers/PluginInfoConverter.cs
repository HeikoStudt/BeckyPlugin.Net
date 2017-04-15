using System.IO;
using BeckyTypes.PluginListener;
using BeckyPlugin.Helpers;

namespace BeckyTypes.Helpers {

    public static class PluginInfoConverter {

        /// <summary>
        ///   Fills the API driven struct to be marshalled to Becky.
        /// </summary>
        public static void FillStruct(this IPluginInfo pluginInfo, ref Api_TagBkPlugininfo lpPlugInInfo) {
            if (pluginInfo == null) {
                throw new InvalidDataException(
                    "You have to set event OnPlugInInfo and return not-null.");
            }
            if (string.IsNullOrWhiteSpace(pluginInfo.PluginName) || string.IsNullOrWhiteSpace(pluginInfo.Vendor)) {
                throw new InvalidDataException(
                    "PlugInInfo need to set pluginname and vendor of the plugin.");
            }
            lpPlugInInfo.szPlugInName = pluginInfo.PluginName.ToCharArray(80);
            lpPlugInInfo.szVendor = pluginInfo.Vendor.ToCharArray(80);
            lpPlugInInfo.szVersion = pluginInfo.Version.ToCharArray(80);
            lpPlugInInfo.szDescription = pluginInfo.Description.ToCharArray(256);
        }
    }
}