using System;
using System.IO;
using System.ServiceProcess;
using System.Timers;

namespace NetShareService
{
    public partial class NetShareService : ServiceBase
    {
        private Timer _serviceTimer = new Timer();
        public NetShareService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _serviceTimer.Start();
            WriteToLog("Service start");

            WriteToLog("Tries to map network drive!");
            MapNetworkDrive();
        }

        protected override void OnStop()
        {
            _serviceTimer.Stop();
            WriteToLog("Service stop");

            WriteToLog("Tries to disconnect network drives");
            DisconnectMapNetworkDrive();
        }

        private void MapNetworkDrive()
        {
            //var status = Utility.NetworkDrive.MapNetworkDrive(@"", "", "", "");

            //if (status != 0)
            //{
            //    string errorMessage = new System.ComponentModel.Win32Exception(status).Message;
            //    WriteToLog($"Fail to map drive error: {errorMessage}");

            //    return;
            //}

            WriteToLog("Mapped drive successfully");
        }

        private void DisconnectMapNetworkDrive()
        {
            //var status = Utility.NetworkDrive.DisconnectNetworkDrive("Z:", true);

            //if (status != 0)
            //{
            //    string errorMessage = new System.ComponentModel.Win32Exception(status).Message;
            //    WriteToLog($"Fail to disconnect drive error: {errorMessage}");

            //    return;
            //}

            //WriteToLog("Disconnect drive successfully");
        }

        private void WriteToLog(string message)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "netshare" ,"logs");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string filepath = $"{path}\\service_{DateTime.Now.ToString("yyMMdd")}.txt";

            if (!File.Exists(filepath))
            {
                using (var sw = File.CreateText(filepath))
                {
                    sw.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")}: {message}");
                }
                return;
            }

            using (var sw = File.AppendText(filepath))
            {
                sw.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")}: {message}");
            }
        }
    }
}
