using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using BeckyTypes.ExportEnums;
using Utilities;
using NLog;
using BeckyApi;

namespace BeckyTypes.PluginListener
{
    public abstract class AbstractBeckyPlugin : IBeckyPlugin
    {
        private static ILogger Logger = LogManager.GetCurrentClassLogger();

        protected CallsIntoBecky CallsIntoBecky { get; } = new CallsIntoBecky();

        protected string DataFolder => CallsIntoBecky.GetDataFolder();

        private IniFile _pluginConfiguration;

        public string PluginName { get; }


        public AbstractBeckyPlugin(string pluginName) {
            PluginName = pluginName;
        }

        protected IniFile PluginConfiguration {
            get {
                if (_pluginConfiguration == null) {
                    var dataFolder = CallsIntoBecky.GetDataFolder();
                    var pluginFolder = Path.Combine(dataFolder, "plugins", PluginName);
                    var pluginIniName = Path.Combine(pluginFolder, PluginName + ".ini");
                    _pluginConfiguration = _pluginConfiguration ?? new IniFile(pluginIniName);
                }
                return _pluginConfiguration;
            }
        }

        public virtual void OnEveryMinute() { }

        /// <remarks>
        ///   Don't use PluginConfiguration in this method.
        /// </remarks>
        /// <returns>null: use mapped assembly properties</returns>
        public virtual IPluginInfo OnPlugInInfo() {
            return null;
        }

        public virtual void OnOpenCompose(IntPtr hWnd, BeckyComposeMode nMode) { }

        public virtual bool OnOutgoing(IntPtr hWnd, BeckyOutgoingMode nMode) {
            return true;
        }

        public virtual bool OnKeyDispatch(IntPtr hWnd, Keys nKey, BeckyShiftMode nShift) {
            return false;
        }

        public virtual void OnRetrieve(string lpMessage, string lpMailId) { }

        public virtual BeckyOnSend OnSend(string lpMessage) {
            return BeckyOnSend.NOTHING;
        }
        
        public virtual void OnFinishRetrieve(int nNumber) { }

        public virtual bool OnPlugInSetup(IntPtr hWnd) {
            return true;
        }

        public virtual bool OnDragDrop(string lpTgt, string lpSrc, int nCount, BeckyDropEffect dropEffect) {
            return false;
        }

        public virtual BeckyFilter OnBeforeFilter2(string lpMessage, string lpMailBox, out BeckyAction action, out string actionParam) {
            action = BeckyAction.ACTION_NOTHING;
            actionParam = null;
            return BeckyFilter.BKC_FILTER_DEFAULT;
        }

        public virtual void OnStart() {
        }

        public virtual bool OnExit() {
            return true;
        }

        public delegate void OnMenuInitDelegate(IntPtr hWnd, IntPtr hMenu);

        public event OnMenuInitDelegate OnMainMenuInit;
        public event OnMenuInitDelegate OnListViewContextMenuInit;
        public event OnMenuInitDelegate OnTreeViewContextMenuInit;
        public event OnMenuInitDelegate OnMessageViewContextMenuInit;
        public event OnMenuInitDelegate OnMessageViewEditableContextMenuInit;
        public event OnMenuInitDelegate OnTaskTrayContextMenuInit;
        public event OnMenuInitDelegate OnComposerMainMenuInit;
        public event OnMenuInitDelegate OnComposerEditContextMenuInit;
        public event OnMenuInitDelegate OnComposerReferenceContextMenuInit;

        /// <remarks>
        ///   Be careful to call base.OnMenuInit(hWnd, hMenu, nType) as otherwise the events do not work..
        /// </remarks>
        public virtual void OnMenuInit(IntPtr hWnd, IntPtr hMenu, BeckyMenu nType) {
            switch (nType) {
                case BeckyMenu.BKC_MENU_MAIN:
                    OnMainMenuInit?.Invoke(hWnd, hMenu);
                    break;
                case BeckyMenu.BKC_MENU_LISTVIEW:
                    OnListViewContextMenuInit?.Invoke(hWnd, hMenu);
                    break;
                case BeckyMenu.BKC_MENU_TREEVIEW:
                    OnTreeViewContextMenuInit?.Invoke(hWnd, hMenu);
                    break;
                case BeckyMenu.BKC_MENU_MSGVIEW:
                    OnMessageViewContextMenuInit?.Invoke(hWnd, hMenu);
                    break;
                case BeckyMenu.BKC_MENU_MSGEDIT:
                    OnMessageViewEditableContextMenuInit?.Invoke(hWnd, hMenu);
                    break;
                case BeckyMenu.BKC_MENU_TASKTRAY:
                    OnTaskTrayContextMenuInit?.Invoke(hWnd, hMenu);
                    break;
                case BeckyMenu.BKC_MENU_COMPOSE:
                    OnComposerMainMenuInit?.Invoke(hWnd, hMenu);
                    break;
                case BeckyMenu.BKC_MENU_COMPEDIT:
                    OnComposerEditContextMenuInit?.Invoke(hWnd, hMenu);
                    break;
                case BeckyMenu.BKC_MENU_COMPREF:
                    OnComposerReferenceContextMenuInit?.Invoke(hWnd, hMenu);
                    break;
            }
        }

        public virtual void OnOpenFolder(string lpFolderId) {
        }

        public virtual void OnOpenMail(string lpMailId) {
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