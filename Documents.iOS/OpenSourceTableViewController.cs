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
            string[] tableItems = { };
            TableView.Source = new OpenSourceLicenseDataSource(_licenseManager.GetLicenseDetails());


        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            NavigationController.SetNavigationBarHidden(false, false);
            Title = "Licenses";
            TableView.RowHeight = UITableView.AutomaticDimension;
            TableView.EstimatedRowHeight = 40f;

			SetTheme();
            SetTint();
        }
        
		private void SetTheme()
        {
            var theme = ThemeManager.GetTheme();
            TableView.BackgroundColor = theme.TableBackgroundColour;
            TableView.TintColor = theme.SeperatorColour;
            TableView.SeparatorColor = theme.SeperatorColour;
			NavigationController.NavigationBar.BarStyle = theme.NavigationBarStyle;
        }

        void SetTint()
        {
            NavigationController.NavigationBar.TintColor = ThemeManager.GetTintColour();
        }
    }
}