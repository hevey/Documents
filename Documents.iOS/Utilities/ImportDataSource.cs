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
        ImportTableViewController _view;

        List<string> TableItems;
        string _fileToSave = "";
        string CellIdentifier = "ImportCellIdentifier";
        string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public ImportDataSource(string fileToSave, ImportTableViewController view)
        {
            _fileToSave = fileToSave;
            _view = view;
            TableItems = GetDirectories();
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);
            string item = TableItems[indexPath.Row];

            //---- if there are no cells to reuse, create a new one
            if (cell == null)
            { 
                cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier); 
            }

            var fullPath = new Uri(item);
            var relativeRoot = new Uri($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}");

            var relativePath = relativeRoot.MakeRelativeUri(fullPath);

            var folderName = Path.GetFileName(Uri.UnescapeDataString(relativePath.ToString()));

            if (folderName == "")
            {
                folderName = Path.GetFileName(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            }
                
            cell.TextLabel.Text = folderName;
            cell.IndentationLevel = relativePath.ToString().Split('/').Count() - 1;

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return TableItems.Count;
        }

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
            path = TableItems[indexPath.Row];

            _view.Title = $"Save to \"{Path.GetFileName(path)}\"";

            foreach (var button in _view.NavigationItem.RightBarButtonItems)
            {
                button.Enabled = true;
            }

		}

		public override void RowDeselected(UITableView tableView, NSIndexPath indexPath)
		{
            path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

		}


		public void SaveFile()
        {
            var filename = Path.GetFileName(_fileToSave);
            File.Copy(_fileToSave, Path.Combine(path, filename));
        }

        public List<string> GetDirectories()
        {
            var allDirectories = Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "*", SearchOption.AllDirectories).ToList();

            allDirectories.Insert(0, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));

            for (int i = allDirectories.Count - 1; i >= 0; i--)
            {
                var fullPath = new Uri(allDirectories[i]);
                var relativeRoot = new Uri($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}");

                var relativePath = relativeRoot.MakeRelativeUri(fullPath);

                if (relativePath.ToString() == "")
                {
                    continue;
                }
                    
                
                if (relativePath.ToString().Split('/')[1] == ".Trash" || relativePath.ToString().Split('/')[1] == "Inbox")
                {
                    allDirectories.RemoveAt(i);
                }
            }
            allDirectories.Sort();

            return allDirectories;
        }

        public void CreateNewFolder()
        {
            var newFolderAlert = UIAlertController.Create("New Folder", "Enter new folder name.", UIAlertControllerStyle.Alert);

            newFolderAlert.AddTextField(textField => {} );

            //Add Action
            newFolderAlert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, (sender) => {
                var newFolderName = newFolderAlert.TextFields.First().Text;

                if (newFolderName == "")
                {
                    _view.PresentViewController(newFolderAlert, true, null);
                    return;
                }

                //creates new directory as requested
                if(Directory.Exists(Path.Combine(path, newFolderName)))
                {
                    var folderExistsAlert = UIAlertController.Create("Folder Exists", "Folder Already Exists", UIAlertControllerStyle.Alert);
                    folderExistsAlert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, (obj) => 
                    {
                        _view.PresentViewController(newFolderAlert, true, null);    
                    }));

                    _view.PresentViewController(folderExistsAlert, true, null);
                }
                else
                {
                    var directory = Directory.CreateDirectory(Path.Combine(path, newFolderName));


                    var oldData = TableItems;

                    TableItems = GetDirectories();

                    //Get the old data length and determines where to insert new row
                    //Animates the adding in of a row
                    _view.TableView.BeginUpdates();
                    for (int i = oldData.Count; i >= 0; i--)
                    {
                        if (i > oldData.Count - 1)
                        {
                            _view.TableView.InsertRows(new[] { NSIndexPath.Create(0, i) }, UITableViewRowAnimation.Fade);
                        }
                        else if (oldData[i] != TableItems[i])
                        {
                            _view.TableView.ReloadRows(new[] { NSIndexPath.Create(0, i) }, UITableViewRowAnimation.Fade);
                        }
                    }
                    _view.TableView.EndUpdates();   
                }

            }));

            newFolderAlert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, (sender) => {} ));

            // Present Alert
            _view.PresentViewController(newFolderAlert, true, null);

        }
    }
}
