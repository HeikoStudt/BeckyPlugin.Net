using System.Collections.Generic;
using System.IO;
using Utilities;

namespace BeckyApi.AddressBook {
    /*public class AddressBook {
        public int Type { get; }
        public string Path { get; }
    }*/

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

    public class Helper {
        /// <summary>
        ///   Get all .bab (=VCard-Files) files in fullname.
        /// </summary>
        /// <param name="dataFolder">Initial Becky!v2 data folder</param>
        /// <returns>List of full pathes to the Becky VCard files.</returns>
        public static IEnumerable<string> GetAllVcfFiles(string dataFolder) {
            // get all *.bab files in dataFolder/AddrBook and follow links to portable address books
            List<string> result = new List<string>();
            foreach (var addrBook in GetBeckyAddressBookPathes(dataFolder)) {
                result.AddRange(Directory.EnumerateFiles(addrBook, "*.bab", SearchOption.AllDirectories));
            }
            return result;
        }

        /// <summary>
        ///   The "final" address book pathes of all the becky type (vcard-type) address books.
        /// </summary>
        public static IEnumerable<string> GetBeckyAddressBookPathes(string dataFolder) {
            var addressBooksBaseFolder = Path.Combine(dataFolder, "AddrBook");
            // all becky type address books start with an @
            foreach (var addrBookInitial in Directory.EnumerateDirectories(addressBooksBaseFolder, "@*")) {
                yield return FollowAddressIniPath(addrBookInitial);
            }
        }

        /// <summary>
        ///   The "final" address book pathes of all the ldap type address books.
        /// </summary>
        public static IEnumerable<string> GetLdapAddressBookPathes(string dataFolder) {
            var addressBooksBaseFolder = Path.Combine(dataFolder, "AddrBook");
            // all ldap type address books start with an ~
            foreach (var addrBook in Directory.EnumerateDirectories(addressBooksBaseFolder, "~*")) {
                yield return addrBook;
            }
        }

        private static string FollowAddressIniPath(string initialPath) {
            List<string> triedPathes = new List<string>();
            string currentPath = initialPath;

            string newPath = "";
            do {
                triedPathes.Add(currentPath);
                var addressIniPath = Path.Combine(currentPath, "Address.ini");
                if (File.Exists(addressIniPath)) {
                    var addressIni = new IniFile(addressIniPath);
                    newPath = addressIni.Read("Path", "Settings");
                    if (!string.IsNullOrWhiteSpace(newPath)) {
                        currentPath = newPath;
                    }
                }
            } while (newPath != "" && !triedPathes.Contains(newPath));

            return currentPath;
        }
    }
}