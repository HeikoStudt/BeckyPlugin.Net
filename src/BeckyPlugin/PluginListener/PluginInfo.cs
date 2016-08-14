namespace BeckyPlugin.PluginListener
{
    /// <summary>
    ///   The information about the plugin given to Becky! 2
    /// </summary>
    public class PluginInfo {
        /// <summary>
        ///   The name of the plugin (should probably be unique)
        ///   Mandatory.
        /// </summary>
        public string PluginName { get; set; }
        /// <summary>
        ///   The vendor of the plugin (your name or nickname or such)
        ///   Mandatory.
        /// </summary>
        public string Vendor { get; set; }
        /// <summary>
        ///   The version of the plugin like 1.0.0.0
        ///   (Please provide)
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        ///   Some description for the user, whether he likes the plugin to be installed.
        /// </summary>
        public string Description { get; set; }
    }
}