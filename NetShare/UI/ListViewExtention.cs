//Source is still under going development and is abit messy right now.
using System.Windows.Forms;

namespace NetShare
{
    public static class ListViewExtention
    {
        public static void UpdateListViewItem(this ListViewItem listViewItem, TableColumns column, string text)
        {
            var matchIndex = -1;
            foreach (ColumnHeader item in listViewItem.ListView.Columns)
            {
                if (item.Name == column.GetName())
                    matchIndex = item.Index;
            }

            if (matchIndex == 0)
                listViewItem.Text = text;

            listViewItem.SubItems[matchIndex].Text = text;
        }
    }
    public static class ListViewColumnExtention
    {
        public static string GetName(this TableColumns tableEnum)
        {
            return tableEnum.ToString().ToLower();
        }
    }
}
