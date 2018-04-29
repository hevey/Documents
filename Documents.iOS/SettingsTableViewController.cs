using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Documents.iOS.Utilities;
using Foundation;
using UIKit;
using Documents.iOS.Managers;
namespace Documents.iOS
{
    [Register("SettingsTableViewController", false)]
    public class SettingsTableViewController : UITableViewController
    {
        private LicenseManager _licenseManager;
        public SettingsTableViewController(IntPtr handle) : base(handle)
        {
            _licenseManager = new LicenseManager();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            string[] tableItems = new string[] { };
            this.TableView.Source = new SettingsDataSource(_licenseManager.GetLicenseDetails());
        }
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            this.NavigationController.SetNavigationBarHidden(false, false);
            this.Title = "Settings";
            this.TableView.RowHeight = UITableView.AutomaticDimension;
            this.TableView.EstimatedRowHeight = 40f;
        }
    }
}