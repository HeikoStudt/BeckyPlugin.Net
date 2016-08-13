using System;
using System.Runtime.CompilerServices;

namespace BeckyPlugin.BeckyApi
{
    /// <summary>
    ///   Alls methods being called by Becky! 2 into the plugin.
    ///   (But the two resource ones).
    /// </summary>
    public interface IBeckyPlugin
    {
        /// <summary>
        ///   Called when the program is started and the main window is created.
        /// </summary>
        /// <remarks>
        /// Since BKC_OnStart is called after Becky!'s main window is
        /// created, at least BKC_OnMenuInit with BKC_MENU_MAIN is called
        /// before BKC_OnStart.So, do not assume BKC_OnStart is called
        /// prior to any other callback.
        /// </remarks>
        void OnStart();

        /// <summary>
        /// Called when the main window is closing.
        /// </summary>
        /// <remarks>
        /// Note that it is NOT guaranteed that BKC_OnExit is the last
        /// called callback.You should not free any of your objects, which
        /// can be used in other callbacks.
        /// </remarks>
        /// <returns>false if becky must not close main window</returns>
        bool OnExit();

        /// <summary>
        /// Called when menu is initialized.
        /// If you want to add your plug-in's menu to Becky!, implement this
        /// callback.
        /// </summary>
        /// <param name="hWnd">A window handle of the window which owns the menu</param>
        /// <param name="hMenu">A menu handle</param>
        /// <param name="nType">A type of the menu.</param>
        void OnMenuInit(IntPtr hWnd, IntPtr hMenu, BeckyMenu nType);

        /// <summary>
        /// Called when a folder is opened.
        /// </summary>
        /// <remarks>
        /// The folder ID is a part of an actual folder directory path.
        /// e.g. "1234abcd\!!!!Inbox\".
        /// You can pass those folder IDs to some API functions.
        /// If the folder ID ends with ".ini" (case insensitive), you can
        /// assume it is an IMAP4 remote folder.
        /// </remarks>
        /// <param name="lpFolderId">lpFolderID represents an ID of the folder which is opening.</param>
        void OnOpenFolder(string lpFolderId);

        /// <summary>
        /// Called when a mail is selected.
        /// </summary>
        /// <remarks>
        /// The message ID is a folder ID + "?" + an unique hexadecimal identifier.
        /// e.g. "1234abcd\!!!!Inbox\?5678cdef".
        /// You can pass those IDs to some API functions.
        /// </remarks>
        /// <param name="lpMailId">lpMailID represents an ID of the mail which is opening.</param>
        void OnOpenMail(string lpMailId);

        /// <summary>
        ///  Called every minute.
        /// </summary>
        void OnEveryMinute();

        /// <summary>
        ///   Called when plug-in information is being retrieved.
        /// </summary>
        /// <remarks>
        /// Note: You should not call any functions of Becky! API in this callback.
        ///       Some API functions can cause GPF.
        /// </remarks>
        /// <returns>
        ///   You MUST specify at least szPlugInName and szVendor.
        ///   otherwise Becky! will silently ignore your plug-in.
        /// </returns>
        PluginInfo OnPlugInInfo();

        /// <summary>
        /// Called when a compose windows is opened.
        /// </summary>
        /// <param name="hWnd">A window handle of the composing window.</param>
        /// <param name="nMode">The mode of the composing window.</param>
        void OnOpenCompose(IntPtr hWnd, BeckyComposeMode nMode);

        /// <summary>
        /// Called when the composing message is saved.
        /// </summary>
        /// <param name="hWnd">A window handle of the composing window.</param>
        /// <param name="nMode">How it will be processed:</param>
        /// <returns>Let it go.</returns>
        bool OnOutgoing(IntPtr hWnd, BeckyOutgoingMode nMode);

        /// <summary>
        ///   Called when a key is pressed.
        /// </summary>
        /// <param name="hWnd">A window handle of the target window in which the key will be processed.
        ///    or NULL for the main window.</param>
        /// <param name="nKey">Windows virtual key code defined in 'windows.h'</param>
        /// <param name="nShift">Shift state</param>
        /// <returns>suppress subsequent command?</returns>
        bool OnKeyDispatch(IntPtr hWnd, System.Windows.Forms.Keys nKey, BeckyShiftMode nShift);

        /// <summary>
        ///   Called when a message is retrieved and saved to a folder
        /// </summary>

        /// <param name="lpMessage"> A complete message source, which ends with
        /// "[CRLF].[CRLF]".</param>
        /// <param name="lpMailId">Mail ID of the new mail.</param>
        void OnRetrieve(string lpMessage, string lpMailId);

        /// <summary>
        ///   Called when a message is about to be sent
        /// </summary>
        /// <param name="lpMessage">A complete mail source, which ends with extra CRLF.</param>
        /// <returns>
        ///   Return BKC_ONSEND_PROCESSED, if you have processed this message
        ///   and don't need Becky! to send it.
        ///   Becky! will move this message to Sent box when the sending
        ///   operation is done.
        ///   CAUTION: You are responsible for the destination of this
        ///   message if you return BKC_ONSEND_PROCESSED.
        ///   Return BKC_ONSEND_ERROR, if you want to cancel the sending operation.
        ///   You are responsible for displaying an error message.
        /// </returns>
        BeckyOnSend OnSend(string lpMessage);

        /// <summary>
        ///  Called when all messages are retrieved in a session.
        /// </summary>
        /// <param name="nNumber">Number of retrieved messages.</param>
        void OnFinishRetrieve(int nNumber);

        /// <summary>
        ///   Called when plug-in setup is needed.
        /// </summary>
        /// <returns>if you have processed</returns>
        bool OnPlugInSetup(IntPtr hWnd);

        /// <summary>
        ///   Called when drag and drop operation occurs.
        /// </summary>
        /// <remarks>
        ///   Do not assume the default action (copy, move, etc.) is always
        ///   processed, because other plug-ins might cancel the operation.
        /// </remarks>
        /// <param name="lpTgt">A folder ID of the target folder.
        ///  You can assume it is a root mailbox, if the string
        ///  contains only one '\' character.</param>
        /// <param name="lpSrc">Either a folder ID or mail IDs. Multiple mail IDs are
        ///  separated by '\n' (0x0a).
        ///  You can assume it is a folder ID, if the string
        ///  doesn't contain '?' character.</param>
        /// <param name="nCount">Number of items to be dropped.
        ///   It can be more than one, if you drop mail items.</param>
        /// <param name="dropEffect">Type of drag and drop operation</param>
        /// <returns>If you want to cancel the default drag and drop action</returns>
        bool OnDragDrop(string lpTgt, string lpSrc, int nCount, BeckyDropEffect dropEffect);

        /// <summary>
        ///   Called when a message was retrieved and about to be filtered.
        ///   * BKC_OnBeforeFilter is obsolete. New programs should use BKC_OnBeforeFilter2 instead.
        /// </summary>
        /// <param name="lpMessage">A complete message source, which ends with
        /// "[CRLF].[CRLF]".</param>
        /// <param name="lpMailBox">ID of the mailbox that is retrieving the message. (XXXXXXXX.mb\)</param>
        /// <param name="action">Returns the filtering action to be applied.</param>
        /// <param name="actionParam">
        /// Returns the filtering parameter string.
        /// ACTION_MOVEFOLDER:	Folder name.
        ///                     e.g.XXXXXXXX.mb\FolderName\
        ///                     or \FolderName\ (begin with '\') to use
        ///                     the mailbox the message belongs.
        /// ACTION_COLORLABEL:	Color code(BGR) in hexadecimal.e.g. 0088FF
        ///ACTION_SETFLAG:		"F" to set flag. "R" to set read.
        ///ACTION_SOUND:		Name of the sound file.
        ///ACTION_RUNEXE:		Command line to execute. %1 will be replaced with the path to the file that contains the entire message.
        ///ACTION_REPLY:		Path to the template file without extension.
        ///                        e.g. #Reply\MyReply
        ///         ACTION_FORWARD:		Path to the template file without extension. + "*" + Address to forward.
        ///                        e.g. #Forward\MyForward*mail@address
        ///                                      * mail@address (no template)
        ///         ACTION_LEAVESERVER:	The string consists of one or two decimals.The second decimal is optional.
        ///                            Two decimals must be separated with a space.
        ///                            First decimal   1: Delete the message from the server.
        ///                                             0: Leave the message on the server.
        ///                            Second decimal  1: Do not store the message to the folder.
        ///                                             0: Store the message to the folder. (default action)
        ///                             e.g. 0 (Leave the message on the server.)
        ///                                  1 1 (Delete the message on the server and do not save. (Means KILL))
        ///         ACTION_ADDHEADER	"X-Header:data" that will be added at the top of the incoming message.
        ///                             You can specify multiple headers by separating CRLF, but each header must
        ///                             begin with "X-". e.g. "X-Plugindata1: test\r\nX-Plugindata2: test2";
        /// </param>
        /// <returns>Returns the filtering action to be applied.</returns>
        BeckyFilter OnBeforeFilter2(string lpMessage, string lpMailBox, out BeckyAction action, out string actionParam);

        /// <summary>
        ///   Should this plugin be disabled (i.e. because it has thrown an unhandled exception)
        ///   It will silently ignore all calls.
        /// </summary>
        bool IsDisabled { get; }

        /// <summary>
        ///   The exception e was through up to the Dll-Entry.
        ///   As this would kill Becky, stay alive... probably you should disable the plugin, though.
        /// </summary>
        void GotUnhandledException(Exception e, [CallerMemberName] string methodName = null);
    }
}