using Foundation;
using System;
using Documents.iOS.Managers;
using Documents.iOS.Utilities;
using UIKit;

namespace Documents.iOS
{
    public partial class OpenSourceTableViewController : UITableViewController
    {
        private LicenseManager _licenseManager;

        public OpenSourceTableViewController (IntPtr handle) : base (handle)
        {
            _licenseManager = new LicenseManager();

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            string[] tableItems = new string[] { };
            this.TableView.Source = new OpenSourceLicenseDataSource(_licenseManager.GetLicenseDetails());
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            this.NavigationController.SetNavigationBarHidden(false, false);
            this.Title = "Licenses";
            this.TableView.RowHeight = UITableView.AutomaticDimension;
            this.TableView.EstimatedRowHeight = 40f;
        }
    }
}