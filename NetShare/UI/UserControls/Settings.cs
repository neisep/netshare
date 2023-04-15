using Domain;
using Infrastracture;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace NetShare.UI.UserControls
{
    public partial class Settings : UserControl
    {
        private SettingFileHandler _settingsHandler;

        public Dialog CallerForm { get; set; }
        public Settings()
        {
            InitializeComponent();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            CallerForm.Close();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            var options = new SettingsItem();
            foreach (var item in Controls.OfType<CheckBox>().Where(x => x.Visible))
            {
                switch (item.Name)
                {
                    case "AutoMountOnStartUp":
                        options.AutoMountOnStartUp = false;
                        if (item.Checked)
                            options.AutoMountOnStartUp = true;
                        break;
                    case "OnStartupMinimizeToTray":
                        options.OnStartupMinimizeToTray = false;
                        if (item.Checked)
                            options.OnStartupMinimizeToTray = true;
                        break;
                }
            }

            Helper.ApplicationOptions = options;
            _settingsHandler.Save(options);
            CallerForm.Close();
        }

        private void Options_Load(object sender, EventArgs e)
        {
            _settingsHandler = new SettingFileHandler();
            var options = _settingsHandler.Load();

            if (options.AutoMountOnStartUp)
                AutoMountOnStartUp.Checked = true;

            if (options.OnStartupMinimizeToTray)
                OnStartupMinimizeToTray.Checked = true;

            Helper.ApplicationOptions = options;
        }
    }
}
