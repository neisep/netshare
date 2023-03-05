using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
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
            foreach (var item in GetUnusedDriveLetters())
            {
                cboDriveLetter.Items.Add(item);
            }

            cboDriveLetter.SelectedIndex = cboDriveLetter.Items.Count - 1;
            
            LoadExisting();
        }

        private void LoadExisting()
        {
            if (ShareItemEdit == null)
                return;

            txtServer.Text = ShareItemEdit.Server;
            txtUsername.Text = ShareItemEdit.UserName;
            txtPassword.Text = ShareItemEdit.Password;
            txtCatalog.Text = ShareItemEdit.Catalog;
            var index = cboDriveLetter.FindString(ShareItemEdit.DriveLetter);
            cboDriveLetter.SelectedIndex = index;
        }

        private List<string> GetUnusedDriveLetters()
        {
            return Enumerable.Range('A', 'Z' - 'A' + 1).Select(i => (Char)i + ":").Except(DriveInfo.GetDrives().Select(x => x.Name.Replace("\\", ""))).ToList();
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
                Password = txtPassword.Text
            };
            CallerForm.ResultObject = item;

            CallerForm.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CallerForm.Close();
        }
    }
}
