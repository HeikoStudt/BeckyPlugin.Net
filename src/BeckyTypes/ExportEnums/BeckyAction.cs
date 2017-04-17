// ReSharper disable InconsistentNaming

namespace BeckyTypes.ExportEnums
{
    /// <summary>
    ///   Important: This enum is defined in both DllExported as well as CallsIntoBecky
    ///   as for seperating.
    /// </summary>
    public enum BeckyAction {
        /// <summary>
        /// Do nothing
        /// </summary>
        ACTION_NOTHING = -1,
        /// <summary>
        /// Move to a folder
        /// </summary>
        ACTION_MOVEFOLDER = 0,
        /// <summary>
        ///  Set the color label
        /// </summary>
        ACTION_COLORLABEL = 1,
        /// <summary>
        /// Set the flag
        /// </summary>
        ACTION_SETFLAG = 2,
        /// <summary>
        /// Make a sound
        /// </summary>
        ACTION_SOUND = 3,
        /// <summary>
        /// Run executable file
        /// </summary>
        ACTION_RUNEXE = 4,
        /// <summary>
        ///  Reply to the message
        /// </summary>
        ACTION_REPLY = 5,
        /// <summary>
        /// Forward the message
        /// </summary>
        ACTION_FORWARD = 6,
        /// <summary>
        /// Leave/delete on the server.
        /// </summary>
        ACTION_LEAVESERVER = 7,
        /// <summary>
        /// Add "X-" header to the top of the message. (Plug-in only feature)
        /// </summary>
        ACTION_ADDHEADER = 8,
    }
}