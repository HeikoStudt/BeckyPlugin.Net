using System.Collections.Generic;
using System.IO;

namespace BeckyApi.AddressBook {
    public class BeckyAddressBookGroup {
        public string GroupPath { get; }
        public string FullGroupPath { get; }

        public IEnumerable<string> AllBabFilePathes
        {
            get {
                return Directory.EnumerateFiles(FullGroupPath, "*.bab", SearchOption.TopDirectoryOnly);
            }
        }

        public BeckyAddressBookGroup(string groupPath, string fullGroupPath) {
            this.GroupPath = groupPath;
            this.FullGroupPath = fullGroupPath;
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