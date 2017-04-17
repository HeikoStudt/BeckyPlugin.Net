using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using BeckyTypes.ExportEnums;
using BeckyTypes.PluginListener;
using MimeKit;
using NLog;
using Utilities;


namespace AutoAddressBookImpl
{
    public class BeckyPlugin : IBeckyPlugin
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly BeckyApi.CallsIntoBecky _callsIntoBecky = new BeckyApi.CallsIntoBecky(); //TODO: structuremap?


        public void OnEveryMinute() {

        }

        public IPluginInfo OnPlugInInfo() {
            return null; // default: use mapped assembly properties
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
            string dataFolder = _callsIntoBecky.GetDataFolder();
            Logger.Info("Datafolder: " + dataFolder);
            var emailAddressesInB2AddressBook = GetEmailAddresses(dataFolder).ToLookup(x => x);
            using (var mailStream = lpMessage.GenerateStream(Encoding.UTF8)) {
                var message = MimeMessage.Load(mailStream);
                var allAddresses = GetAllAddressesSentTo(message);

                var newAddresses = allAddresses.Where(x => !emailAddressesInB2AddressBook.Contains(x.Address));
                foreach (var address in newAddresses) {
                    var emailaddress = address.Address;
                    var name = address.Name;
                    Logger.Info("New address: " + emailaddress + " name: " + name);
                }
            }
            return BeckyOnSend.NOTHING;
        }

        private IEnumerable<string> GetEmailAddresses(string dataFolder) {
            List<string> emailAddresses = new List<string>();
            foreach (var vcf in BeckyApi.AddressBook.Helper.GetAllVcfFiles(dataFolder)) {
                emailAddresses.AddRange(GetEmailAddressesVcf(vcf));
            }
            return emailAddresses;
        }

        private IEnumerable<string> GetEmailAddressesVcf(string vcfFile) {
            // not quite correct as of encoding, but not bad as EMAIl is never encoded:
            var lines = File.ReadAllLines(vcfFile);
            foreach (string line in lines) {
                //startswith is fast
                if (line.StartsWith("EMAIL")) {
                    //TODO: there seems to be a notion of "folding" in VCards, currently I am ignoring this fact
                    // The nuget OS library Thought.vCard seems to be of high standard, but dead (2007) vCard 3.0 is latest (?)
                    // The nuget OS library MixERP.Net.VCards seems rather new and in work, but lacks support of inline-charsets
                    //    i made an issue
                    // Third nuget OS library is EWSoftware.PDI.Data of 2015 (so dead again)
                    string email = line.Substring(line.IndexOf(':') + 1);
                    yield return email;
                }
            }
        }

        

        private static IEnumerable<MailboxAddress> GetAllAddressesSentTo(MimeMessage message) {
            var to = message.To.Mailboxes;
            var cc = message.Cc.Mailboxes;
            var bcc = message.Bcc.Mailboxes;
            var rcc = message.ResentCc.Mailboxes;
            return to.Concat(cc).Concat(bcc).Concat(rcc); ;
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
            //var lpMessage = _callsIntoBecky.GetHeader(lpMailId) + "\r\n";
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