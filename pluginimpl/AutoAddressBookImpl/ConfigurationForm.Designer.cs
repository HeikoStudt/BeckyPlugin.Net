namespace AutoAddressBookImpl {
    partial class ConfigurationForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.cbAddressBook = new System.Windows.Forms.ComboBox();
            this.cbGroupPath = new System.Windows.Forms.ComboBox();
            this.lblAddressBook = new System.Windows.Forms.Label();
            this.lblGroupPath = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbAddressBook
            // 
            this.cbAddressBook.AccessibleDescription = "Choose the address book to put in addresses";
            this.cbAddressBook.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbAddressBook.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbAddressBook.FormattingEnabled = true;
            this.cbAddressBook.Location = new System.Drawing.Point(116, 35);
            this.cbAddressBook.Name = "cbAddressBook";
            this.cbAddressBook.Size = new System.Drawing.Size(121, 21);
            this.cbAddressBook.TabIndex = 0;
            this.cbAddressBook.SelectedIndexChanged += new System.EventHandler(this.cbAddressBook_SelectedIndexChanged);
            // 
            // cbGroupPath
            // 
            this.cbGroupPath.AccessibleDescription = "Choose the group (path) for adding addresses";
            this.cbGroupPath.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbGroupPath.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbGroupPath.Enabled = false;
            this.cbGroupPath.FormattingEnabled = true;
            this.cbGroupPath.Location = new System.Drawing.Point(116, 76);
            this.cbGroupPath.Name = "cbGroupPath";
            this.cbGroupPath.Size = new System.Drawing.Size(121, 21);
            this.cbGroupPath.TabIndex = 1;
            // 
            // lblAddressBook
            // 
            this.lblAddressBook.AutoSize = true;
            this.lblAddressBook.Location = new System.Drawing.Point(13, 38);
            this.lblAddressBook.Name = "lblAddressBook";
            this.lblAddressBook.Size = new System.Drawing.Size(70, 13);
            this.lblAddressBook.TabIndex = 2;
            this.lblAddressBook.Text = "Address&Book";
            // 
            // lblGroupPath
            // 
            this.lblGroupPath.AutoSize = true;
            this.lblGroupPath.Location = new System.Drawing.Point(16, 79);
            this.lblGroupPath.Name = "lblGroupPath";
            this.lblGroupPath.Size = new System.Drawing.Size(58, 13);
            this.lblGroupPath.TabIndex = 3;
            this.lblGroupPath.Text = "&GroupPath";
            // 
            // btnOk
            // 
            this.btnOk.AccessibleDescription = "Button OK";
            this.btnOk.Location = new System.Drawing.Point(19, 138);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleDescription = "Cancel Button";
            this.btnCancel.AccessibleName = "";
            this.btnCancel.Location = new System.Drawing.Point(162, 138);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(255, 184);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblGroupPath);
            this.Controls.Add(this.lblAddressBook);
            this.Controls.Add(this.cbGroupPath);
            this.Controls.Add(this.cbAddressBook);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ConfigurationForm";
            this.Text = "Plugin AutoAddressBook";
            this.Load += new System.EventHandler(this.ConfigurationForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbAddressBook;
        private System.Windows.Forms.ComboBox cbGroupPath;
        private System.Windows.Forms.Label lblAddressBook;
        private System.Windows.Forms.Label lblGroupPath;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}