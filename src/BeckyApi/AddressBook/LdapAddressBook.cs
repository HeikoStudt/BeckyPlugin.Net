using System.Collections.Generic;

namespace BeckyApi.AddressBook {
    public class LdapAddressBook : AbstractAddressBook {

        public LdapAddressBook(string addrBook, string initialPath) 
            : base(addrBook, initialPath, false) {
        }

        public override IEnumerable<string> GroupPathes => new string[] { }; // TODO implement for Ldap

        public override IEnumerable<string> GetEmailAddresses() {
            return new string[] { }; // TODO implement for Ldap
        }
    }
}