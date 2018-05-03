using Documents.iOS.Utilities;
using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

namespace Documents.iOS
{
    public partial class ImportTableViewController : UITableViewController
    {
        public ImportTableViewController (IntPtr handle) : base (handle)
        {
            
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.TableView.Source = new ImportDataSource();
        }
         
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        partial void CancelButton_Activated(UIBarButtonItem sender)
        {
            this.DismissViewController(true, null);                                       
        }
    }
}