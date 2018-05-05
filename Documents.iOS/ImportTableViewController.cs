using Documents.iOS.Utilities;
using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

namespace Documents.iOS
{
    public partial class ImportTableViewController : UITableViewController
    {

        public string _fileToSave { get; set; }
        private ImportDataSource _dataSource;

        public ImportTableViewController (IntPtr handle) : base (handle)
        {
            
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _dataSource = new ImportDataSource(_fileToSave, this);
            this.TableView.Source = _dataSource;
            this.Title = "Save File";

        }
         
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        partial void CancelButton_Activated(UIBarButtonItem sender)
        {
            this.DismissViewController(true, null);                                       
        }

        partial void SaveButton_Activated(UIBarButtonItem sender)
        {
            _dataSource.saveFile();
            this.DismissViewController(true, null);
        }

        partial void NewFolderButton_Activated(UIBarButtonItem sender)
        {
            _dataSource.createNewFolder();
        }

    }
}