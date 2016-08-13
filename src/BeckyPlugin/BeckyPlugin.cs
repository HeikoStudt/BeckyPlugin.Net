using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using BeckyPlugin.BeckyApi;
using NLog;

namespace BeckyPlugin
{
    /// <summary>
    ///   Note that BeckyPlugin must not use generics
    /// </summary>
    public class BeckyPlugin : IBeckyPlugin
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly CallsIntoBecky _callsIntoBecky = new CallsIntoBecky(); //TODO: structuremap?


        public void OnEveryMinute() {

        }

        public PluginInfo OnPlugInInfo() {
            return new PluginInfo {
                PluginName = "Becky C# Plugin",
                Vendor = "HeikoStudt",
                Version = "0.0.0.1",
                Description = "Becky Plugin Description",
            };
        }

        public void OnOpenCompose(IntPtr hWnd, BeckyComposeMode nMode) {
        }

        public bool OnOutgoing(IntPtr hWnd, BeckyOutgoingMode nMode) {
            return true;
        }

        public bool OnKeyDispatch(IntPtr hWnd, Keys nKey, BeckyShiftMode nShift) {
            return false;
        }

        public void OnRetrieve(string lpMessage, string lpMailId) {
        }

        public BeckyOnSend OnSend(string lpMessage) {
            return BeckyOnSend.NOTHING;
        }

        public void OnFinishRetrieve(int nNumber) {
        }

        public bool OnPlugInSetup(IntPtr hWnd) {
            return false;
        }

        public bool OnDragDrop(string lpTgt, string lpSrc, int nCount, BeckyDropEffect dropEffect) {
            return false;
        }

        public BeckyFilter OnBeforeFilter2(string lpMessage, string lpMailBox, out BeckyAction action, out string actionParam) {
            action = BeckyAction.ACTION_NOTHING;
            actionParam = null;
            return BeckyFilter.BKC_FILTER_DEFAULT;
        }

        public void OnStart() {

        }

        public bool OnExit() {
            return true;
        }
        
        public void OnMenuInit(IntPtr hWnd, IntPtr hMenu, BeckyMenu nType) {
            switch (nType) {
                case BeckyMenu.BKC_MENU_MAIN:

                    // Test code is invoked
                    new TestExamples(_callsIntoBecky)
                        .OnMainMenuInit(hWnd, hMenu, nType);

                    break;
                case BeckyMenu.BKC_MENU_LISTVIEW:
                    break;
                case BeckyMenu.BKC_MENU_TREEVIEW:
                case BeckyMenu.BKC_MENU_MSGVIEW:
                case BeckyMenu.BKC_MENU_MSGEDIT:
                    break;
                case BeckyMenu.BKC_MENU_COMPOSE:
                    break;
                case BeckyMenu.BKC_MENU_COMPEDIT:
                case BeckyMenu.BKC_MENU_COMPREF:
                    break;
            }
        }

        public void OnOpenFolder(string lpFolderId) {

        }

        public void OnOpenMail(string lpMailId) {

        }


        #region Exception handling, do not touch
        public bool IsDisabled { get; private set; }

        public void GotUnhandledException(Exception e, [CallerMemberName] string methodName = null) {
            IsDisabled = true; // First
            Logger.Fatal(e, "Got an unhandled exception: {0}", e.Message);
        }
        #endregion
    }
}