using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BeckyApi;
using BeckyApi.AddressBook;
using BeckyTypes.ExportEnums;
using MimeKit;
using NLog;
using Utilities;
using BeckyTypes.PluginListener;

namespace AutoAddressBookImpl
{
    public class BeckyPlugin : AbstractBeckyPlugin
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public BeckyPlugin(string pluginName) : base(pluginName) {
            //OnMainMenuInit += OnMainMenuInitImpl;
        }

        public override BeckyOnSend OnSend(string lpMessage) {
            var emailAddressesInB2AddressBook = GetEmailAddresses(DataFolder).ToLookup(x => x);
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

        private string DefaultAddressBook
        {
            get {
                var result = PluginConfiguration.Read("DefaultAddressBook", "Settings");
                if (string.IsNullOrWhiteSpace(result)) {
                    result = "@Personal";
                }
                return result;
            }
            set {
                PluginConfiguration.Write("DefaultAddressBook", value, "Settings");
            }
        }

        private string DefaultGroupPath
        {
            get {
                var result = PluginConfiguration.Read("DefaultGroupPath", "Settings");
                if (string.IsNullOrWhiteSpace(result)) {
                    result = "@Default";
                }
                return result;
            }
            set {
                PluginConfiguration.Write("DefaultGroupPath", value, "Settings");
            }
        }

        public override bool OnPlugInSetup(IntPtr hWnd) {
            using (var form = new ConfigurationForm(DataFolder, DefaultAddressBook, DefaultGroupPath)) {
                if (DialogResult.OK == form.ShowDialog(NativeWindow.FromHandle(hWnd))) {
                    DefaultAddressBook = form.ChosenAddressBook;
                    DefaultGroupPath = form.ChosenGroupPath;
                }
            }
            return true;
        }


        /*
Carty:
You can directly edit address book data, but make sure if you edit "bab"
file, you will need to delete Group.idx file.
Also, it is better to make sure Address book is not open when you do that.

In Becky!'s address book, VERSION 2.1 and 3.0 is mixed and both can be
used for backward compatibility.
If you put VERSION:3.0 entry, all data will be considered as UTF-8.

Becky!'s vCard implementation is in house, but the vCard structure is
pretty standard, so probably you can use any library to handle vCard
data.
     */
        private void AddAddressToDefaultAddressBook(string emailaddress, string name) {
            
            var babFile = GetFirstBabFile(DefaultAddressBook, DefaultGroupPath);
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

            string uid = Guid.NewGuid().ToString("D");

            var vCard = new[] {
                $"BEGIN:VCARD",
                $"VERSION:3.0",
                $"UID:auto-{uid}",
                $"FN:{name}",
                $"N:{lastName};{firstName};{middle};",
                $"EMAIL;PREF:{emailaddress}",
                $"END:VCARD"
            };
            return vCard;
        }

        private string GetFirstBabFile(string addressBook, string groupPath) {
            var factory = new AddressBookFactory(DataFolder);
            AbstractAddressBook addressBookObject = factory.GetAddressBook(addressBook);
            var beckyAddressBook = (addressBookObject as BeckyAddressBook);
            if (beckyAddressBook == null || !beckyAddressBook.Chooseable) {
                return null;
            }
            var group = beckyAddressBook.GetGroup(groupPath);
            if (group == null) {
                // create ?
                Logger.Error("Could not find group {1} in address book {0}", addressBook, groupPath);
                return null;
            }
            var babs = group.AllBabFilePathes.ToList();
            if (!babs.Any()) {
                Logger.Error("Did not find any BAB file in " + group.FullGroupPath);
                return null;
            }
            if (babs.Count > 1) {
                Logger.Warn("Found more than one BAB file (taking first) in " + group.FullGroupPath);
            }
            return babs.First();
        }

        private IEnumerable<string> GetEmailAddresses(string dataFolder) {
            List<string> emailAddresses = new List<string>();

            var factory = new AddressBookFactory(DataFolder);
            var addressBooks = factory.GetAddressBooks();
            foreach(var addressBook in addressBooks) {
                emailAddresses.AddRange(addressBook.GetEmailAddresses());
            }

            return emailAddresses;
        }

        private static IEnumerable<MailboxAddress> GetAllAddressesSentTo(MimeMessage message) {
            var to = message.To.Mailboxes;
            var cc = message.Cc.Mailboxes;
            var bcc = message.Bcc.Mailboxes;
            var rcc = message.ResentCc.Mailboxes;
            return to.Concat(cc).Concat(bcc).Concat(rcc);
        }

        /*
        public void OnMainMenuInitImpl(IntPtr hWnd, IntPtr hMenu) {
                Logger.Info("OnMainMenuInit");
                //TODO create and use a standard menu win32 wrapper
                //var menu = MenuUtils.GetStandardMenu(hMenu, "&Tools");

                IntPtr hToolsMenu = Menus.GetSubMenu(hMenu, 4); //Tools

                var nId = CallsIntoBecky.RegisterCommand(
                    "Configure AutoAddressBook", 
                    BeckyApi.Enums.BeckyMenu.BKC_MENU_MAIN, 
                    CmdOpenConfiguration);

                Menus.AppendMenu(hToolsMenu, Menus.MenuFlags.MF_SEPARATOR, 0, null);
                Menus.AppendMenu(hToolsMenu, Menus.MenuFlags.MF_STRING, nId, "Configure AutoAddressBook");
        }

        public void CmdOpenConfiguration(IntPtr hWnd, short menuCommandId, short futureUse) {
            //OpenConfigurationDialog(hWnd);
        }*/
    }
}
