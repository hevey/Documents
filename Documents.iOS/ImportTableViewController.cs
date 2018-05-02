using Foundation;
using System;
using UIKit;

namespace Documents.iOS
{
    public partial class ImportTableViewController : UITableViewController
    {
        public ImportTableViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            this.NavigationController.SetNavigationBarHidden(false, false);
            this.Title = "Import";
            this.TableView.RowHeight = UITableView.AutomaticDimension;
        }

        partial void CancelButton_Activated(UIBarButtonItem sender)
        {
            this.DismissViewController(true, null);                                       
        }
    }
}