using Infrastracture;
using System;
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
    }
}
