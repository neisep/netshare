using System.Collections.Generic;

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
    }

    public enum MountStatus
    {
        notMapped,
        mapped,
    }

    public class Shares
    {
        public Shares()
        {
            ShareItems = new List<ShareItem>();
        }

        public List<ShareItem> ShareItems { get; set; }
    }
}
