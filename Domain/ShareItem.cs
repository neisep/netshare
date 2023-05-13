using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Domain
{
    public class ShareItem
    {
        public string DriveLetter { get; set; }
        public string Server { get; set; }
        public string Catalog { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public MountStatus Status { get; set; }

        public Guid ListViewItemKey { get; set; }
    }

    public enum MountStatus
    {
        notMapped,
        mapped,
    }
}
