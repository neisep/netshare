//Source is still under going development and is abit messy right now.
using Domain;
using Infrastracture;
using NetShare.UI;
using NetShare.UI.UserControls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace NetShare
{
    public partial class Main : Form
    {
        private About _userControlAbout;
        private AddShare _userControlAddShare;
        private Loading _userControlLoading;
        private List<ShareItem> _shares = new List<ShareItem>();
        private Dialog _loadingWindow;

        public Main()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            SetupListView();
            InitializeUserControllers();
            Helper.ApplicationName = typeof(Program).Assembly.GetName().Name;
            PopulateListViewFromConfig();

            MountDrives();
        }

        private void InitializeUserControllers()
        {
            _userControlAbout = new About();
            _userControlAddShare = new AddShare();
            _userControlLoading = new Loading();
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

        private void MountDrives()
        {
            foreach (ListViewItem item in listViewShares.Items)
            {
                MountDrive((ShareItem)item.Tag, item);
            }
        }

        private void SetupListView()
        {
            listViewShares.View = View.Details;
            AddListViewColumns();
        }
        private void AddListViewColumns()
        {
            listViewShares.Columns.Add(new ColumnHeader { Name = TableColumns.Drive.GetName(), Text = "Drive letter", Width = 100, TextAlign = HorizontalAlignment.Left});
            listViewShares.Columns.Add(new ColumnHeader { Name = TableColumns.Server.GetName(), Text = "Server adress", Width = 100, TextAlign = HorizontalAlignment.Left });
            listViewShares.Columns.Add(new ColumnHeader { Name = TableColumns.Catalog.GetName(), Text = "Catalog", Width = 100, TextAlign = HorizontalAlignment.Left });
            listViewShares.Columns.Add(new ColumnHeader { Name = TableColumns.Status.GetName(), Text = "Status", Width = 400, TextAlign = HorizontalAlignment.Left });
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

        private void CheckDuplicates()
        {
            //if (listViewShares.)
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialogWindow = new Dialog())
            {
                
                dialogWindow.Controls.Clear();
                dialogWindow.Controls.Add(_userControlAbout);

                dialogWindow.ShowDialog(this);
                dialogWindow.Controls.Clear();
            }
        }

        private void AddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowDialogAddShare(null);
            SaveListViewToConfig();
        }

        private void changeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewShares.SelectedItems.Count == 0)
                return;

            var selectedItem = listViewShares.SelectedItems[0];
            ShowDialogAddShare((ShareItem)selectedItem.Tag);
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
            ShowDialogAddShare(null);
            SaveListViewToConfig();
        }

        private void ShowDialogAddShare(ShareItem editShareItem)
        {
            using (var dialogWindow = new Dialog())
            {
                _userControlAddShare.CallerForm = dialogWindow;
                
                if(editShareItem != null)
                    _userControlAddShare.ShareItemEdit = editShareItem;

                dialogWindow.Controls.Clear();
                dialogWindow.Controls.Add(_userControlAddShare);

                dialogWindow.ShowDialog(this);
                dialogWindow.Controls.Clear();

                var shareItem = (ShareItem)dialogWindow.ResultObject;

                if (shareItem != null)
                    AddRow(shareItem);
            }
        }

        private void mapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewShares.SelectedItems.Count == 0)
                return;

            var selectedItem = listViewShares.SelectedItems[0];
            var shareItem = (ShareItem)selectedItem.Tag;
            var status = Utility.NetworkDrive.MapNetworkDrive($@"\\{shareItem.Server}\{shareItem.Catalog}", shareItem.DriveLetter, shareItem.UserName, shareItem.Password);

            MountDrive(shareItem, selectedItem);
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

            //backgroundWorker1.RunWorkerAsync(container);


            //var status = Utility.NetworkDrive.MapNetworkDrive($@"\\{shareItem.Server}\{shareItem.Catalog}", shareItem.DriveLetter, shareItem.UserName, shareItem.Password);

            //var status = task.Result;

            //shareItem.Status = MountStatus.mapped;
            //statusMessage = "Mapped successfully";
            //listViewItem.UpdateListViewItem(TableColumns.Status, statusMessage);

            //if (status == 0) return; //Mount Succeded

            //statusMessage = new System.ComponentModel.Win32Exception(status).Message;
            //shareItem.Status = MountStatus.notMapped;
            //listViewItem.UpdateListViewItem(TableColumns.Status, statusMessage);

        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveListViewToConfig();
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var container = (Container)e.Argument;
            var shareItem = container.ShareItem;

            container.Status = Utility.NetworkDrive.MapNetworkDrive($@"\\{shareItem.Server}\{shareItem.Catalog}", shareItem.DriveLetter, shareItem.UserName, shareItem.Password);

            e.Result = container;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            _loadingWindow.Close();
            _loadingWindow.Dispose();

            var container = (Container)e.Result;

            container.ShareItem.Status = MountStatus.mapped;
            container.Item.UpdateListViewItem(TableColumns.Status, "Mapped successfully");

            if (container.Status == 0) return;

            var statusMessage = new System.ComponentModel.Win32Exception(container.Status).Message;
            container.ShareItem.Status = MountStatus.notMapped;
            container.Item.UpdateListViewItem(TableColumns.Status, statusMessage);
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
    public static class EnumExtension
    {
        public static string GetName(this TableColumns tableEnum)
        {
            return tableEnum.ToString().ToLower();
        }
    }
    public static class ListViewExtention
    {
        public static void UpdateListViewItem(this ListViewItem listViewItem, TableColumns column, string text)
        {
            var matchIndex = -1;
            foreach (ColumnHeader item in listViewItem.ListView.Columns)
            {
                if (item.Name == column.GetName())
                    matchIndex = item.Index;
            }

            if (matchIndex == 0)
                listViewItem.Text = text;

            listViewItem.SubItems[matchIndex].Text = text;
        }
    }
}
