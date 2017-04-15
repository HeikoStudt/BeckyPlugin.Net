using System.Runtime.InteropServices;

namespace BeckyTypes.PluginListener
{
    /// <summary>
    ///   Do not use in your code.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct Api_TagBkPlugininfo {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]
        public char[] szPlugInName; // Name of the plug-in
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]
        public char[] szVendor; // Name of the vendor
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]
        public char[] szVersion; // Version string
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public char[] szDescription; // Short description about this plugin
    };

    
    /// <summary>
    ///   Standard dto implementation of IPluginInfo
    /// </summary>
    public class PluginInfo : IPluginInfo {
        
        public string PluginName { get; set; }

        public string Vendor { get; set; }

        public string Version { get; set; }

        public string Description { get; set; }
    }
}
