using System;
using System.Runtime.InteropServices;
using System.Text;
using BeckyApi.Enums;

// ReSharper disable InconsistentNaming

namespace BeckyApi
{
    /// <summary>
    ///   API of Becky (B2.exe)
    /// </summary>
    /// <remarks>
    ///   Why using PtrToStringAnsi? Answer: http://stackoverflow.com/questions/16715985/shared-memory-between-c-dll-and-c-sharp-code
    ///   Every PtrToStringAnsi needs to be PureApi.Free(d)
    /// </remarks>
    public class CallsIntoBecky
    {
        const int GetNextMail_BufferSize = 65535;
        const int GetCharSet_BufferSize = 1024;
        const int GetText_MimeType_BufferSize = 1024;
        const int GetSpecifiedHeader_BufferSize = 1024;
        const int CompGetSpecifiedHeader_BufferSize = 1024;
        const int CompGetCharSet_BufferSize = 1024;
        const int CompGetText_MimeType_BufferSize = 1024;
        const int MIMEHeader_BufferSize = 1024;


        private Version _beckyVersion; // = null;

        /// <summary>
        ///   Gets Becky!'s version.
        /// </summary>
        /// <returns>
        /// A string which represents the version of Becky!.
        /// The format is:
        /// X.YY.ZZ
        /// X represents major version.
        /// YY represents minor version
        /// ZZ represents revision number.
        /// e.g. 2.00.02
        /// </returns>
        public string GetVersion()
        {
            IntPtr strPtr = PureApi.GetVersion();
            var res = Marshal.PtrToStringAnsi(strPtr);
            PureApi.Free(strPtr);
            return res;
        }


        /// <summary>
        ///   Caching the version for being called very often as we have version-dependent methods.
        /// </summary>
        public Version GetVersionCached() {
            if (_beckyVersion == null) {
                string pure = GetVersion();

                _beckyVersion = Version.Parse(pure);
            }
            return _beckyVersion;
        }

        /// <summary>
        ///   Is the minVersion older than the actual Becky! version?
        /// </summary>
        public bool IsVersionHigherThan(Version minVersion) {
            Version actual = GetVersionCached();
            return actual.CompareTo(minVersion) >= 0;
        }

        /// <summary>
        /// Executes Becky!'s command.
        /// Valid command names are found in the "Shortcut Keys" tab of
        /// Becky!'s "General Settings".
        /// e.g. bka.Command(NULL, "SendReceive");
        /// </summary>
        /// <remarks>
        /// Not all functions can be called with this command. If you can not find
        /// functions you want to call, retrieve the command IDs directly from
        /// Becky!'s executable using SPY or a resource editor, and use
        /// PostMessage(WM_COMMAND, nID, 0)
        /// </remarks>
        /// <param name="hWnd">The target window handle on which the command will be executed. Or NULL for the main window.</param>
        /// <param name="lpCmd">Command Name</param>
        public void Command(IntPtr hWnd, string lpCmd)
        {
            PureApi.Command(hWnd, lpCmd);
        }

        public class WindowHandles
        {
            /// <summary>
            ///   A pointer to HWND which receives the window handle of the main frame.
            /// </summary>
            public IntPtr Main;

            /// <summary>
            ///   A pointer to HWND which receives the window handle of the TreeView window.
            /// </summary>
            public IntPtr Tree;

            /// <summary>
            ///   A pointer to HWND which receives the window handle of the ListView window.
            /// </summary>
            public IntPtr List;

            /// <summary>
            ///   A pointer to HWND which receives the window handle of the message viewer window.
            /// </summary>
            public IntPtr View;
        }

        /// <summary>
        ///   Gets window handles of particular windows belong to Becky!
        /// </summary>
        /// <returns>null (no success) or object with handles.</returns>
        public WindowHandles GetWindowHandles()
        {
            WindowHandles wh = new WindowHandles();
            bool done = PureApi.GetWindowHandles(ref wh.Main, ref wh.Tree, ref wh.List, ref wh.View);
            if (!done)
            {
                return null;
            }
            return wh;
        }

        /// <summary>
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="menuCommandId">Menu command ID</param>
        /// <param name="futureUse">Future use</param>
        public delegate void RegisterCommandCallback(IntPtr hWnd, short menuCommandId, short futureUse);

        private class RegisterCommandCallbackSplitter {
            public RegisterCommandCallbackSplitter(RegisterCommandCallback callback) {
                RealCallback = callback;
            }
            private RegisterCommandCallback RealCallback { get; }

            internal void ApiCallback(IntPtr hWnd, IntPtr lParam) {
                uint xy = unchecked(IntPtr.Size == 8 ? (uint) lParam.ToInt64() : (uint) lParam.ToInt32());
                short x = unchecked((short) xy);
                short y = unchecked((short) (xy >> 16));
                RealCallback(hWnd, x, y);
            }
        }
    

        /// <summary>
        ///   You can register a same callback function for different menu IDs. When
        ///   the callback is called, you can check lower word of lParam to
        ///   determine which menu ID is executed.
        /// 
        ///   See the description of BKC_OnMenuInit() for more details.
        /// </summary>
        /// <param name="lpszComment">A short description for the command.</param>
        /// <param name="nTarget">An identifier of the window on which the command is executed.</param>
        /// <param name="cmd">The callback function which will be called when the command is executed.</param>
        /// <returns>Menu command ID which you can use to add a command to the menu.</returns>
        public uint RegisterCommand(string lpszComment, BeckyMenu nTarget, RegisterCommandCallback cmd) {
            var delegatePromoter = new RegisterCommandCallbackSplitter(cmd);
            return PureApi.RegisterCommand(lpszComment, nTarget, delegatePromoter.ApiCallback);
        }


        /// <summary>
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="menuCommandId">Menu command ID.</param>
        /// <param name="futureUse"></param>
        /// <returns>Return value can be a combination of the BeckyCmdUI flags</returns>
        public delegate BeckyCmdUI RegisterUiCallbackCallback(IntPtr hWnd, short menuCommandId, short futureUse);

        private class RegisterUICallbackSplitter {
            public RegisterUICallbackSplitter(RegisterUiCallbackCallback callback) {
                RealCallback = callback;
            }
            private RegisterUiCallbackCallback RealCallback { get; }

            internal BeckyCmdUI ApiCallback(IntPtr hWnd, IntPtr lParam) {
                uint xy = unchecked(IntPtr.Size == 8 ? (uint)lParam.ToInt64() : (uint)lParam.ToInt32());
                short x = unchecked((short)xy);
                short y = unchecked((short)(xy >> 16));
                return RealCallback(hWnd, x, y);
            }
        }

        private const uint REGISTER_UI_CALLBACK_FAILURE = 0xffffffff;

        /// <summary>
        ///   You can register a same callback function for different menu IDs. When
        ///   the callback is called, you can check lower word of lParam to
        ///   determine which menu ID is executed.
        /// Example of CmdUIProc:
        /// <code>
        /// BeckyCmdUI CmdUIProc(IntPtr hWnd, IntPtr lParam) {
        ///     BeckyCmdUI nRetVal;
        ///     if (you want to disable the menu item) {
        ///         nRetVal |= BKMENU_CMDUI_DISABLED;
        ///     }
        ///     if (you want to check the menu item) {
        ///         nRetVal |= BKMENU_CMDUI_CHECKED;
        ///     }
        ///     return nRetVal
        /// }
        /// </code>
        /// </summary>
        /// <param name="nID">Menu command ID</param>
        /// <param name="cmd">The callback function which will be called when the command UI for the menu ID is being updated</param>
        /// <returns>Null or menu command ID which you passed.</returns>
        public uint? RegisterUICallback(uint nID, RegisterUiCallbackCallback cmd) {
            var delegatePromoter = new RegisterUICallbackSplitter(cmd);
            var res = PureApi.RegisterUICallback(nID, delegatePromoter.ApiCallback);
            if (res == REGISTER_UI_CALLBACK_FAILURE) {
                return null;
            }
            return res;
        }

        /// <summary>
        ///   Gets the path of the data folder Becky! is currently using.
        ///   Mailbox subfolders (XXXXXXXX.mb) are located under this folder.
        /// </summary>
        /// <returns>Full path name of Becky!'s Data folder</returns>
        public string GetDataFolder() {
            IntPtr strPtr = PureApi.GetDataFolder();
            var res = Marshal.PtrToStringAnsi(strPtr);
            PureApi.Free(strPtr);
            return res;
        }

        /// <summary>
        ///   Returns the temporary path which plug-ins can use.
        ///   It is strongly encouraged to use this folder if you want to create
        ///   temporary files.
        /// </summary>
        /// <remarks>
        ///   Becky! will clean them up automatically when the program quits.
        ///   (It is always better to clean them up by yourself, though.)
        /// </remarks>
        /// <returns>Full path name of the temporary folder Becky! is using.</returns>
        public string GetTempFolder() {
            IntPtr strPtr = PureApi.GetTempFolder();
            var res = Marshal.PtrToStringAnsi(strPtr);
            PureApi.Free(strPtr);
            return res;
        }

        /// <summary>
        ///   Gets an unique temporary file name under the temporary folder.
        ///   It returns only a name. Actual file is not created by this command.
        ///   <code>string lpName = bka.GetTempFileName("txt");</code>
        /// </summary>
        /// <param name="lpType">File type of the temporary file you want to create.</param>
        /// <returns>Full path name of the temporary file name.</returns>
        public string GetTempFileName(string lpType) {
            IntPtr strPtr = PureApi.GetTempFileName(lpType);
            var res = Marshal.PtrToStringAnsi(strPtr);
            PureApi.Free(strPtr);
            return res;
        }

        
    /// <summary>
    /// 
    /// </summary>
    /// <returns>Identifier of the current mail box. It is actually a sub folder name of the mailbox folder. (XXXXXXXX.mb\)</returns>
        public string GetCurrentMailBox() {
            IntPtr strPtr = PureApi.GetCurrentMailBox();
            var res = Marshal.PtrToStringAnsi(strPtr);
            PureApi.Free(strPtr);
            return res;
        }

        /// <summary>
        ///   Switches the current mailbox.
        /// </summary>
        /// <param name="lpMailBox">Identifier of the mailbox.</param>
        public void SetCurrentMailBox(string lpMailBox) {
            PureApi.SetCurrentMailBox(lpMailBox);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Identifier of the current mail folder. It is actually a part of the folder path. (XXXXXXXX.mb\!!!!Inbox\)</returns>
        public string GetCurrentFolder() {
            IntPtr strPtr = PureApi.GetCurrentFolder();
            var res = Marshal.PtrToStringAnsi(strPtr);
            PureApi.Free(strPtr);
            return res;
        }

        /// <summary>
        ///   Switches the current folder.
        /// </summary>
        /// <param name="lpFolderID">An ID of the folder you want to switch to.</param>
        public void SetCurrentFolder(string lpFolderID) {
            PureApi.SetCurrentFolder(lpFolderID);
        }

        /// <summary>
        ///  Convert a folder ID to its display name.
        ///  e.g.   XXXXXXXX.mb\!!!!Inbox\ --> [Mailbox1]Inbox
        /// </summary>
        /// <param name="lpFolderID">An ID of the folder you want to get the display name.</param>
        /// <returns>Display name of the folder.</returns>
        public string GetFolderDisplayName(string lpFolderID) {
            IntPtr strPtr = PureApi.GetFolderDisplayName(lpFolderID);
            var res = Marshal.PtrToStringAnsi(strPtr);
            PureApi.Free(strPtr);
            return res;
        }

        /// <summary>
        ///   Shows specified text on the status bar of the target window.
        /// </summary>
        /// <param name="hWnd">Window handle of the target composing window, or NULL for the main frame.</param>
        /// <param name="lpszMsg">A string you want to set on the status bar.</param>
        public void SetMessageText(IntPtr hWnd, string lpszMsg) {
            PureApi.SetMessageText(hWnd, lpszMsg);
        }


        /// <summary>
        ///   
        /// </summary>
        /// <remarks>
        ///   it is actually a combination of a folder ID, literal "?",
        ///   and an unique hexadecimal identifier.
        ///   (XXXXXXXX.mb\!!!!Inbox\?1234ABCD)
        ///   Returns empty string ("") if there is no current mail.
        /// </remarks>
        /// <returns>Mail identifier of the current mail.</returns>
        public string GetCurrentMail() {
            IntPtr strPtr = PureApi.GetCurrentMail();
            var res = Marshal.PtrToStringAnsi(strPtr);
            PureApi.Free(strPtr);
            return res;
        }

        /// <summary>
        ///   Switches the focused mail item.
        /// </summary>
        /// <param name="lpMailID">An ID of the mail you want to select.</param>
        public void SetCurrentMail(string lpMailID) {
            PureApi.SetCurrentMail(lpMailID);
        }


        /// <summary>
        /// <code>
        /// char szMailID[256];
        /// int n = -1;
        /// int nSelected = 0;
        /// do {
        ///     n = bka.GetNextMail(n, szMailID, 256, TRUE);
        ///     nSelected++;
        /// } while (n != -1)
        /// nSelected--;
        /// TRACE("%d items are selected", nSelected);
        /// </code>
        /// </summary>
        /// <param name="nStart">Start position to find the next mail item. Set -1 for the first call.</param>
        /// <param name="lpszMailID">[out] A buffer which receives mail identifier of found mail.</param>
        /// <param name="bSelected">Set TRUE if you want to get only selected items.</param>
        /// <param name="mailSize">How many characters should be get of the mail?</param>
        /// <returns>A position of the item which is found. You can use it for the next call. Returns -1 if no more items are found.</returns>
        public int GetNextMail(int nStart, out string lpszMailID, bool bSelected, int mailSize = GetNextMail_BufferSize) {
            StringBuilder sb = new StringBuilder(mailSize);
            var result = PureApi.GetNextMail(nStart, sb, sb.Capacity, bSelected);
            lpszMailID = sb.ToString();
            return result;
        }

        /// <summary>
        ///   Selects or Deselects the specified mail item.
        /// </summary>
        /// <param name="lpMailID">An ID of the mail item you want to select/deselect.</param>
        /// <param name="bSel">TRUE to select the item. FALSE to deselect the item.</param>
        public void SetSel(string lpMailID, bool bSel) {
            PureApi.SetSel(lpMailID, bSel);
        }

        /// <summary>
        /// <code>
        /// string lpFolderID = bka.GetCurrentFolder();
        /// bka.AppendMessage(lpFolderID, "From: me\r\nTo: you\r\n\r\nThis is test.\r\n");
        /// </code>
        /// </summary>
        /// <param name="lpFolderID">An ID of the folder you want to append a message.</param>
        /// <param name="lpszData">Message text you want to append. It must take the form of RFC822 message.</param>
        /// <returns>Success/Failure.</returns>
        public bool AppendMessage(string lpFolderID, string lpszData) {
            return PureApi.AppendMessage(lpFolderID, lpszData);
        }


        /// <summary>
        ///  Move specified mail items to a specified folder.
        ///  You can specify multiple mail IDs by separating with "|" character.
        ///  e.g.
        /// <code>
        ///  strcat_s(lpBuf, nBufSiz, szMailID1);
        ///  strcat_s(lpBuf, nBufSiz, "|");
        ///  strcat_s(lpBuf, nBufSiz, szMailID2);
        ///  strcat_s(lpBuf, nBufSiz, "|");
        ///  ...
        /// </code>
        /// </summary>
        /// <remarks>
        ///   Becky! Ver.2.50 or newer.
        /// </remarks>
        /// <param name="lpFolderID">An ID of the folder you want to move items to.</param>
        /// <param name="lpMailIDSet">A collection of multiple mail IDs separated by "|".</param>
        /// <param name="bCopy">Set TRUE if you want to copy items instead of moving.</param>
        /// <returns>The number of messages that has been successfully moved or copied.</returns>
        public int MoveMessages(string lpFolderID, string lpMailIDSet, bool bCopy) {
            if (!IsVersionHigherThan(new Version(2, 50, 0))) throw new InvalidOperationException();
            return PureApi.MoveMessages(lpFolderID, lpMailIDSet, bCopy);
        }

        /// <summary>
        /// Move the selected mail items to a specified folder.
        /// </summary>
        /// <param name="lpFolderID">An ID of the folder you want to move items to.</param>
        /// <param name="bCopy">Set TRUE if you want to copy items instead of moving.</param>
        /// <returns>TRUE:  Success; FALSE: Failure</returns>
        public bool MoveSelectedMessages(string lpFolderID, bool bCopy) {
            return PureApi.MoveSelectedMessages(lpFolderID, bCopy);
        }

        /// <summary>
        /// <code>
        /// DWORD dwFlag = bka.GetStatus(lpMailID);
        /// if (dwFlag & MESSAGE_READ) {
        ///     TRACE("This message is read");
        /// }
        /// </code>
        /// </summary>
        /// <param name="lpMailID">An ID of the mail item you want to get the status flag.</param>
        /// <returns>A status flag of the mail item. It can be a combination of the following bit flag.</returns>
        public BeckyMessage GetStatus(string lpMailID) {
            return PureApi.GetStatus(lpMailID);
        }

        /// <summary>
        ///   You can modify only the following bits.
        ///   MESSAGE_READ, MESSAGE_FORWARDED, MESSAGE_REPLIED
        ///   Other bits are simply ignored.
        /// </summary>
        /// <remarks>
        ///   Becky! Ver.2.00.06 or higher.
        /// </remarks>
        /// <param name="lpMailID">An ID of the mail item you want to get the status flag.</param>
        /// <param name="dwSet">Status flags to be set.</param>
        /// <param name="dwReset">Status flags to be cleared.</param>
        /// <returns>A new status flag of the mail item.</returns>
        public BeckyMessage SetStatus(string lpMailID, BeckyMessage dwSet, BeckyMessage dwReset) {
            if (!IsVersionHigherThan(new Version(2, 0, 6))) throw new InvalidOperationException();
            return PureApi.SetStatus(lpMailID, dwSet, dwReset);
        }

        /// <summary>
        /// You can use the "mailto" URL scheme defined in RFC2368.
        /// 
        /// You can use the special "x-becky-mailto" URL scheme for
        /// the following special purpose headers with Becky! 2.
        /// This has once been changed for security reasons.
        /// 
        /// X-Becky-Attachment: full path name of the file you want to attach.
        /// X-Becky-Template: full path name of the template file you want to apply.
        /// 
        /// <example>bka.ComposeMail("x-becky-mailto:mail@address?X-Becky-Attachment=C:\test.txt");</example>
        /// 
        /// You should URL-encode the data if it contains one of the following characters.
        /// %,?,&amp;,=,~, and less or equal than 0x20
        /// <example>bka.ComposeMail("x-becky-mailto:Name%20&lt;mail@address>");</example>
        /// </summary>
        /// 
        /// <param name="lpURL">"mailto:" URL string to be passed to an outgoing message.
        ///   <example>e.g. "mailto:mail@address"</example>
        ///   <example>e.g. "x-becky-mailto:mail@address"</example>
        /// </param>
        /// <returns>Window handle of the composing window.</returns>
        public IntPtr ComposeMail(string lpURL) {
            return PureApi.ComposeMail(lpURL);
        }

        /// <summary>
        ///   Obtains charset information of the specified mail item.
        /// </summary>
        /// <param name="lpMailID">An ID of the mail item. Or NULL for the current mail.</param>
        /// <param name="lpszCharSet">[out] A charset name (max: 1024 chars)</param>
        /// <returns>ANSI code page.</returns>
        public int GetCharSet(string lpMailID, out string lpszCharSet) {
            StringBuilder sb = new StringBuilder(GetCharSet_BufferSize);
            var result = PureApi.GetCharSet(lpMailID, sb, sb.Capacity);
            lpszCharSet = sb.ToString();
            return result;
        }

        /// <summary>
        /// Gets a complete source of the specified mail item. It is not decoded.
        /// </summary>
        /// <param name="lpMailID">
        /// An ID of the mail item. Or NULL for the current mail.
        /// If the current mail is displaying a temporary
        /// content that was set by SetSource("TEMP", ...) API,
        /// GetSource(NULL) returns the source of the temporary
        /// content and GetSource(MailID) returns the source of
        /// the original content.
        /// </param>
        /// <returns>A source of the mail item. Returns NULL if mail item is not found.</returns>
        public string GetSource(string lpMailID) {
            IntPtr strPtr = PureApi.GetSource(lpMailID);
            var res = Marshal.PtrToStringAnsi(strPtr);
            PureApi.Free(strPtr);
            return res;
        }

        /// <summary>
        /// Apply a mail source to the specified mail item. Existing data will 
        /// be completely overwritten by this command.
        /// </summary>
        /// <param name="lpMailID">
        /// An ID of the mail item. Or NULL for the current mail. 
        /// Specifying "TEMP", the source will be set to the
        /// current mail item temporarily. The change will be
        /// discarded when the user select other mail items.
        /// </param>
        /// <param name="lpSource">A mail source to be set</param>
        public void SetSource(string lpMailID, string lpSource) {
            PureApi.SetSource(lpMailID, lpSource);
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        ///   Becky! Ver.2.43.00 or higher.
        /// </remarks>
        /// <param name="lpMailID"> An ID of the mail item. Or NULL for the current mail.</param>
        /// <returns>
        ///   The size of the specified message in bytes. 
        ///   If lpMailID is incorrect or size could not be retrieved somehow, it will return -1;
        /// </returns>
        public uint GetSize(string lpMailID) {
            if (!IsVersionHigherThan(new Version(2, 43, 0))) throw new InvalidOperationException();
            return PureApi.GetSize(lpMailID);
        }

        /// <summary>
        ///  Gets a header part of the specified mail item. It is decoded. 
        /// </summary>
        /// <param name="lpMailID">An ID of the mail item. Or NULL for the current mail.</param>
        /// <returns>A header part of the mail item.</returns>
        public string GetHeader(string lpMailID) {
            IntPtr strPtr = PureApi.GetHeader(lpMailID);
            var res = Marshal.PtrToStringAnsi(strPtr);
            PureApi.Free(strPtr);
            return res;
        }

        /// <summary>
        /// Gets the text part of the current mail item. It is decoded.
        /// Unlike above functions, it works only the current mail.
        /// </summary>
        /// <param name="lpszMimeType">
        /// [out] A buffer to receive the MIME type of the text.
        /// It is usually "text/plain" but it could be "text/html" or others sometimes.
        /// (max: 1024 chars)
        /// </param>
        /// <returns>Text of the current mail item.</returns>
        public string GetText(out string lpszMimeType) {
            StringBuilder sb = new StringBuilder(GetText_MimeType_BufferSize);
            IntPtr strPtr = PureApi.GetText(sb, sb.Capacity);
            var res = Marshal.PtrToStringAnsi(strPtr);
            PureApi.Free(strPtr);

            lpszMimeType = sb.ToString();
            return res;
        }

        

        /// <summary>
        /// Sets text to the current mail item.
        /// </summary>
        /// <param name="nMode">Where to put the text (temporarily)</param>
        /// <param name="lpText">Text to be set.</param>
        public void SetText(TextMode nMode, string lpText) {
            PureApi.SetText(nMode, lpText);
        }

        /// <summary>
        /// Retrieve the data of the specified header field from the current
        /// mail item. It could be a large string separated by CRLF, so better
        /// prepare a sufficient buffer for particular headers like "To", "Cc", etc.
        /// <code>
        /// char szData[1024];
        /// bka.GetSpecifiedHeader("From", szData, 1024);
        /// TRACE("This mail is sent from %s", szData);
        /// </code>
        /// </summary>
        /// <param name="lpHeader">A header field to retrieve. e.g. "From", "Subject"</param>
        /// <returns>Receiving data (max 1024 chars).</returns>
        public string GetSpecifiedHeader(string lpHeader) {
            StringBuilder sb = new StringBuilder(GetSpecifiedHeader_BufferSize);
            PureApi.GetSpecifiedHeader(lpHeader, sb, sb.Capacity);
            return sb.ToString();
        }

        /// <summary>
        /// Set the data to the specified header.
        /// </summary>
        /// <param name="lpHeader">A header field which data will be set to.</param>
        /// <param name="lpszData">Data to be set.</param>
        public void SetSpecifiedHeader(string lpHeader, string lpszData) {
            PureApi.SetSpecifiedHeader(lpHeader, lpszData);
        }

        /// <summary>
        /// Obtains charset information of the outgoing message.
        /// </summary>
        /// <param name="hWnd">A window handle of the composing window.</param>
        /// <param name="lpszData">[out] A charset name (max 1024 chars)</param>
        /// <returns>ANSI code page.</returns>
        public int CompGetCharSet(IntPtr hWnd, out string lpszData) {
            StringBuilder sb = new StringBuilder(CompGetCharSet_BufferSize);
            var result = PureApi.CompGetCharSet(hWnd, sb, sb.Capacity);
            lpszData = sb.ToString();
            return result;
        }

        /// <summary>
        ///   Gets a complete source of the outgoing message.
        /// </summary>
        /// <param name="hWnd">A window handle of the composing window.</param>
        /// <returns>A source of the outgoing message.</returns>
        public string CompGetSource(IntPtr hWnd) {
            IntPtr strPtr = PureApi.CompGetSource(hWnd);
            var res = Marshal.PtrToStringAnsi(strPtr);
            PureApi.Free(strPtr);
            return res;
        }

        /// <summary>
        /// Apply a mail source to the outgoing message. Existing data will 
        /// be completely overwritten by this command.
        /// </summary>
        /// <param name="hWnd">A window handle of the composing window.</param>
        /// <param name="lpSource">A mail source to be set</param>
        public void CompSetSource(IntPtr hWnd, string lpSource) {
            PureApi.CompSetSource(hWnd, lpSource);
        }

        /// <summary>
        /// Gets a header part of the outgoing message.
        /// </summary>
        /// <param name="hWnd">A window handle of the composing window.</param>
        /// <returns> A header part of the outgoing message.</returns>
        public string CompGetHeader(IntPtr hWnd) {
            IntPtr strPtr = PureApi.CompGetHeader(hWnd);
            var res = Marshal.PtrToStringAnsi(strPtr);
            PureApi.Free(strPtr);
            return res;
        }

        /// <summary>
        ///  Retrieve the data of the specified header field from the outgoing
        ///  message. It could be a large string separated by CRLF, so better
        ///  prepare a sufficient buffer for particular headers like "To", "Cc", etc.
        ///  Usually, you cant retrieve "X-Becky-" headers, which Becky! uses
        ///  internally. One exception is "X-Becky-Ref", which contains the
        ///  information about the reference mail for the replying/forwarding
        ///  message. It is similar to Mail ID, but ">" character is used for
        ///  the separator instead of "?".
        ///  e.g. X-Becky-Ref: XXXXXXXX.mb\!!!!Inbox\>1234ABCD
        /// </summary>
        /// <param name="hWnd"> A window handle of the composing window.</param>
        /// <param name="lpHeader">A header field to retrieve. e.g. "From", "Subject"</param>
        /// <returns>The data (max 1024 chars)</returns>
        public string CompGetSpecifiedHeader(IntPtr hWnd, string lpHeader) {
            StringBuilder sb = new StringBuilder(CompGetSpecifiedHeader_BufferSize);
            PureApi.CompGetSpecifiedHeader(hWnd, lpHeader, sb, sb.Capacity);
            return sb.ToString();
        }

        /// <summary>
        /// Set the data to the specified header.
        /// </summary>
        /// <param name="hWnd"> A window handle of the composing window.</param>
        /// <param name="lpHeader">A header field which data will be set to.</param>
        /// <param name="lpszData">Data to be set.</param>
        public void CompSetSpecifiedHeader(IntPtr hWnd, string lpHeader, string lpszData) {
            PureApi.CompSetSpecifiedHeader(hWnd, lpHeader, lpszData);
        }

        /// <summary>
        /// Gets the text part of the outgoing message.
        /// </summary>
        /// <param name="hWnd">A window handle of the composing window.</param>
        /// <param name="lpszMimeType">
        /// [out] Receive the MIME type of the text.
        /// It is usually "text/plain" but it could be
        /// "text/html" or others sometimes. (max. 1024 chars)
        /// </param>
        /// <returns>Text of the outgoing message.</returns>
        public string CompGetText(IntPtr hWnd, out string lpszMimeType) {
            StringBuilder sb = new StringBuilder(CompGetText_MimeType_BufferSize);
            IntPtr strPtr = PureApi.CompGetText(hWnd, sb, sb.Capacity);
            var res = Marshal.PtrToStringAnsi(strPtr);
            PureApi.Free(strPtr);

            lpszMimeType = sb.ToString();
            return res;
        }

        /// <summary>
        ///   Sets text to the outgoing message.
        /// </summary>
        /// <param name="hWnd">A window handle of the composing window.</param>
        /// <param name="nMode">Where to put the text (temporarily)</param>
        /// <param name="lpText">Text to be set.</param>
        public void CompSetText(IntPtr hWnd, TextMode nMode, string lpText) {
            PureApi.CompSetText(hWnd, nMode, lpText);
        }

        /// <summary>
        /// Attaches a specified file to the outgoing message.
        /// </summary>
        /// <param name="hWnd">A window handle of the composing window.</param>
        /// <param name="lpAttachFile">A full path name of the file to be attached.</param>
        /// <param name="lpMimeType">MIME type of the file. It can be NULL.</param>
        public void CompAttachFile(IntPtr hWnd, string lpAttachFile, string lpMimeType) {
            PureApi.CompAttachFile(hWnd, lpAttachFile, lpMimeType);
        }

        /* This is not needed for C# API, as it is garbage collected anyway
        /// <summary>
        /// Allocates a memory block.
        /// It is strongly encouraged to use this function to allocate temporary
        /// buffer instead of using malloc() C library function. Allocated
        /// memory block will be garbaged by Becky! on exiting a callback even
        /// if you forget to call CBeckyAPI::Free().
        /// 
        /// <code>
        /// int WINAPI BKC_OnEveryMinute() {
        ///     LPSTR lpBuf = bka.Alloc(256);
        ///     ...
        ///     // bka.Free(lpBuf); // You can call this, but you don't have to.
        ///     return 0;
        /// }
        /// </code>
        /// </summary>
        /// <param name="dwSize">Size to allocate</param>
        /// <returns>Pointer to the allocated buffer.</returns>
        public IntPtr Alloc(uint dwSize) {
            return PureApi.Alloc(dwSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpVoid">The pointer to the allocated buffer.</param>
        /// <param name="dwSize">Size to reallocate</param>
        /// <returns>Pointer to the reallocated buffer.</returns>
        public IntPtr ReAlloc(IntPtr lpVoid, uint dwSize) {
            return PureApi.ReAlloc(lpVoid, dwSize);
        }

        /// <summary>
        /// Frees a memory block allocated by CBeckyAPI::Alloc() command.
        /// </summary>
        /// <param name="lpVoid">The pointer to the buffer.</param>
        public void Free(IntPtr lpVoid) {
            PureApi.Free(lpVoid);
        }*/


        /// <summary>
        /// Encodes Shift_JIS to ISO-2022-JP, or vice versa.
        /// </summary>
        /// <param name="lpSrc">A source string.</param>
        /// <param name="bEncode">TRUE:  Encode, FALSE: Decode</param>
        /// <returns>A converted string.</returns>
        public string ISO_2022_JP(string lpSrc, bool bEncode) {
            IntPtr strPtr = PureApi.ISO_2022_JP(lpSrc, bEncode);
            var res = Marshal.PtrToStringAnsi(strPtr);
            PureApi.Free(strPtr);
            return res;
        }

        /// <summary>
        /// Encodes EUC-KR to ISO-2022-KR, or vice versa.
        /// </summary>
        /// <param name="lpSrc">A source string.</param>
        /// <param name="bEncode">TRUE:  Encode, FALSE: Decode</param>
        /// <returns>A converted string.</returns>
        public string ISO_2022_KR(string lpSrc, bool bEncode) {
            IntPtr strPtr = PureApi.ISO_2022_KR(lpSrc, bEncode);
            var res = Marshal.PtrToStringAnsi(strPtr);
            PureApi.Free(strPtr);
            return res;
        }

        /// <summary>
        /// Encodes GB2312 to HZ-GB-2312, or vice versa.
        /// </summary>
        /// <param name="lpSrc">A source string.</param>
        /// <param name="bEncode">TRUE:  Encode, FALSE: Decode</param>
        /// <returns>A converted string.</returns>
        public string HZ_GB2312(string lpSrc,bool bEncode) {
            IntPtr strPtr = PureApi.HZ_GB2312(lpSrc, bEncode);
            var res = Marshal.PtrToStringAnsi(strPtr);
            PureApi.Free(strPtr);
            return res;
        }

        /// <summary>
        /// Encodes CP1250 to ISO-8859-2, or vice versa.
        /// </summary>
        /// <param name="lpSrc">A source string.</param>
        /// <param name="bEncode">TRUE:  Encode, FALSE: Decode</param>
        /// <returns>A converted string.</returns>
        public string ISO_8859_2(string lpSrc,bool bEncode) {
            IntPtr strPtr = PureApi.ISO_8859_2(lpSrc, bEncode);
            var res = Marshal.PtrToStringAnsi(strPtr);
            PureApi.Free(strPtr);
            return res;
        }

        /// <summary>
        /// Encodes Shift_JIS to EUC-JP, or vice versa.
        /// </summary>
        /// <param name="lpSrc">A source string.</param>
        /// <param name="bEncode">TRUE:  Encode, FALSE: Decode</param>
        /// <returns>A converted string.</returns>
        public string EUC_JP(string lpSrc,bool bEncode) {
            IntPtr strPtr = PureApi.EUC_JP(lpSrc, bEncode);
            var res = Marshal.PtrToStringAnsi(strPtr);
            PureApi.Free(strPtr);
            return res;
        }

        /// <summary>
        /// Encodes MBCS to UTF-7, or vice versa.
        /// </summary>
        /// <param name="lpSrc">A source string.</param>
        /// <param name="bEncode">TRUE:  Encode, FALSE: Decode</param>
        /// <returns>A converted string.</returns>
        public string UTF_7(string lpSrc,bool bEncode) {
            IntPtr strPtr = PureApi.UTF_7(lpSrc, bEncode);
            var res = Marshal.PtrToStringAnsi(strPtr);
            PureApi.Free(strPtr);
            return res;
        }

        /// <summary>
        /// Encodes MBCS to UTF-8, or vice versa.
        /// </summary>
        /// <param name="lpSrc">A source string.</param>
        /// <param name="bEncode">TRUE:  Encode, FALSE: Decode</param>
        /// <returns>A converted string.</returns>
        public string UTF_8(string lpSrc,bool bEncode) {
            IntPtr strPtr = PureApi.UTF_8(lpSrc, bEncode);
            var res = Marshal.PtrToStringAnsi(strPtr);
            PureApi.Free(strPtr);
            return res;
        }

        /// <summary>
        /// Encodes binary file to Base64 text file, or vice versa.
        /// </summary>
        /// <param name="lpszOutFile">A full path name of the output file.</param>
        /// <param name="lpszInFile">A full path name of the input file.</param>
        /// <param name="bEncode">TRUE:  Encode, FALSE: Decode</param>
        /// <returns>TRUE:  Success, FALSE: Failure</returns>
        public bool B64Convert(string lpszOutFile, string lpszInFile, bool bEncode) {
            return PureApi.B64Convert(lpszOutFile, lpszInFile, bEncode);
        }

        /// <summary>
        /// Encodes binary file to Quoted-Printable text file, or vice versa.
        /// </summary>
        /// <param name="lpszOutFile">A full path name of the output file.</param>
        /// <param name="lpszInFile">A full path name of the input file.</param>
        /// <param name="bEncode">TRUE:  Encode, FALSE: Decode</param>
        /// <returns>TRUE:  Success, FALSE: Failure</returns>
        public bool QPConvert(string lpszOutFile, string lpszInFile, bool bEncode) {
            return PureApi.QPConvert(lpszOutFile, lpszInFile, bEncode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpszIn">A source string.</param>
        /// <param name="lpszCharSet">The charset to be used for encoding.</param>
        /// <returns>A converted string.</returns>
        /// <seealso cref="DecodeMIMEHeader"/>
        public string EncodeMIMEHeader(string lpszIn, string lpszCharSet) {
            IntPtr strPtr = PureApi.MIMEHeader(lpszIn, lpszCharSet, lpszCharSet.Length);
            var res = Marshal.PtrToStringAnsi(strPtr);
            PureApi.Free(strPtr);
            return res;
        }

        /// <summary>
        /// Processes MIME header decoding.
        /// </summary>
        /// <param name="lpszIn">A source string.</param>
        /// <param name="lpszCharSet">[out]The name of charset (max 1024 chars).</param>
        /// <returns>A converted string.</returns>
        /// <seealso cref="EncodeMIMEHeader"/>
        public string DecodeMIMEHeader(string lpszIn, out string lpszCharSet) {
            StringBuilder sb = new StringBuilder(MIMEHeader_BufferSize);
            IntPtr strPtr = PureApi.MIMEHeader(lpszIn, sb, sb.Capacity);
            var res = Marshal.PtrToStringAnsi(strPtr);
            PureApi.Free(strPtr);

            lpszCharSet = sb.ToString();
            return res;
        }

        /// <summary>
        /// Format e-mail addresses expanding address groups.
        /// A returned pointer will be garbaged upon exiting a callback,
        /// or you can explicitly discard it using CBeckyAPI::Free().
        /// 
        /// e.g.
        /// <code>
        /// LPSTR lpAddr = bka.SerializeRcpt("mail@address, @\"Group1\"");
        /// -->
        /// mail @address,
        /// member1 @of.group1
        /// member2 @of.group1
        /// member3 @of.group1
        /// member4 @of.group1
        /// </code>
        /// </summary>
        /// <param name="lpAddresses">E-mail addresses delimitered by ",".</param>
        /// <returns>Formatted E-mail addresses delimitered by ",\r\n "</returns>
        public string SerializeRcpts(string lpAddresses) {
            IntPtr strPtr = PureApi.SerializeRcpts(lpAddresses);
            var res = Marshal.PtrToStringAnsi(strPtr);
            PureApi.Free(strPtr);
            return res;
        }

        /// <summary>
        /// Connects to the Internet using current mailbox's setting.
        /// If an user configured Becky! to use dialup, this function will work as an
        /// auto dialer.
        /// If an user is using LAN connection, this function does nothing.
        /// Note that "Disconnect()" doesn't always hang up the phone.
        /// Whether it actually hangs up or not depends on the user's configuration
        /// and the current situation 
        /// 
        /// e.g.
        /// <code>
        ///   Connect();
        ///   // Do any online operation.
        ///   Disconnect();
        /// </code>
        /// </summary>
        /// <seealso cref="Disconnect"/>
        /// <returns>TRUE: Success, FALSE: Failure</returns>
        public bool Connect() {
            return PureApi.Connect(true);
        }

        /// <summary>
        /// Disconnects from the Internet using current mailbox's setting.
        /// If an user is using LAN connection, this function does nothing.
        /// Note that "Disconnect()" doesn't always hang up the phone.
        /// Whether it actually hangs up or not depends on the user's configuration
        /// and the current situation 
        /// 
        /// e.g.
        /// <code>
        ///   Connect();
        ///   // Do any online operation.
        ///   Disconnect();
        /// </code>
        /// </summary>
        /// <seealso cref="Connect"/>
        /// <returns>TRUE: Success, FALSE: Failure</returns>
        public bool Disconnect() {
            return PureApi.Connect(false);
        }

        /// <summary>
        /// This function act like when you hit SPACE key to browse unread messages.
        /// </summary>
        /// <remarks>
        ///   Becky! Ver.2.05 or newer.
        /// </remarks>
        /// <param name="bBackScroll">
        ///   If TRUE, the current message will backscroll.
        ///   This is the same behavior when you hit SPACE key with Shift key.
        /// </param>
        /// <param name="bGoNext">
        ///   If TRUE, it will set read flag on the current message
        ///   and go to a next unread message immediately.
        ///   This is the same behavior when you hit SPACE key with Ctrl key.
        /// </param>
        /// <returns>Always returns TRUE.</returns>
        public bool NextUnread(bool bBackScroll, bool bGoNext) {
            if (!IsVersionHigherThan(new Version(2, 05, 0))) throw new InvalidOperationException();
            return PureApi.NextUnread(bBackScroll, bGoNext);
        }

        /// <summary>
        ///   Processes certain filtering action on a specified message.
        /// </summary>
        /// <remarks>
        ///   Becky! Ver.2.40.00 or higher.
        /// </remarks>
        /// <param name="lpMailID">An ID of the mail item.</param>
        /// <param name="nAction">Action to do</param>
        /// <param name="lpParam">
        /// Parameter meaning depends on the action.
        /// <para>
        ///   ACTION_COLORLABEL:
        ///   Color code(BGR) in hexadecimal. e.g. 0088FF
        /// </para>
        /// <para>
        ///   ACTION_SETFLAG:
        ///   "F" to set flag. "R" to set read.
        /// </para>
        /// <para>
        ///   ACTION_SOUND:
        ///   Name of the sound file.
        /// </para>
        /// <para>
        ///   ACTION_RUNEXE:
        ///   Command line to execute. %1 will be replaced with the path to the file that
        ///   contains the entire message. Note that Becky! freezes until this process is terminated.
        /// </para>
        /// <para>
        ///   ACTION_REPLY: 
        ///   Path to the template file without extension.
        ///   e.g. #Reply\MyReply
        /// </para>
        /// <para>
        ///   ACTION_FORWARD:
        ///   Path to the template file without extension. + "*" + Address to forward.
        ///   e.g. #Forward\MyForward*mail@address
        ///   *mail @address (no template)
        /// </para>
        /// </param>
        public void ProcessMail(string lpMailID, BeckyAction nAction, string lpParam) {
            if (!IsVersionHigherThan(new Version(2, 40, 0))) throw new InvalidOperationException();
            PureApi.ProcessMail(lpMailID, nAction, lpParam);
        }




        public static class PureApi
        {
            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_GetVersion", CharSet = CharSet.Auto)]
            public static extern IntPtr GetVersion();

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_Command")]
            public static extern void Command(
                IntPtr hWnd, 
                [MarshalAs(UnmanagedType.LPStr)] string lpCmd);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_GetWindowHandles")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GetWindowHandles(
                ref IntPtr lphMain, 
                ref IntPtr lphTree, 
                ref IntPtr lphList, 
                ref IntPtr lphView);


            /// <summary>
            /// </summary>
            /// <param name="hWnd"></param>
            /// <param name="lParam">
            ///   Lower word of lParam is a menu command ID.
            ///   Higher word of lParam is reserved for future use.
            /// </param>
            public delegate void RegisterCommandCallbackApi(IntPtr hWnd, IntPtr lParam);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_RegisterCommand")]
            public static extern uint RegisterCommand(
                [MarshalAs(UnmanagedType.LPStr)] string lpszComment, 
                BeckyMenu nTarget,
                [MarshalAs(UnmanagedType.FunctionPtr)] RegisterCommandCallbackApi cmd);

            /// <summary>
            /// </summary>
            /// <param name="hWnd"></param>
            /// <param name="lParam">
            ///   Lower word of lParam is a menu command ID.
            ///   Higher word of lParam is reserved for future use.
            /// </param>
            /// <returns>Return value can be a combination of the BeckyCmdUI flags</returns>
            public delegate BeckyCmdUI RegisterUiCallbackCallbackApi(IntPtr hWnd, IntPtr lParam);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_RegisterUICallback")]
            public static extern uint RegisterUICallback(
                uint nID,
                [MarshalAs(UnmanagedType.FunctionPtr)] RegisterUiCallbackCallbackApi cmd);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_GetDataFolder")]
            public static extern IntPtr GetDataFolder();

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_GetTempFolder")]
            public static extern IntPtr GetTempFolder();

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_GetTempFileName")]
            public static extern IntPtr GetTempFileName(
                [MarshalAs(UnmanagedType.LPStr)] string lpType);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_GetCurrentMailBox")]
            public static extern IntPtr GetCurrentMailBox();

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_SetCurrentMailBox")]
            public static extern void SetCurrentMailBox(
                [MarshalAs(UnmanagedType.LPStr)] string lpMailBox);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_GetCurrentFolder")]
            public static extern IntPtr GetCurrentFolder();

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_SetCurrentFolder")]
            public static extern void SetCurrentFolder(
                [MarshalAs(UnmanagedType.LPStr)] string lpFolderID);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_GetFolderDisplayName")]
            public static extern IntPtr GetFolderDisplayName(
                [MarshalAs(UnmanagedType.LPStr)] string lpFolderID);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_SetMessageText")]
            public static extern void SetMessageText(
                IntPtr hWnd, 
                [MarshalAs(UnmanagedType.LPStr)] string lpszMsg);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_GetCurrentMail")]
            public static extern IntPtr GetCurrentMail();

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_SetCurrentMail")]
            public static extern void SetCurrentMail(
                [MarshalAs(UnmanagedType.LPStr)] string lpMailID);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_GetNextMail")]
            public static extern int GetNextMail(
                int nStart, 
                [Out, MarshalAs(UnmanagedType.LPStr)] StringBuilder lpszMailID,
                int nBuf, 
                [MarshalAs(UnmanagedType.Bool)] bool bSelected);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_SetSel")]
            public static extern void SetSel(
                [MarshalAs(UnmanagedType.LPStr)] string lpMailID, 
                [MarshalAs(UnmanagedType.Bool)] bool bSel);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_AppendMessage")]
            [return:MarshalAs(UnmanagedType.Bool)]
            public static extern bool AppendMessage(
                [MarshalAs(UnmanagedType.LPStr)] string lpFolderID, 
                [MarshalAs(UnmanagedType.LPStr)] string lpszData);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_MoveMessages")]
            public static extern int MoveMessages(
                [MarshalAs(UnmanagedType.LPStr)] string lpFolderID,
                [MarshalAs(UnmanagedType.LPStr)] string lpMailIDSet,
                [MarshalAs(UnmanagedType.Bool)] bool bCopy);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_MoveSelectedMessages")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool MoveSelectedMessages(
                [MarshalAs(UnmanagedType.LPStr)] string lpFolderID,
                [MarshalAs(UnmanagedType.Bool)]bool bCopy);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_GetStatus")]
            public static extern BeckyMessage GetStatus(
                [MarshalAs(UnmanagedType.LPStr)] string lpMailID);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_SetStatus")]
            public static extern BeckyMessage SetStatus(
                [MarshalAs(UnmanagedType.LPStr)] string lpMailID,
                BeckyMessage dwSet,
                BeckyMessage dwReset);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_ComposeMail")]
            public static extern IntPtr ComposeMail(
                [MarshalAs(UnmanagedType.LPStr)] string lpURL);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_GetCharSet")]
            public static extern int GetCharSet(
                [MarshalAs(UnmanagedType.LPStr)] string lpMailID,
                [Out, MarshalAs(UnmanagedType.LPStr)] StringBuilder lpszCharSet,
                int nBuf);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_GetSource")]
            public static extern IntPtr GetSource(
                [MarshalAs(UnmanagedType.LPStr)] string lpMailID);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_SetSource")]
            public static extern void SetSource(
                [MarshalAs(UnmanagedType.LPStr)] string lpMailID, 
                [MarshalAs(UnmanagedType.LPStr)] string lpSource);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_GetSize")]
            public static extern uint GetSize(
                [MarshalAs(UnmanagedType.LPStr)] string lpMailID);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_GetHeader")]
            public static extern IntPtr GetHeader(
                [MarshalAs(UnmanagedType.LPStr)] string lpMailID);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_GetText")]
            public static extern IntPtr GetText(
                [MarshalAs(UnmanagedType.LPStr)] StringBuilder lpszMimeType,
                int nBuf);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_SetText")]
            public static extern void SetText(
                TextMode nMode,
                [MarshalAs(UnmanagedType.LPStr)] string lpText);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_GetSpecifiedHeader")]
            public static extern void GetSpecifiedHeader(
                [MarshalAs(UnmanagedType.LPStr)] string lpHeader,
                [Out, MarshalAs(UnmanagedType.LPStr)] StringBuilder lpszData,
                int nBuf);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_SetSpecifiedHeader")]
            public static extern void SetSpecifiedHeader(
                [MarshalAs(UnmanagedType.LPStr)] string lpHeader,
                [MarshalAs(UnmanagedType.LPStr)] string lpszData);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_CompGetCharSet")]
            public static extern int CompGetCharSet(
                IntPtr hWnd, [Out, MarshalAs(UnmanagedType.LPStr)] StringBuilder lpszData,
                int nBuf);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_CompGetSource")]
            public static extern IntPtr CompGetSource(
                IntPtr hWnd);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_CompSetSource")]
            public static extern void CompSetSource(
                IntPtr hWnd, 
                [MarshalAs(UnmanagedType.LPStr)] string lpSource);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_CompGetHeader")]
            public static extern IntPtr CompGetHeader(
                IntPtr hWnd);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_CompGetSpecifiedHeader")]
            public static extern void CompGetSpecifiedHeader(
                IntPtr hWnd,
                [MarshalAs(UnmanagedType.LPStr)] string lpHeader,
                [MarshalAs(UnmanagedType.LPStr)] StringBuilder lpszData,
               int nBuf);
            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_CompSetSpecifiedHeader")]
            public static extern void CompSetSpecifiedHeader(
                IntPtr hWnd, 
                [MarshalAs(UnmanagedType.LPStr)] string lpHeader,
                [MarshalAs(UnmanagedType.LPStr)] string lpszData);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_CompGetText")]
            public static extern IntPtr CompGetText(
                IntPtr hWnd, 
                [Out, MarshalAs(UnmanagedType.LPStr)] StringBuilder lpszMimeType,
                int nBuf);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_CompSetText")]
            public static extern void CompSetText(
                IntPtr hWnd, 
                TextMode nMode,
                [MarshalAs(UnmanagedType.LPStr)] string lpText);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_CompAttachFile")]
            public static extern void CompAttachFile(
                IntPtr hWnd,
                [MarshalAs(UnmanagedType.LPStr)] string lpAttachFile,
                [MarshalAs(UnmanagedType.LPStr)] string lpMimeType);

            /// <summary>
            ///   Do not use this function. It is here for completeness.
            /// </summary>
            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_Alloc")]
            //[return: MarshalAs(UnmanagedType.AsAny)]
            [Obsolete]
            public static extern IntPtr Alloc(
                uint dwSize);

            /// <summary>
            ///   Do not use this function. It is here for completeness.
            /// </summary>
            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_ReAlloc")]
            [Obsolete]
            public static extern IntPtr ReAlloc(
                IntPtr lpVoid, 
                uint dwSize);

            /// <summary>
            ///   Be careful while using - it is here for internal usage only.
            /// </summary>
            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_Free")]
            public static extern void Free(
                IntPtr lpVoid);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_ISO_2022_JP")]
            public static extern IntPtr ISO_2022_JP(
                [MarshalAs(UnmanagedType.LPStr)] string lpSrc,
                [MarshalAs(UnmanagedType.Bool)]bool bEncode);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_ISO_2022_KR")]
            public static extern IntPtr ISO_2022_KR(
                [MarshalAs(UnmanagedType.LPStr)] string lpSrc,
                [MarshalAs(UnmanagedType.Bool)]bool bEncode);


            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_HZ_GB2312")]
            public static extern IntPtr HZ_GB2312(
                [MarshalAs(UnmanagedType.LPStr)] string lpSrc,
                [MarshalAs(UnmanagedType.Bool)]bool bEncode);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_ISO_8859_2")]
            public static extern IntPtr ISO_8859_2(
                [MarshalAs(UnmanagedType.LPStr)] string lpSrc,
                [MarshalAs(UnmanagedType.Bool)]bool bEncode);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_EUC_JP")]
            public static extern IntPtr EUC_JP(
                [MarshalAs(UnmanagedType.LPStr)] string lpSrc,
                [MarshalAs(UnmanagedType.Bool)]bool bEncode);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_UTF_7")]
            public static extern IntPtr UTF_7(
                [MarshalAs(UnmanagedType.LPStr)] string lpSrc,
                [MarshalAs(UnmanagedType.Bool)]bool bEncode);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_UTF_8")]
            public static extern IntPtr UTF_8(
                [MarshalAs(UnmanagedType.LPStr)] string lpSrc,
                [MarshalAs(UnmanagedType.Bool)]bool bEncode);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_B64Convert")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool B64Convert(
                [MarshalAs(UnmanagedType.LPStr)] string lpszOutFile,
                [MarshalAs(UnmanagedType.LPStr)] string lpszInFile,
                [MarshalAs(UnmanagedType.Bool)]bool bEncode);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_QPConvert")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool QPConvert(
                [MarshalAs(UnmanagedType.LPStr)] string lpszOutFile,
                [MarshalAs(UnmanagedType.LPStr)] string lpszInFile,
                [MarshalAs(UnmanagedType.Bool)]bool bEncode);

            /// <param name="lpszIn">Input.</param>
            /// <param name="lpszCharSet">[out] Charset.</param>
            /// <param name="nBuf">Max size Output</param>
            /// <param name="bEncode">MUST be false</param>
            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_MIMEHeader")]
            public static extern IntPtr MIMEHeader(
                [MarshalAs(UnmanagedType.LPStr)] string lpszIn,
                [Out, MarshalAs(UnmanagedType.LPStr)] StringBuilder lpszCharSet,
                int nBuf,
                [MarshalAs(UnmanagedType.Bool)]bool bEncode = false);

            /// <param name="lpszIn">Input.</param>
            /// <param name="lpszCharSet">[in] Charset.</param>
            /// <param name="nBuf">Ignored</param>
            /// <param name="bEncode">MUST be true</param>
            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_MIMEHeader")]
            public static extern IntPtr MIMEHeader(
               [MarshalAs(UnmanagedType.LPStr)] string lpszIn,
               [In, MarshalAs(UnmanagedType.LPStr)] string lpszCharSet,
               int nBuf = 0,
               [MarshalAs(UnmanagedType.Bool)]bool bEncode = true);

            
            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_SerializeRcpts")]
            public static extern IntPtr SerializeRcpts(
                [MarshalAs(UnmanagedType.LPStr)] string lpAddresses);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_Connect")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool Connect(
                [MarshalAs(UnmanagedType.Bool)]bool bConnect);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_NextUnread")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool NextUnread(
                [MarshalAs(UnmanagedType.Bool)]bool bBackScroll,
                [MarshalAs(UnmanagedType.Bool)]bool bGoNext);

            [DllImport("B2.EXE", CallingConvention = CallingConvention.Winapi, EntryPoint = "BKA_ProcessMail")]
            public static extern void ProcessMail(
                [MarshalAs(UnmanagedType.LPStr)] string lpMailID,
                BeckyAction nAction,
                [MarshalAs(UnmanagedType.LPStr)] string lpParam);
        }
    }
}