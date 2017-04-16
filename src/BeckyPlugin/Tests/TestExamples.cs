using System;
using BeckyApi;
using BeckyApi.Enums;
using BeckyApi.WinApi;
using NLog;
using BeckyMenu = BeckyTypes.ExportEnums.BeckyMenu;


namespace BeckyPlugin {
    public class TestExamples {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public TestExamples(CallsIntoBecky callsIntoBecky)
        {
            CallsIntoBecky = callsIntoBecky;
        }

        private CallsIntoBecky CallsIntoBecky { get; }

        public void CmdTest(IntPtr hWnd, short menuCommandId, short futureUse) {
            //Logger.Info("CmdTest was called with {0} and {1}", hWnd, lParam);
            Logger.Info("Becky version: {0}", CallsIntoBecky.GetVersion());
            //GetWindowHandlesTest();
            //Microsoft.Win32.SafeHandles
            //GetStatus();

            var compose = @"x-becky-mailto:mail@address?X-Becky-Attachment=C:\temp\BeckyPluginTest\bk27300\Plugins\log.txt";
            var hwnd = CallsIntoBecky.ComposeMail(compose);


        }

        private void GetNextMail() {
            int position = -1;
            string mailStart;
            int nSelected = -1; // one time it is incremented anyway
            do {
                position = CallsIntoBecky.GetNextMail(position, out mailStart, true);
                nSelected++;
            } while (position != -1);
            Logger.Info("Selected: {0}", nSelected);
        }

        private void GetStatus() {
            var mailId = CallsIntoBecky.GetCurrentMail();
            Logger.Info("Status: {0}", CallsIntoBecky.GetStatus(mailId));
            CallsIntoBecky.SetStatus(mailId, BeckyMessage.MESSAGE_READ, BeckyMessage.NONE);
            Logger.Info("Status: {0}", CallsIntoBecky.GetStatus(mailId));
            CallsIntoBecky.SetStatus(mailId, BeckyMessage.NONE, BeckyMessage.MESSAGE_READ);
            Logger.Info("Status: {0}", CallsIntoBecky.GetStatus(mailId));
        }

        private void GetCharset() {
            var mail = CallsIntoBecky.GetCurrentMail();
            string charset;
            Logger.Info("Return: {0}", CallsIntoBecky.GetCharSet(mail, out charset));
            Logger.Info("Charset: {0}", charset);
        }


        private string _firstMail = null;
        public void SetCurrentMail() {
            Logger.Info("CurrentMail: {0}", CallsIntoBecky.GetCurrentMail());
            _firstMail = _firstMail ?? CallsIntoBecky.GetCurrentMail();
            Logger.Info("FirstMail:: {0}", _firstMail);
            CallsIntoBecky.SetCurrentMail(_firstMail);

        }

        public void ApiGetters() {
            Logger.Info("Datafolder: {0}", CallsIntoBecky.GetDataFolder());
            Logger.Info("CurrentMailbox: {0}", CallsIntoBecky.GetCurrentMailBox());
            Logger.Info("Tempfolder: {0}", CallsIntoBecky.GetTempFolder());
            Logger.Info("Tempfile: {0}", CallsIntoBecky.GetTempFileName("txt"));
            Logger.Info("CurrentMailbox: {0}", CallsIntoBecky.GetCurrentMailBox());
            Logger.Info("CurrentFolder: {0}", CallsIntoBecky.GetCurrentFolder());
            Logger.Info("CurrentFolder DN: {0}", CallsIntoBecky.GetFolderDisplayName(CallsIntoBecky.GetCurrentFolder()));
            Logger.Info("CurrentMail: {0}", CallsIntoBecky.GetCurrentMail());
        }

        public BeckyCmdUI CmdTestUi(IntPtr hWnd, short menuCommandId, short futureUse) {
            BeckyCmdUI nRetVal = 0;
            nRetVal |= BeckyCmdUI.BKMENU_CMDUI_CHECKED;
            return nRetVal;
        }

        private void CommandTest() {
            CallsIntoBecky.Command(IntPtr.Zero, "Compose");
        }

        private void GetWindowHandlesTest() {

            var res = CallsIntoBecky.GetWindowHandles();
            Logger.Info("{0} | {1} | {2} | {3} | {4}", res, res?.Main, res?.Tree, res?.List, res?.View);
        }

        public void OnMainMenuInit(IntPtr hWnd, IntPtr hMenu, BeckyMenu nType)
        {
            {
                IntPtr hSubMenu = Menus.GetSubMenu(hMenu, 4);
                Menus.AppendMenu(hSubMenu, Menus.MenuFlags.MF_SEPARATOR, 0, null);


                var nId = CallsIntoBecky.RegisterCommand("Test", (BeckyApi.Enums.BeckyMenu)nType, CmdTest);
                CallsIntoBecky.RegisterUICallback(nId, CmdTestUi);
                Menus.AppendMenu(hSubMenu, Menus.MenuFlags.MF_STRING, nId, "Test");

                Logger.Info("NID: {0} {1}", nId, nType);
            }
        }
    }
}
