using Infrastracture;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetShare.UI.UserControls
{
    public partial class Options : UserControl
    {
        private SettingFileHandler _settingsHandler;

        public Dialog CallerForm { get; set; }
        public Options()
        {
            InitializeComponent();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            CallerForm.Close();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            
            //_settingsHandler.Save();
        }

        private void Options_Load(object sender, EventArgs e)
        {
            _settingsHandler = new SettingFileHandler();
            //_settingsHandler.Load();
        }
    }
}
