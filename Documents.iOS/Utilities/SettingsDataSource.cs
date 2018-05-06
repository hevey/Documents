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
        private const string SettingsCellIdentifier = "SettingsCell";
        private const string OtherSettingsCellIdentifier = "OtherSettingsCell";
        private const int SettingsSectionNumber = 0;
        private const int OtherSettingsSectionNumber = 1;

        private List<string> _settingsDetails; 
        private List<string> _otherSettingsDetails;
        private UIViewController _viewController;

        public SettingsDataSource(UIViewController controller)
        {
            _settingsDetails = new List<string>{"Test Setting"};
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
                case SettingsSectionNumber:
                    return _settingsDetails.Count;
                case OtherSettingsSectionNumber:
                    return _otherSettingsDetails.Count;
            }
            return 0;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            switch (indexPath.Section)
            {
                case SettingsSectionNumber:
                    break;
                case OtherSettingsSectionNumber:
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
                case SettingsSectionNumber:
                    return "Settings";
                default:
                    return "";
            }
        }
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
           

            if(indexPath.Section == OtherSettingsSectionNumber)
            {

                UITableViewCell cell = tableView.DequeueReusableCell(OtherSettingsCellIdentifier);

                //---- if there are no cells to reuse, create a new one
                if (cell == null)
                {
                    cell = new UITableViewCell(UITableViewCellStyle.Default, OtherSettingsCellIdentifier);
                }
                var data = _otherSettingsDetails.ToList()[indexPath.Row];
                cell.TextLabel.Text = data;
                return cell;
            }

            if (indexPath.Section == SettingsSectionNumber)
            {

                UITableViewCell cell = tableView.DequeueReusableCell(SettingsCellIdentifier);

                //---- if there are no cells to reuse, create a new one
                if (cell == null)
                {
                    cell = new UITableViewCell(UITableViewCellStyle.Default, SettingsCellIdentifier);
                }
                var data = _settingsDetails.ToList()[indexPath.Row];
                cell.TextLabel.Text = data;
                return cell;
            }
            return null;
        }
    }
}
