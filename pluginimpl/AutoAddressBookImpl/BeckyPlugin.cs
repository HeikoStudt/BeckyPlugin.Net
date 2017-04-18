using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using BeckyApi;
using BeckyApi.AddressBook;
using BeckyApi.Enums;
using BeckyApi.WinApi;
using BeckyTypes.ExportEnums;
using BeckyTypes.PluginListener;
using MimeKit;
using NLog;
using PInvoke;
using Utilities;
using BeckyMenu = BeckyTypes.ExportEnums.BeckyMenu;


namespace AutoAddressBookImpl
{
    public class BeckyPlugin : IBeckyPlugin
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly CallsIntoBecky _callsIntoBecky = new CallsIntoBecky(); //TODO: structuremap?

        private IniFile _pluginConfiguration;

        public string PluginName { get; }


        public BeckyPlugin(string pluginName) {
            PluginName = pluginName;
        }

        private IniFile PluginConfiguration {
            get {
                if (_pluginConfiguration == null) {
                    var dataFolder = _callsIntoBecky.GetDataFolder();
                    var pluginFolder = Path.Combine(dataFolder, "plugins", PluginName);
                    var pluginIniName = Path.Combine(pluginFolder, PluginName + ".ini");
                    _pluginConfiguration = _pluginConfiguration ?? new IniFile(pluginIniName);
                }
                return _pluginConfiguration;
            }
        }


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
                    AddAddressToDefaultAddressBook(emailaddress, name);
                }
            }
            return BeckyOnSend.NOTHING;
        }

        private void AddAddressToDefaultAddressBook(string emailaddress, string name) {
            string defaultAddressBook = PluginConfiguration.Read("DefaultAddressBook", "Settings");
            string defaultGroupPath = PluginConfiguration.Read("DefaultGroupPath", "Settings");

            if (string.IsNullOrWhiteSpace(defaultAddressBook)) {
                defaultAddressBook = "@Personal";
            }
            if (string.IsNullOrWhiteSpace(defaultGroupPath)) {
                defaultGroupPath = "@Default";
            }

            var babFile = GetFirstBabFile(defaultAddressBook, defaultGroupPath);
            if (babFile == null) {
                return;
            }

            // according to Carty, the groups.idx file should be deleted if you add things in bab files.
            var groupsIdxFile = Path.Combine(Path.GetDirectoryName(babFile), "groups.idx");
            if (File.Exists(groupsIdxFile)) {
                File.Delete(groupsIdxFile);
            }

            var vCard = GetVCard(emailaddress, name);
            File.AppendAllLines(babFile, vCard);

            Logger.Info("New address: {0}, name: {1}. Writing into {2} the vCard {3}", emailaddress, name, babFile, string.Join("\r\n", vCard));
        }

        private static string[] GetVCard(string emailaddress, string name) {
            string[] names = name.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            string firstName = names.Length <= 1 ? "" : (names.FirstOrDefault() ?? "");
            string lastName = names.Length == 0 ? "" : names.Last();
            string middle = names.Length <= 2 ? "" : string.Join(" ", names.Skip(1).Take(names.Length - 2));

            //string uid = "58F279F5.008D0F25.4"; //TODO: create an algorithm
            string uid = Guid.NewGuid().ToString("D");

            var vCard = new[] {
                $"BEGIN:VCARD",
                $"VERSION:3.0",
                $"UID:auto-{uid}",
                $"FN:{name}",
                $"N:{lastName};{firstName};{middle};",
                $"EMAIL;PREF:{emailaddress}",
                //$"REV:2017-04-15T22:18:54Z",
                $"END:VCARD"
            };
            return vCard;
        }

        private string GetFirstBabFile(string addressBook, string groupPath) {
            //TODO: look whether this is an ldap or becky address book
            string initialAddressbookPath = Path.Combine(
                _callsIntoBecky.GetDataFolder(),
                "AddrBook",
                addressBook);
            string addressBookPath = Helper.FollowBeckyAddressBookPath(initialAddressbookPath);
            var fullGroupPath = Path.Combine(addressBookPath, groupPath);
            if (!Directory.Exists(fullGroupPath)) {
                // create ?
                Logger.Error("Could not find address book {1} / group {2} path: {0}",
                    fullGroupPath, addressBook, groupPath);
                return null;
            }
            var babs = Directory.EnumerateFiles(fullGroupPath, "*.bab", SearchOption.TopDirectoryOnly).ToList();
            if (!babs.Any()) {
                Logger.Error("Did not find any BAB file in " + fullGroupPath);
                return null;
            }
            if (babs.Count > 1) {
                Logger.Warn("Found more than one BAB file (taking first) in " + fullGroupPath);
            }
            return babs.First();
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

        public BeckyFilter OnBeforeFilter2(string lpMessage, string lpMailBox, out BeckyTypes.ExportEnums.BeckyAction action, out string actionParam) {
            action = BeckyTypes.ExportEnums.BeckyAction.ACTION_NOTHING;
            actionParam = null;
            return BeckyFilter.BKC_FILTER_DEFAULT;
        }

        public void OnStart() {

        }

        public bool OnExit() {
            return true;
        }


        public void OnMainMenuInit(IntPtr hWnd, IntPtr hMenu, BeckyMenu nType) {
            {
                Logger.Info("OnMainMenuInit");
                var menu = MenuUtils.GetStandardMenu(hMenu, "Tools");
                var nativeWindow = NativeWindow.FromHandle(hWnd);
                Logger.Info("nativeWindow " + nativeWindow);

                IntPtr hSubMenu = Menus.GetSubMenu(hMenu, 4);
                Menus.AppendMenu(hSubMenu, Menus.MenuFlags.MF_SEPARATOR, 0, null);


                //Tools
                var nId = _callsIntoBecky.RegisterCommand("Test", (BeckyApi.Enums.BeckyMenu)nType, CmdTest);
                _callsIntoBecky.RegisterUICallback(nId, CmdTestUi);
                Menus.AppendMenu(hSubMenu, Menus.MenuFlags.MF_STRING, nId, "Test");

                // Main menu
                //Menus.AppendMenu(hMenu, Menus.MenuFlags.MF_STRING, nId, "Test");

                Logger.Info("NID: {0} {1}", nId, nType);
            }
        }

        public void CmdTest(IntPtr hWnd, short menuCommandId, short futureUse) {
            Logger.Info("Becky version: {0}", _callsIntoBecky.GetVersion());
        }

        public BeckyCmdUI CmdTestUi(IntPtr hWnd, short menuCommandId, short futureUse) {
            BeckyCmdUI nRetVal = 0;
            nRetVal |= BeckyCmdUI.BKMENU_CMDUI_CHECKED;
            return nRetVal;
        }

        public void OnMenuInit(IntPtr hWnd, IntPtr hMenu, BeckyMenu nType) {
            switch (nType) {
                case BeckyMenu.BKC_MENU_MAIN:

                    // Test code is invoked
                    //OnMainMenuInit(hWnd, hMenu, nType);

                    break;
                case BeckyMenu.BKC_MENU_LISTVIEW:
                    break;
                case BeckyMenu.BKC_MENU_TREEVIEW:
                case BeckyMenu.BKC_MENU_MSGVIEW:
                case BeckyMenu.BKC_MENU_MSGEDIT:
                    break;
                case BeckyMenu.BKC_MENU_COMPOSE:
                    // Compose window main menu
                    break;
                case BeckyMenu.BKC_MENU_COMPEDIT:
                case BeckyMenu.BKC_MENU_COMPREF:
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