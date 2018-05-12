using Foundation;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Documents.iOS.Models;
using System.Linq;
namespace Documents.iOS.Utilities
{
    public class SettingsDataSource : UITableViewSource
    {
		private const string VisualCellIdentifier = "SettingsCell";
		private const string SwitchCellIdentifier = "SwitchCell";
        private const string LicensesCellIdentifier = "LicensesCell";
		private const int VisualSettingsSection = 0;
        private const int AcknowledgementsSection = 1;

        private List<string> _settingsDetails; 
        private List<string> _otherSettingsDetails;
        private UIViewController _viewController;

        public SettingsDataSource(UIViewController controller)
        {
            _settingsDetails = new List<string>{"Dark Theme"};
            _otherSettingsDetails = new List<string>{ "Open Source Licenses" };
            _viewController = controller;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 2;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            switch (section)
            {
                case VisualSettingsSection:
                    return _settingsDetails.Count;
				case AcknowledgementsSection:
                    return _otherSettingsDetails.Count;
            }
            return 0;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            switch (indexPath.Section)
            {
                case VisualSettingsSection:
                    break;
				case AcknowledgementsSection:
                    var rowString = _otherSettingsDetails[indexPath.Row];
                    if (rowString == "Open Source Licenses")
                    {
                        _viewController.PerformSegue("OSSSegue", this);
                    }

                    break;
            }
        }


        public override string TitleForHeader(UITableView tableView, nint section)
        {
            switch (section)
            {
                case VisualSettingsSection:
                    return "Visual Settings";
                default:
                    return "Acknowledgements";
            }
        }
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
           

			if(indexPath.Section == AcknowledgementsSection)
            {

                UITableViewCell cell = tableView.DequeueReusableCell(LicensesCellIdentifier);

                //---- if there are no cells to reuse, create a new one
                if (cell == null)
                {
                    cell = new UITableViewCell(UITableViewCellStyle.Default, LicensesCellIdentifier);
                }
                var data = _otherSettingsDetails.ToList()[indexPath.Row];
                cell.TextLabel.Text = data;
                return cell;
            }

            if (indexPath.Section == VisualSettingsSection)
            {

				UITableViewCell cell = tableView.DequeueReusableCell(SwitchCellIdentifier);

                //---- if there are no cells to reuse, create a new one
                if (cell == null)
                {
					cell = new UITableViewCell(UITableViewCellStyle.Default, SwitchCellIdentifier);
                }
                var data = _settingsDetails.ToList()[indexPath.Row];
                cell.TextLabel.Text = data;
                return cell;
            }
            return null;
        }
    }
}
