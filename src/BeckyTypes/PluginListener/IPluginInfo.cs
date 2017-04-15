namespace BeckyTypes.PluginListener {

    /// <summary>
    ///   The information about the plugin given to Becky! 2
    /// </summary>
    public interface IPluginInfo {

        /// <summary>
        ///   The name of the plugin (should probably be unique)
        ///   Mandatory.
        ///   Maxlength: 80
        /// </summary>
        string PluginName { get; set; }

        /// <summary>
        ///   The vendor of the plugin (your name or nickname or such)
        ///   Mandatory.
        ///   Maxlength: 80
        /// </summary>
        string Vendor { get; set; }

        /// <summary>
        ///   The version of the plugin like 1.0.0.0
        ///   (Please provide)
        ///   Maxlength: 80
        /// </summary>
        string Version { get; set; }

        /// <summary>
        ///   Some description for the user, whether he likes the plugin to be installed.
        ///   Maxlength: 256
        /// </summary>
        string Description { get; set; }
    }
}
