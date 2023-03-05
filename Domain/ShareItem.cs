using System.Collections.Generic;
using Infrastracture.Encryption;

namespace Domain
{
    public class ShareItem
    {
        private string _password;

        public string DriveLetter { get; set; }
        public string Server { get; set; }
        public string Catalog { get; set; }
        public string UserName { get; set; }
        public string Password 
        {
            get
            {
                return _password;
            }
            set
            {
                _password = AESGCM.SimpleEncrypt(value, AESGCM.NewKey());
            }
        }
        public MapStatus Status { get; set; }
    }

    public enum MapStatus
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
