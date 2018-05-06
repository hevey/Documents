using Documents.iOS.Managers;
using Documents.iOS.Utilities;
using Foundation;
using System;
using UIKit;

namespace Documents.iOS
{
    public partial class SettingsViewController : UITableViewController
    {

        public SettingsViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            string[] tableItems = new string[] { };
            this.TableView.Source = new SettingsDataSource(this);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            this.NavigationController.SetNavigationBarHidden(false, false);
            this.Title = "Settings";
            this.TableView.RowHeight = UITableView.AutomaticDimension;
            this.TableView.EstimatedRowHeight = 40f;
        }

        partial void CloseButton_Activated(UIBarButtonItem sender)
        {
            this.DismissViewController(true, null);
        }
    }
}