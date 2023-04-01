//Source is still under going development and is abit messy right now.
using Domain;
using Infrastracture;
using Infrastracture.Encryption;
using NetShare.UI;
using NetShare.UI.UserControls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace NetShare
{
    public partial class Main : Form
    {
        private About _userControlAbout;
        private AddShare _userControlAddShare;
        private Loading _userControlLoading;
        private Options _userControlOptions;
        private List<ShareItem> _shares = new List<ShareItem>();
        private Dialog _loadingWindow;

        public Main()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            SetupKeyFile();
            SetupListView();
            InitializeUserControllers();
            Helper.ApplicationName = typeof(Program).Assembly.GetName().Name;
            PopulateListViewFromConfig();

            //MountDrives();
        }

        #region Setup At Startup
        private void SetupKeyFile()
        {
            var keyfile = new KeyFile();
            Helper.Key = keyfile.CreateNew();
        }

        private void SetupListView()
        {
            listViewShares.View = View.Details;
            AddListViewColumns();
        }

        private void AddListViewColumns()
        {
            listViewShares.Columns.Add(new ColumnHeader { Name = TableColumns.Drive.GetName(), Text = "Drive letter", Width = 100, TextAlign = HorizontalAlignment.Left });
            listViewShares.Columns.Add(new ColumnHeader { Name = TableColumns.Server.GetName(), Text = "Server adress", Width = 100, TextAlign = HorizontalAlignment.Left });
            listViewShares.Columns.Add(new ColumnHeader { Name = TableColumns.Catalog.GetName(), Text = "Catalog", Width = 100, TextAlign = HorizontalAlignment.Left });
            listViewShares.Columns.Add(new ColumnHeader { Name = TableColumns.Status.GetName(), Text = "Status", Width = 400, TextAlign = HorizontalAlignment.Left });
        }

        private void InitializeUserControllers()
        {
            _userControlAbout = new About();
            _userControlAddShare = new AddShare();
            _userControlLoading = new Loading();
            _userControlOptions = new Options();
        }

        private void PopulateListViewFromConfig()
        {
            if (!File.Exists(Helper.SharesConfigWithPathAndFileName))
                return;

            var fileContent = File.ReadAllText(Helper.SharesConfigWithPathAndFileName);

            if (string.IsNullOrEmpty(fileContent))
                return;

            var jsonData = JsonConvert.DeserializeObject<Shares>(fileContent);

            foreach (var item in jsonData.ShareItems)
            {
                ListViewItem listViewItem = new ListViewItem(new[] { item.DriveLetter, item.Server, item.Catalog, item.Status.ToString() });
                listViewItem.Tag = item;

                listViewShares.Items.Add(listViewItem);
                _shares.Add(item);
            }
        }

        #endregion

        #region GUI Events

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialogWindow = new Dialog())
            {
                dialogWindow.Open(_userControlAbout);
            }
        }

        private void AddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenDialogAddShare(null);
            SaveListViewToConfig();
        }

        private void changeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewShares.SelectedItems.Count == 0)
                return;

            var selectedItem = listViewShares.SelectedItems[0];
            if (!OpenDialogAddShare((ShareItem)selectedItem.Tag))
                return;
            listViewShares.Items.Remove(selectedItem);
            SaveListViewToConfig();
        }

        private void removeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listViewShares.SelectedItems.Count == 0)
                return;

            var selectedItem = listViewShares.SelectedItems[0];
            listViewShares.Items.Remove(selectedItem);

            SaveListViewToConfig();
        }

        private void addToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenDialogAddShare(null);
            SaveListViewToConfig();
        }

        private void mapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewShares.SelectedItems.Count == 0)
                return;
            SetEnableOrDisableControls();

            var selectedItem = listViewShares.SelectedItems[0];
            var shareItem = (ShareItem)selectedItem.Tag;

            MountDrive(shareItem, selectedItem);
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                e.Cancel = true;
                return;
            }

            SaveListViewToConfig();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialogWindow = new Dialog())
            {
                _userControlOptions.CallerForm = dialogWindow;
                dialogWindow.Controls.Clear();
                dialogWindow.Controls.Add(_userControlOptions);

                dialogWindow.ShowDialog(this);
                dialogWindow.Controls.Clear();
            }
        }

        #endregion

        private void MountDrives()
        {
            SetEnableOrDisableControls();
            foreach (ListViewItem item in listViewShares.Items)
            {
                MountDrive((ShareItem)item.Tag, item);
            }
        }

        private void AddRow(ShareItem entity)
        {
            ListViewItem item = new ListViewItem(new[] { entity.DriveLetter, entity.Server, entity.Catalog, "" });
            item.Tag = entity;

            listViewShares.Items.Add(item);
            _shares.Add(entity);
        }

        private void SaveListViewToConfig()
        {
            var shares = new Shares();
            foreach (ListViewItem shareItem in listViewShares.Items)
            {
                shares.ShareItems.Add((ShareItem)shareItem.Tag);
            }

            using (var sw = File.CreateText(Helper.SharesConfigWithPathAndFileName))
            {
                var shareConfigFile = JsonConvert.SerializeObject(shares);
                sw.WriteLine(shareConfigFile);
            }
        }

        private bool OpenDialogAddShare(ShareItem editShareItem)
        {
            using (var dialogWindow = new Dialog())
            {
                var shareItem = dialogWindow.OpenAddShare(editShareItem, _userControlAddShare, _shares.Select(x => x.DriveLetter.ToLower()).ToList());
                if (shareItem == null)
                    return false; 
               
                AddRow(shareItem);
                return true;
            }
        }

        private void MountDrive(ShareItem shareItem, ListViewItem listViewItem)
        {
            _loadingWindow = new Dialog();
            _loadingWindow.ControlBox = false;
            
            _loadingWindow.Controls.Clear();
            _loadingWindow.Controls.Add(_userControlLoading);
            _loadingWindow.Show(this);

            string statusMessage = string.Empty;

            var container = new Container();
            container.ShareItem = shareItem;
            container.Item = listViewItem;
            container.Item.UpdateListViewItem(TableColumns.Status, "Trying to mount drive...");

            if (!Helper.GetUnusedDriveLetters.Any(x => x == shareItem.DriveLetter))
                return;

            backgroundWorker1.RunWorkerAsync(container);
        }

        #region BackgroundWorker

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var container = (Container)e.Argument;
            var shareItem = container.ShareItem;

            container.Status = Utility.NetworkDrive.MapNetworkDrive($@"\\{shareItem.Server}\{shareItem.Catalog}", shareItem.DriveLetter, shareItem.UserName, AESGCM.SimpleDecrypt(shareItem.Password, Helper.Key));

           e.Result = container;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            _loadingWindow.Controls.Clear();
            _loadingWindow.Close();
            _loadingWindow.Dispose();

            var container = (Container)e.Result;

            container.ShareItem.Status = MountStatus.mapped;
            container.Item.UpdateListViewItem(TableColumns.Status, "Mount successfully");

            if (container.Status == 0) return;

            var statusMessage = new System.ComponentModel.Win32Exception(container.Status).Message;
            container.ShareItem.Status = MountStatus.notMapped;
            container.Item.UpdateListViewItem(TableColumns.Status, statusMessage);

            SetEnableOrDisableControls();
        }

        #endregion

        private void SetEnableOrDisableControls()
        {
            if(menuStrip1.Enabled == true)
            {
                menuStrip1.Enabled = false;
                contextMenuStrip1.Enabled = false;
            }
            else
            {
                menuStrip1.Enabled = true;
                contextMenuStrip1.Enabled = true;
            }
        }
    }

    public enum TableColumns
    {
        Drive,
        Server,
        Catalog,
        Status
    }

    public class Container
    {
        public ShareItem ShareItem { get; set; }
        public ListViewItem Item { get; set; }
        public int Status { get; set; }
    }
}
