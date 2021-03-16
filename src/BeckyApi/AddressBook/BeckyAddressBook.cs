using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utilities;

namespace BeckyApi.AddressBook {
    public class BeckyAddressBook : AbstractAddressBook {

        public BeckyAddressBook(string addrBook, string initialPath) 
            : base(addrBook, initialPath, true) {
        }

        public string FollowedPath => FollowBeckyAddressBookPath(InitialPath); // evaluate each time anew
        public IEnumerable<string> AllBabFilePathes {
            get {
                string followedPath = FollowedPath;
                if (followedPath == null) {
                    return new string[] { };
                }
                return Directory.EnumerateFiles(followedPath, "*.bab", SearchOption.AllDirectories);
            }
        }

        public BeckyAddressBookGroup GetGroup(string groupPath) {
            var fullGroupPath = Path.Combine(FollowedPath, groupPath);
            if (!Directory.Exists(fullGroupPath)) {
                return null;
            }
            return new BeckyAddressBookGroup(groupPath, fullGroupPath);
        }

        // not quite true, it counts only groups having addresses - groups without a bab file (i.e. new groups) are not considered
        public override IEnumerable<string> GroupPathes => AllBabFilePathes
            .Select(x => Path.GetDirectoryName(x)
                .Substring(FollowedPath.Length)
                .Replace('/', '\\')
                .Trim('\\')
            ).Distinct();

        public IEnumerable<BeckyAddressBookGroup> Groups => GroupPathes.Select(x => GetGroup(x));


        public override IEnumerable<string> GetEmailAddresses() {
            List<string> result = new List<string>();
            foreach(var group in Groups) {
                foreach(var babFile in group.AllBabFilePathes) {
                    result.AddRange(GetEmailAddressesVcf(babFile));
                }
            }
            return result;
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

        /// <summary>
        ///   Follow the address book path via Address.ini to the point of no return.
        ///   Returns the 'real' path of the address book.
        ///   Does not work for Ldap address books.
        /// </summary>
        public static string FollowBeckyAddressBookPath(string initialPath) {
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
     
}