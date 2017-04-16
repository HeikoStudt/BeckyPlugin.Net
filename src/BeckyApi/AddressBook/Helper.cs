using System.Collections.Generic;
using System.IO;
using Utilities;

namespace BeckyApi.AddressBook {
    /*public class AddressBook {
        public int Type { get; }
        public string Path { get; }
    }*/

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

        public static IEnumerable<string> GetBeckyAddressBookPathes(string dataFolder) {
            foreach (var addrBookInitial in Directory.EnumerateDirectories(dataFolder)) {
                yield return FollowAddressIniPath(addrBookInitial);
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