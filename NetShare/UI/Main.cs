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
        private Settings _userControlOptions;
        private List<ShareItem> _shares = new List<ShareItem>();
        private Dialog _loadingWindow;
        private bool _mainFormVisible;

        public Main()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            SetupKeyFile();
            LoadApplicationOptions();
            SetupListView();
            InitializeUserControllers();
            Helper.ApplicationName = typeof(Program).Assembly.GetName().Name;
            PopulateListViewFromConfig();

            if (Helper.ApplicationOptions.OnStartupMinimizeToTray)
            {
                this.ShowInTaskbar = false;
                Hide();
                _mainFormVisible = false;
            }

            var autoMountTimer = new System.Timers.Timer(TimeSpan.FromSeconds(30).TotalMilliseconds);
            autoMountTimer.Elapsed += AutoMountTimer_Elapsed;

            if (Helper.ApplicationOptions.AutoMountOnStartUp)
                autoMountTimer.Start();

            InitializeTopMenu();
        }

        private void InitializeTopMenu()
        {
            FontAwesomeHelper fontAwesomeHelper = new FontAwesomeHelper();

            btnAdd.Font = fontAwesomeHelper.GetFont();
            btnEdit.Font = fontAwesomeHelper.GetFont();
            btnRemove.Font = fontAwesomeHelper.GetFont();
            btnAdd.Text = $"{fontAwesomeHelper.LoadIcon("fa-plus-square")} Add";
            btnEdit.Text = $"{fontAwesomeHelper.LoadIcon("fa-edit")} Edit";
            btnRemove.Text = $"{fontAwesomeHelper.LoadIcon("fa-minus-square")} Remove";
        }

        private void AutoMountTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ((System.Timers.Timer)sender).Stop();

            this.Invoke(new MethodInvoker(delegate ()
            {
                MountDrives();
            }));
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
            _userControlOptions = new Settings();
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

            var container = new Container();

            if (!Helper.GetUnusedDriveLetters.Any(x => x == shareItem.DriveLetter))
                return;

            container.ShareItem = shareItem;
            container.Item = selectedItem;
            container.Item.UpdateListViewItem(TableColumns.Status, "Trying to mount drive...");

            OpenLoadingWindow();

            MountDrive(new List<Container>() { container });
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

        //TrayIconMenu
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        //TrayIconMenu
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                WindowState = FormWindowState.Normal;

            _mainFormVisible = true;
            this.ShowInTaskbar = true;
            Show();
            this.Activate();

            if (backgroundWorker1.IsBusy)
                OpenLoadingWindow();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                e.Cancel = true;
                return;
            }

            SaveListViewToConfig();
            trayIcon.Visible = false;
        }

        #endregion

        private void MountDrives()
        {
            SetEnableOrDisableControls();
            List<Container> containers = new List<Container>();
            foreach (ListViewItem item in listViewShares.Items)
            {
                var container = new Container();
                var shareItem = (ShareItem)item.Tag;

                if (!Helper.GetUnusedDriveLetters.Any(x => x == shareItem.DriveLetter))
                    continue;

                container.ShareItem = shareItem;
                container.Item = item;
                container.Item.UpdateListViewItem(TableColumns.Status, "Trying to mount drive...");

                containers.Add(container);
            }

            OpenLoadingWindow();

            MountDrive(containers);
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

        private void MountDrive(List<Container> containers)
        {
            backgroundWorker1.RunWorkerAsync(containers);
        }

        private void OpenLoadingWindow()
        {
            if (_mainFormVisible == false)
                return;

            if (_loadingWindow != null) 
                return;

            _loadingWindow = new Dialog();
            _loadingWindow.ControlBox = false;

            _loadingWindow.Controls.Clear();
            _loadingWindow.Controls.Add(_userControlLoading);
            _loadingWindow.Show(this);
        }

        private void CloseLoadingWindow()
        {
            if (_loadingWindow == null)
                return;

            _loadingWindow.Controls.Clear();
            _loadingWindow.Close();
            _loadingWindow.Dispose();
            _loadingWindow = null;
        }

        #region BackgroundWorker

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var containers = (List<Container>)e.Argument;
            foreach (var item in containers)
            {
                var shareItem = item.ShareItem;
                item.Status = Utility.NetworkDrive.MapNetworkDrive($@"\\{shareItem.Server}\{shareItem.Catalog}", shareItem.DriveLetter, shareItem.UserName, AESGCM.SimpleDecrypt(shareItem.Password, Helper.Key));
            }

            e.Result = containers;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            CloseLoadingWindow();

            var containers = (List<Container>)e.Result;

            foreach (var container in containers)
            {
                container.ShareItem.Status = MountStatus.mapped;
                container.Item.UpdateListViewItem(TableColumns.Status, "Mount successfully");

                if (container.Status == 0)
                    continue;

                var statusMessage = new System.ComponentModel.Win32Exception(container.Status).Message;
                container.ShareItem.Status = MountStatus.notMapped;
                container.Item.UpdateListViewItem(TableColumns.Status, statusMessage);
            }

            SetEnableOrDisableControls();
        }

        #endregion

        private void SetEnableOrDisableControls()
        {
            if(menuStrip.Enabled == true)
            {
                menuStrip.Enabled = false;
                contextMenuStrip.Enabled = false;
            }
            else
            {
                menuStrip.Enabled = true;
                contextMenuStrip.Enabled = true;
            }
        }

        private void LoadApplicationOptions()
        {
            var settingsHandler = new SettingFileHandler();
            Helper.ApplicationOptions = settingsHandler.Load();
        }

        private void Main_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
                _mainFormVisible = false;
                CloseLoadingWindow();
                Hide();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            OpenDialogAddShare(null);
            SaveListViewToConfig();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (listViewShares.SelectedItems.Count == 0)
                return;

            var selectedItem = listViewShares.SelectedItems[0];
            if (!OpenDialogAddShare((ShareItem)selectedItem.Tag))
                return;
            listViewShares.Items.Remove(selectedItem);
            SaveListViewToConfig();
        }
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (listViewShares.SelectedItems.Count == 0)
                return;

            var selectedItem = listViewShares.SelectedItems[0];
            listViewShares.Items.Remove(selectedItem);

            SaveListViewToConfig();
        }
        private void viewHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This button is useless, just like Microsoft windows troubleshooter.");
        }
    }
}
