// ReSharper disable InconsistentNaming

using System;

namespace BeckyPlugin.BeckyApi
{

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

    public enum BeckyBitmap {
        INVALID = 0,
        BKC_BITMAP_ADDRESSBOOKICON = 1,
        BKC_BITMAP_ADDRESSPERSON = 2,
        BKC_BITMAP_ANIMATION = 3,
        BKC_BITMAP_FOLDERCOLOR = 4,
        BKC_BITMAP_FOLDERICON = 5,
        BKC_BITMAP_LISTICON = 6,
        BKC_BITMAP_PRIORITYSTAMP = 7,
        BKC_BITMAP_RULETREEICON = 8,
        BKC_BITMAP_TEMPLATEFOLDER = 9,
        BKC_BITMAP_WHATSNEWLIST = 10,
        BKC_BITMAP_LISTICON2 = 11,
        BKC_BITMAP_AGENTS = 12,
    }

    public enum BeckyIcon {
        INVALID = 0,
        BKC_ICON_ADDRESSBOOK = 101,
        BKC_ICON_ANIMATION1_SMALL = 102,
        BKC_ICON_ANIMATION2_SMALL = 103,
        BKC_ICON_COMPOSEFRAME = 104,
        BKC_ICON_MAINFRAME = 105,
        BKC_ICON_NEWARRIVAL1_SMALL = 106,
        BKC_ICON_NEWARRIVAL2_SMALL = 107,
    }


    public enum BeckyToolbar {
        INVALID = 0,
        BKC_TOOLBAR_ADDRESSBOOK = 201,
        BKC_TOOLBAR_COMPOSEFRAME = 202,
        BKC_TOOLBAR_HTMLEDITOR = 203,
        BKC_TOOLBAR_MAINFRAME = 204,
    }

    public enum BeckyOnSend {
        /// <summary>
        ///   Did nothing.
        /// </summary>
        NOTHING = 0,
        /// <summary>
        ///    Return BKC_ONSEND_ERROR, if you want to cancel the sending operation.
        ///   You are responsible for displaying an error message.
        /// </summary>
        BKC_ONSEND_ERROR = -1,
        /// <summary>
        ///   if you have processed this message
        ///   and don't need Becky! to send it.
        ///   Becky! will move this message to Sent box when the sending
        ///   operation is done.
        /// </summary>
        /// <remarks>
        ///   CAUTION: You are responsible for the destination of this
        ///       message if you return BKC_ONSEND_PROCESSED.
        /// </remarks>
        BKC_ONSEND_PROCESSED = -2,
    }

    public enum BeckyFilter {
        /// <summary>
        /// Do nothing and apply default filtering rules.
        /// </summary>
        BKC_FILTER_DEFAULT = 0,
        /// <summary>
        /// Apply default filtering rules after applying the rule it returns.
        /// </summary>
        BKC_FILTER_PASS = 1,
        /// <summary>
        /// Do not apply default rules.
        /// </summary>
        BKC_FILTER_DONE = 2,
        /// <summary>
        /// Request Becky! to call this callback again so that another rules can be added.
        /// </summary>
        BKC_FILTER_NEXT = 3,
    }

    public enum BeckyAction : int {
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

    [Flags]
    public enum BeckyMessage {
        NONE = 0,
        /// <summary>
        /// Message is read.
        /// </summary>
        MESSAGE_READ = 0x00000001,
        /// <summary>
        /// Message is forwarded.
        /// </summary>
        MESSAGE_FORWARDED = 0x00000002,
        /// <summary>
        /// Message is replied.
        /// </summary>
        MESSAGE_REPLIED = 0x00000004,
        /// <summary>
        ///  Message has attachments
        /// </summary>
        MESSAGE_ATTACHMENT = 0x00000008,
        /// <summary>
        /// Message is a part of message/partial
        /// </summary>
        MESSAGE_PARTIAL = 0x00000100,
        /// <summary>
        /// Message is sent as a redirected message.
        /// (Resent- headers are found.)
        /// </summary>
        MESSAGE_REDIRECT = 0x00000200,

        UNDOCUMENTED_1 = 0x02000,
        UNDOCUMENTED_2 = 0x10000,
    }

    public enum BeckyComposeMode {
        /// <summary>
        /// Compose new message.
        /// </summary>
        COMPOSE_MODE_COMPOSE1 = 0,
        /// <summary>
        /// Compose to replying address.
        /// </summary>
        COMPOSE_MODE_COMPOSE2 = 1,
        /// <summary>
        ///  Compose to selected addresses.
        /// </summary>
        COMPOSE_MODE_COMPOSE3 = 2,
        /// <summary>
        /// Edit/Create a template.
        /// </summary>
        COMPOSE_MODE_TEMPLATE = 3,
        /// <summary>
        /// Reply
        /// </summary>
        COMPOSE_MODE_REPLY1 = 5,
        /// <summary>
        /// Reply to All
        /// </summary>
        COMPOSE_MODE_REPLY2 = 6,
        /// <summary>
        /// Reply to selected addresses.
        /// </summary>
        COMPOSE_MODE_REPLY3 = 7,
        /// <summary>
        /// Forward.
        /// </summary>
        COMPOSE_MODE_FORWARD1 = 10,
        /// <summary>
        ///  Redirect.
        /// </summary>
        COMPOSE_MODE_FORWARD2 = 11,
        /// <summary>
        /// Forward as attachments.
        /// </summary>
        COMPOSE_MODE_FORWARD3 = 12,
    }

    [Flags]
    public enum BeckyCmdUI : uint {
        INVALID = 0,
        /// <summary>
        ///   menu item will be grayed out and the command will never been called.
        /// </summary>
        BKMENU_CMDUI_DISABLED = 1,
        /// <summary>
        /// menu item will be checked.
        /// </summary>
        BKMENU_CMDUI_CHECKED = 2,
    }



    public enum BeckyOutgoingMode {
        /// <summary>
        /// Save to outbox.
        /// </summary>
        OUTGOINGMODE_OUTBOX = 0,
        /// <summary>
        ///  Save to draft box.
        /// </summary>
        OUTGOINGMODE_DRAFTBOX = 1,
        /// <summary>
        /// Save as a reminder.
        /// </summary>
        OUTGOINGMODE_REMINDER = 2,
    }

    public enum BeckyShiftMode {
        /// <summary>
        ///   Shift+
        /// </summary>
        MODIFIER_SHIFT = 0x40,
        /// <summary>
        ///   Ctrl+
        /// </summary>
        MODIFIER_CTRL = 0x20,
        /// <summary>
        ///   Shift+Ctrl+
        /// </summary>
        MODIFIER_SHIFTCTRL = 0x60,
        /// <summary>
        ///   Alt+
        /// </summary>
        MODIFIER_ALT = 0xfe,
    }

    public enum BeckyDropEffect {
        INVALID = 0,
        /// <summary>
        /// Copy
        /// </summary>
        DROPEFFECT_COPY = 1,
        /// <summary>
        /// Move
        /// </summary>
        DROPEFFECT_MOVE = 2,
        /// <summary>
        /// Link (Used for filtering setup in Becky!)
        /// </summary>
        DROPEFFECT_LINK = 4,
    }

    public enum TextMode {
        /// <summary>
        /// Insert at the top of the existing text.
        /// </summary>
        INSERT_TOP = -1,
        /// <summary>
        /// Insert at the top of the existing text temporarily.
        /// </summary>
        INSERT_TOP_TEMP = -2,
        /// <summary>
        /// Replace the existing text.
        /// </summary>
        REPLACE = 0,
        /// <summary>
        /// Append to the existing text.
        /// </summary>
        APPEND = 1,
        /// <summary>
        /// Append to the existing text temporarily.
        /// </summary>
        APPEND_TEMP = 2,
    }
}