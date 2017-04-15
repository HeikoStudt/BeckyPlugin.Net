using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using BeckyTypes.ExportEnums;
using BeckyTypes.PluginListener;
using NLog;
using GetAssemblyInformation = BeckyTypes.Helpers.GetAssemblyInformation;


namespace AutoAddressBook
{
    public class BeckyPlugin : IBeckyPlugin
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly BeckyApi.CallsIntoBecky _callsIntoBecky = new BeckyApi.CallsIntoBecky(); //TODO: structuremap?


        public void OnEveryMinute() {

        }

        public IPluginInfo OnPlugInInfo() {
            return new PluginInfo {
                PluginName = GetAssemblyInformation.Title,
                Vendor = GetAssemblyInformation.Vendor,
                Version = GetAssemblyInformation.Version.ToString(),
                Description = GetAssemblyInformation.Description,
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

        public BeckyFilter OnBeforeFilter2(string lpMessage, string lpMailBox, out global::BeckyTypes.ExportEnums.BeckyAction action, out string actionParam) {
            action = global::BeckyTypes.ExportEnums.BeckyAction.ACTION_NOTHING;
            actionParam = null;
            return BeckyFilter.BKC_FILTER_DEFAULT;
        }

        public void OnStart() {

        }

        public bool OnExit() {
            return true;
        }
        
        public void OnMenuInit(IntPtr hWnd, IntPtr hMenu, global::BeckyTypes.ExportEnums.BeckyMenu nType) {
            switch (nType) {
                case global::BeckyTypes.ExportEnums.BeckyMenu.BKC_MENU_MAIN:

                    // Test code is invoked
                    //new TestExamples(_callsIntoBecky)
                    //    .OnMainMenuInit(hWnd, hMenu, nType);

                    break;
                case global::BeckyTypes.ExportEnums.BeckyMenu.BKC_MENU_LISTVIEW:
                    break;
                case global::BeckyTypes.ExportEnums.BeckyMenu.BKC_MENU_TREEVIEW:
                case global::BeckyTypes.ExportEnums.BeckyMenu.BKC_MENU_MSGVIEW:
                case global::BeckyTypes.ExportEnums.BeckyMenu.BKC_MENU_MSGEDIT:
                    break;
                case global::BeckyTypes.ExportEnums.BeckyMenu.BKC_MENU_COMPOSE:
                    break;
                case global::BeckyTypes.ExportEnums.BeckyMenu.BKC_MENU_COMPEDIT:
                case global::BeckyTypes.ExportEnums.BeckyMenu.BKC_MENU_COMPREF:
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