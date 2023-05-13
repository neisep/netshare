using System.Collections.Generic;

namespace Domain
{
    public class Shares
    {
        public Shares()
        {
            ShareItems = new List<ShareItem>();
        }

        public List<ShareItem> ShareItems { get; set; }
    }
}
