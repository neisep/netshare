using Domain;
using Infrastracture;
using Infrastracture.Encryption;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NetShare.UI.UserControls
{
    public partial class AddShare : UserControl
    {
        public Dialog CallerForm { get; set; }
        public ShareItem ShareItemEdit { get; set; }
        public List<String> ExcludedDriveLetters { get; set; }
        public AddShare()
        {
            InitializeComponent();
        }

        private void AddShare_Load(object sender, EventArgs e)
        {
            FontAwesomeHelper fontAwesomeHelper = new FontAwesomeHelper();

            cboDriveLetter.Items.Clear();
            foreach (var item in Helper.GetUnusedDriveLetters)
            {
                if (ExcludedDriveLetters.Contains(item.ToLower()))
                    continue;

                cboDriveLetter.Items.Add(item);
            }

            cboDriveLetter.SelectedIndex = cboDriveLetter.Items.Count - 1;
            
            LoadExisting();

            btnSave.Font = fontAwesomeHelper.GetFont(10);
            btnSave.Text = $"{fontAwesomeHelper.LoadIcon("fa-floppy-o")} Save";
            btnCancel.Font = fontAwesomeHelper.GetFont(10);
            btnCancel.Text = $"{fontAwesomeHelper.LoadIcon("fa-ban")} Cancel"; ;
        }

        private void LoadExisting()
        {
            txtServer.Text = string.Empty;
            txtUsername.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtCatalog.Text = string.Empty;
            cboDriveLetter.SelectedIndex = 0;

            if (ShareItemEdit == null)
                return;

            txtServer.Text = ShareItemEdit.Server;
            txtUsername.Text = ShareItemEdit.UserName;

            if(!string.IsNullOrEmpty(ShareItemEdit.Password))
                txtPassword.Text = AESGCM.SimpleDecrypt(ShareItemEdit.Password, Helper.Key);

            txtCatalog.Text = ShareItemEdit.Catalog;
            var index = cboDriveLetter.FindString(ShareItemEdit.DriveLetter);
            cboDriveLetter.SelectedIndex = index;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtServer.Text))
                return;
            if (string.IsNullOrEmpty(txtUsername.Text))
                return;
            if (string.IsNullOrEmpty(txtUsername.Text))
                return;

            var item = new ShareItem
            {
                DriveLetter = cboDriveLetter.Text,
                Server = txtServer.Text,
                Catalog = txtCatalog.Text,
                UserName = txtUsername.Text,
                ListViewItemKey = Guid.NewGuid()
            };
            if (!string.IsNullOrEmpty(txtPassword.Text))
                item.Password = AESGCM.SimpleEncrypt(txtPassword.Text, Helper.Key);

            CallerForm.ResultObject = item;

            CallerForm.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CallerForm.Close();
        }
    }
}
