using Domain;
using System;
using System.Windows.Forms;

namespace NetShare
{
    public static class ShareItemExtension
    {
        public static ListViewItem ToListView(this ShareItem entity)
        {
            ListViewItem item = new ListViewItem(new[] { entity.DriveLetter, entity.Server, entity.Catalog, "" });
            item.Name = entity.ListViewItemKey.ToString();
            item.Tag = entity;
            return item;
        }
    }
}
