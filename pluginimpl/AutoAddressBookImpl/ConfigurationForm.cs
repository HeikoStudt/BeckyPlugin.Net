using BeckyApi.AddressBook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoAddressBookImpl {
    public partial class ConfigurationForm : Form {
        private string beckyDataPath;
        private string defaultAddressBook;
        private string defaultGroupPath;
        

        public string ChosenAddressBook => ((AbstractAddressBook)cbAddressBook.SelectedItem).Name;
        public string ChosenGroupPath => (string)cbGroupPath.SelectedItem;

        public ConfigurationForm() {
            InitializeComponent();
        }

        public ConfigurationForm(string beckyDataPath, string defaultAddressBook, string defaultGroupPath) : this (){
            this.beckyDataPath = beckyDataPath;
            this.defaultAddressBook = defaultAddressBook;
            this.defaultGroupPath = defaultGroupPath;
        }

        private void btnOk_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ConfigurationForm_Load(object sender, EventArgs e) {
            AddressBookFactory factory = new AddressBookFactory(beckyDataPath);
            var addressBooks = factory.GetAddressBooks().ToList();

            cbAddressBook.Items.Clear();
            foreach (var addrBook in addressBooks) {
                // disabled item would be nice...
                if (addrBook.Chooseable) {
                    cbAddressBook.Items.Add(addrBook);
                }
            }

            var selected = addressBooks.FirstOrDefault(x => x.Name == defaultAddressBook);
            if (selected != null) {
                cbAddressBook.SelectedItem = selected;
                if (!selected.GroupPathes.Contains(defaultGroupPath)) {
                    cbGroupPath.Items.Add(defaultGroupPath);
                }
                cbGroupPath.SelectedItem = defaultGroupPath;
            } else {
                cbGroupPath.Items.Add(defaultGroupPath);
                cbGroupPath.SelectedItem = defaultGroupPath;

                if (cbAddressBook.Items.Count == 0) {
                    return; // don't kill myself
                }
                // there is one chosen, even if it was wrong
                cbAddressBook.SelectedIndex = 0;
            }

            // Select the default group path, even if it does not exist
            // for clicking on "OK" without changing should not do nothing wrong
        }

        private void cbAddressBook_SelectedIndexChanged(object sender, EventArgs e) {
            var selectedAddressBook = (AbstractAddressBook)cbAddressBook.SelectedItem;
            var groupPathes = selectedAddressBook.GroupPathes;
            cbGroupPath.Items.Clear();
            foreach (var groupPath in groupPathes.ToList()) {
                cbGroupPath.Items.Add(groupPath);
            }

            if (groupPathes.Contains(defaultGroupPath)) {
                cbGroupPath.SelectedItem = defaultGroupPath;
                cbGroupPath.Enabled = true;
            } else if (cbGroupPath.Items.Count > 0){
                cbGroupPath.SelectedIndex = 0;
                cbGroupPath.Enabled = true;
            } else {
                cbGroupPath.Enabled = false;
            }
        }
    }
}
