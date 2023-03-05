using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace NetShareService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();

            //ProcessInstaller
            this.serviceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.User;
            this.serviceProcessInstaller.Username = null;
            this.serviceProcessInstaller.Password = null;

            //ServiceInstaller
            this.serviceInstaller.Description = "Service for NetShare project";
            this.serviceInstaller.DisplayName = "NetShare Service";
            this.serviceInstaller.ServiceName = "NetShareService";

        }
    }
}
