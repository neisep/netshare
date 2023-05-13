using System.ComponentModel;

namespace Infrastracture
{
    public class BindingListEx<ShareItem> : BindingList<ShareItem>
    {
        protected override void RemoveItem(int index)
        {
            ShareItem deletedItem = this.Items[index];

            if(ItemDeleted != null)
                ItemDeleted(deletedItem);

            base.RemoveItem(index);
        }

        public delegate void RemoveDelegate(ShareItem deletedItem);
        public event RemoveDelegate ItemDeleted;
    }
}
