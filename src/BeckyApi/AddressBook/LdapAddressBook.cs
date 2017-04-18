using System.Collections.Generic;

namespace BeckyApi.AddressBook {
    public class LdapAddressBook : AbstractAddressBook {

        public LdapAddressBook(string addrBook, string initialPath) 
            : base(addrBook, initialPath) {
        }

        public override IEnumerable<string> GroupPathes => new string[] { }; // TODO implement for Ldap

        public override IEnumerable<string> GetEmailAddresses() {
            return new string[] { }; // TODO implement for Ldap
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