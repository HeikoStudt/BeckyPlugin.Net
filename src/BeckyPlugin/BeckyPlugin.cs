using System;
using System.Windows.Forms;
using BeckyTypes.ExportEnums;
using BeckyTypes.PluginListener;
using NLog;


namespace BeckyPlugin
{
    public class BeckyPlugin : AbstractBeckyPlugin
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public BeckyPlugin(string pluginName) : base(pluginName) {
            OnMainMenuInit += BeckyPlugin_OnMainMenuInit;
            OnListViewContextMenuInit += BeckyPlugin_OnListViewContextMenuInit;
            OnTreeViewContextMenuInit += BeckyPlugin_OnTreeViewContextMenuInit;
            OnMessageViewContextMenuInit += BeckyPlugin_OnMessageViewContextMenuInit;
            OnMessageViewEditableContextMenuInit += BeckyPlugin_OnMessageViewEditableContextMenuInit;
            OnTaskTrayContextMenuInit += BeckyPlugin_OnTaskTrayContextMenuInit;
            OnComposerMainMenuInit += BeckyPlugin_OnComposerMainMenuInit;
            OnComposerEditContextMenuInit += BeckyPlugin_OnComposerEditContextMenuInit;
            OnComposerReferenceContextMenuInit += BeckyPlugin_OnComposerReferenceContextMenuInit;
        }

        private void BeckyPlugin_OnComposerReferenceContextMenuInit(IntPtr hWnd, IntPtr hMenu) {
        }

        private void BeckyPlugin_OnComposerEditContextMenuInit(IntPtr hWnd, IntPtr hMenu) {
        }

        private void BeckyPlugin_OnComposerMainMenuInit(IntPtr hWnd, IntPtr hMenu) {
        }

        private void BeckyPlugin_OnTaskTrayContextMenuInit(IntPtr hWnd, IntPtr hMenu) {
        }

        private void BeckyPlugin_OnMessageViewEditableContextMenuInit(IntPtr hWnd, IntPtr hMenu) {
        }

        private void BeckyPlugin_OnMessageViewContextMenuInit(IntPtr hWnd, IntPtr hMenu) {
        }

        private void BeckyPlugin_OnTreeViewContextMenuInit(IntPtr hWnd, IntPtr hMenu) {
        }

        private void BeckyPlugin_OnListViewContextMenuInit(IntPtr hWnd, IntPtr hMenu) {
        }

        private void BeckyPlugin_OnMainMenuInit(IntPtr hWnd, IntPtr hMenu) {
        }

        public override void OnEveryMinute() {

        }

        public override IPluginInfo OnPlugInInfo() {
            // don't use DataFolder or CallsIntoBecky
            return null; // null: use mapped assembly properties
        }

        public override void OnOpenCompose(IntPtr hWnd, BeckyComposeMode nMode) {
        }

        public override bool OnOutgoing(IntPtr hWnd, BeckyOutgoingMode nMode) {
            return true;
        }

        public override bool OnKeyDispatch(IntPtr hWnd, Keys nKey, BeckyShiftMode nShift) {
            return false;
        }

        public override void OnRetrieve(string lpMessage, string lpMailId) {
        }

        public override BeckyOnSend OnSend(string lpMessage) {
            return BeckyOnSend.NOTHING;
        }

        public override void OnFinishRetrieve(int nNumber) {
        }

        public override bool OnPlugInSetup(IntPtr hWnd) {
            return false;
        }

        public override bool OnDragDrop(string lpTgt, string lpSrc, int nCount, BeckyDropEffect dropEffect) {
            return false;
        }

        public override BeckyFilter OnBeforeFilter2(string lpMessage, string lpMailBox, out BeckyAction action, out string actionParam) {
            action = BeckyAction.ACTION_NOTHING;
            actionParam = null;
            return BeckyFilter.BKC_FILTER_DEFAULT;
        }

        public override void OnStart() {

        }

        public override bool OnExit() {
            return true;
        }
        
        public override void OnMenuInit(IntPtr hWnd, IntPtr hMenu, BeckyMenu nType) {
            base.OnMenuInit(hWnd, hMenu, nType);
        }

        public override void OnOpenFolder(string lpFolderId) {

        }

        public override void OnOpenMail(string lpMailId) {

        }
    }
}
