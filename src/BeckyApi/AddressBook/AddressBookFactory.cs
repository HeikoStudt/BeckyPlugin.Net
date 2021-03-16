using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BeckyApi.AddressBook {

    public class AddressBookFactory {
        private string _beckyDataFolder;

        private string AddressBooksBaseFolder => Path.Combine(_beckyDataFolder, "AddrBook");

        public AddressBookFactory(string beckyDataFolder) {
            _beckyDataFolder = beckyDataFolder;
        }

        /// <summary>
        ///   The list of addressbook names (= folder name) of type Becky.
        /// </summary>
        public IEnumerable<string> GetAddressBookNames() {
            var addressBookPathes = Directory.EnumerateDirectories(AddressBooksBaseFolder, "*", SearchOption.TopDirectoryOnly);
            // all becky type address books start with an @
            foreach (var addrBook in addressBookPathes) {
                // get last directory part of path [ugly]
                yield return addrBook
                    .Replace('/', '\\')
                    .Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries)
                    .Last();
            }
        }

        /// <summary>
        ///   The list of address book objects.
        /// </summary>
        public IEnumerable<AbstractAddressBook> GetAddressBooks() {
            foreach(var addrBook in GetAddressBookNames()) {
                yield return GetAddressBook(addrBook);
            }
        }

        /// <summary>
        ///   Get the address book object with name addressBook (including the prefix @/~)
        /// </summary>
        public AbstractAddressBook GetAddressBook(string addressBook) {
            var initialPath = Path.Combine(AddressBooksBaseFolder, addressBook);
            if (addressBook.StartsWith("@")) {
                // Becky Address Book
                return new BeckyAddressBook(addressBook, initialPath);
            } else if (addressBook.StartsWith("~")) {
                // Ldap address book
                return new LdapAddressBook(addressBook, initialPath);
            } else if (addressBook.StartsWith("^")) {
                // CardDav address book (e.g. ^Google)
                return new CardDavAddressBook(addressBook, initialPath);
            }
            // so that it is not chooseable
            return new UnknownAddressBook(addressBook, initialPath);
        }
    }
}
