using Infrastracture;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace NetShare.UI.UserControls
{
    public partial class About : UserControl
    {
        public About()
        {
            InitializeComponent();
        }

        private void About_Load(object sender, EventArgs e)
        {
            label1.Text = $"{Helper.ApplicationName}";
            richTextBoxAbout.Text = $"This software was was made to store share in Microsoft Windows 10{Environment.NewLine}{Environment.NewLine}";
            richTextBoxAbout.Text += $"Version: {this.ProductVersion} {Environment.NewLine}{Environment.NewLine}";
            richTextBoxAbout.Text += $"Developer: Jimmie Jönsson {Environment.NewLine}";
            richTextBoxAbout.Text += $"@Github: https://www.github.com/neisep {Environment.NewLine}";
            richTextBoxAbout.Text += $"Repo: https://www.github.com/ {Environment.NewLine}";

        }

        private void OpenNewBrowser(LinkLabel linkLabel)
        {
            linkLabel.LinkVisited = true;
            System.Diagnostics.Process.Start(linkLabel.Text);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var linkLabel = (LinkLabel)sender;
            OpenNewBrowser(linkLabel);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var linkLabel = (LinkLabel)sender;
            OpenNewBrowser(linkLabel);
        }

        private void richTextBoxAbout_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            var url = e.LinkText.ToLower();
            if (string.IsNullOrEmpty(url))
                return;

            if (!url.StartsWith("http"))
                return;
            if (!url.StartsWith("https"))
                return;

            Process.Start(url);
        }
    }
}
