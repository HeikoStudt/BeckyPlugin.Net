using System.Collections.Generic;

namespace BeckyApi.AddressBook {
    public class UnknownAddressBook : AbstractAddressBook {

        public UnknownAddressBook(string addrBook, string initialPath) 
            : base(addrBook, initialPath, false) {
        }

        public override IEnumerable<string> GroupPathes => new string[] { }; // TODO implement for this variant

        public override IEnumerable<string> GetEmailAddresses() {
            return new string[] { }; // TODO implement for this variant
        }
    }
}