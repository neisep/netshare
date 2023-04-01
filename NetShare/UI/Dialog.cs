using Domain;
using Infrastracture;
using NetShare.UI.UserControls;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NetShare.UI
{
    public partial class Dialog : Form
    {
        public Object ResultObject { get; set; }
        public Dialog()
        {
            InitializeComponent();            
        }

        private void Dialog_Load(object sender, EventArgs e)
        {
            foreach (var item in Controls)
            {
                if (typeof(UserControl) == item.GetType().BaseType)
                {
                    var userControl = (UserControl)item;
                    Text = $"{userControl.Name} {Helper.ApplicationName}";

                    this.Width = userControl.Width + 25;
                    this.Height = userControl.Height + 50;
                }
            }

            this.CenterToParent();
        }

        public void Open(UserControl userControl)
        {
            Controls.Clear();
            Controls.Add(userControl);
            this.ShowDialog();
            Controls.Clear();
        }

        public ShareItem OpenAddShare(ShareItem editShareItem, AddShare userControl, List<string> excludedDriveLetter)
        {
            userControl.ExcludedDriveLetters = excludedDriveLetter;
            userControl.CallerForm = this;
            if (editShareItem != null)
                userControl.ShareItemEdit = editShareItem;

            this.Controls.Clear();
            this.Controls.Add(userControl);
            this.ShowDialog();
            this.Controls.Clear();

            return (ShareItem)ResultObject;
        }
    }
}
