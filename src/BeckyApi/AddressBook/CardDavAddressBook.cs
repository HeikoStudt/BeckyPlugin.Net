using System.Collections.Generic;

namespace BeckyApi.AddressBook {
    public class CardDavAddressBook : AbstractAddressBook {

        public CardDavAddressBook(string addrBook, string initialPath) 
            : base(addrBook, initialPath, false) {
        }

        public override IEnumerable<string> GroupPathes => new string[] { }; // TODO implement for CardDav

        public override IEnumerable<string> GetEmailAddresses() {
            return new string[] { }; // TODO implement for CardDav
        }
    }
}