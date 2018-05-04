using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Foundation;
using UIKit;

namespace Documents.iOS.Utilities
{
    public class ImportDataSource: UITableViewSource
    {
        string[] TableItems;
        string CellIdentifier = "ImportCellIdentifier";

        public ImportDataSource()
        {
            TableItems = GetDirectories();
        }


        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);
            string item = TableItems[indexPath.Row];

            //---- if there are no cells to reuse, create a new one
            if (cell == null)
            { cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier); }

            var fullPath = new Uri(item);
            var relativeRoot = new Uri($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\");

            var relativePath = relativeRoot.MakeRelativeUri(fullPath);

            cell.TextLabel.Text = relativePath.ToString();

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return TableItems.Length;
        }

        public string[] GetDirectories()
        {
            var allDirectories = Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "*", SearchOption.AllDirectories).ToList();


            for (int i = allDirectories.Count - 1; i >= 0; i--)
            {
                var directoryName = Path.GetFileName(allDirectories[i]);
                if (directoryName == ".Trash" || directoryName == "Inbox")
                {
                    allDirectories.RemoveAt(i);
                }
            }

            return allDirectories.ToArray();
        }


    }
}
