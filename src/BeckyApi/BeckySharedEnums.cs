// ReSharper disable InconsistentNaming

namespace BeckyApi
{
    /// <summary>
    ///   Important: This enum is defined in both DllExported as well as CallsIntoBecky
    ///   as for seperating.
    /// </summary>
    public enum BeckyMenu {
        /// <summary>
        /// Menu bar of the main window
        /// </summary>
        BKC_MENU_MAIN = 0,
        /// <summary>
        /// Context menu of ListView window
        /// </summary>
        BKC_MENU_LISTVIEW = 1,
        /// <summary>
        /// Context menu of TreeView window
        /// </summary>
        BKC_MENU_TREEVIEW = 2,
        /// <summary>
        /// Context menu of message view Window
        /// </summary>
        BKC_MENU_MSGVIEW = 3,
        /// <summary>
        /// Context menu of message view Window when it's editable
        /// </summary>
        BKC_MENU_MSGEDIT = 4,
        /// <summary>
        /// Popup menu of the task tray icon
        /// </summary>
        BKC_MENU_TASKTRAY = 5,
        /// <summary>
        /// Menu bar of the composing window
        /// </summary>
        BKC_MENU_COMPOSE = 10,
        /// <summary>
        /// Context menu of the editor of the composing window.
        /// </summary>
        BKC_MENU_COMPEDIT = 11,
        /// <summary>
        /// Context menu of the reference view of the composing window
        /// </summary>
        BKC_MENU_COMPREF = 12,
    }


   
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