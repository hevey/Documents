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

			SetTheme();
            SetTint();

			NSNotificationCenter.DefaultCenter.AddObserver((NSString)"theme_changed", SetTheme);
            NSNotificationCenter.DefaultCenter.AddObserver((NSString)"tint_changed", SetTint);

        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            
            this.NavigationController.SetNavigationBarHidden(false, false);
            this.Title = "Settings";
            this.TableView.RowHeight = UITableView.AutomaticDimension;
            this.TableView.EstimatedRowHeight = 40f;
            
            
        }

		private void SetTheme(NSNotification obj)
		{
			UIView.Animate(0.3, () => {
				SetTheme();
			});

		}

        void SetTheme()
		{
			var theme = ThemeManager.GetTheme();
            this.TableView.BackgroundColor = theme.TableBackgroundColour;
            this.TableView.TintColor = theme.SeperatorColour;
            this.TableView.SeparatorColor = theme.SeperatorColour;
            this.NavigationController.NavigationBar.BarStyle = theme.NavigationBarStyle;
			this.TableView.ReloadData();
		}

		partial void CloseButton_Activated(UIBarButtonItem sender)
        {
            this.DismissViewController(true, null);
        }

        private void SetTint(NSNotification obj)
        {
            UIView.Animate(0.3, () => {
                SetTint();
            });

        }

        void SetTint()
        {
            CloseButton.TintColor = ThemeManager.GetTintColour();
        }
    }
}