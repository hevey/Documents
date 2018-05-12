using System;
using System.Collections.Generic;
using System.Linq;
using Documents.iOS.Managers;
using Documents.iOS.Models;
using Foundation;
using UIKit;

namespace Documents.iOS.Utilities
{
    public class OpenSourceLicenseDataSource : UITableViewSource
    {
        private const int LicenseDetailsSectionNumber = 0;

        string LicenseCellIdentifier = "LicenseCell";
        IEnumerable<LicenseDetails> _licenses;
		Theme _theme;

        public OpenSourceLicenseDataSource(IEnumerable<LicenseDetails> Licenses)
        {
            _licenses = Licenses;
			_theme = ThemeManager.GetTheme();
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            switch (section)
            {
                case LicenseDetailsSectionNumber:
                    return _licenses.Count();
            }
            return 0;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            switch (indexPath.Section)
            {
                case LicenseDetailsSectionNumber:
                    var data = _licenses.ToList()[indexPath.Row];
                    OpenUrl(data.Uri);
                    break;
            }
        }

        private void OpenUrl(Uri uri)
        {
            UIApplication.SharedApplication.OpenUrl(uri);
        }

        public override string TitleForHeader(UITableView tableView, nint section)
        {
            switch (section)
            {
                case LicenseDetailsSectionNumber:
                    return "Open Source Software - Licenses";
                default:
                    return "";
            }
        }
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {


            if (indexPath.Section == LicenseDetailsSectionNumber)
            {

                UITableViewCell cell = tableView.DequeueReusableCell(LicenseCellIdentifier);

                //---- if there are no cells to reuse, create a new one
                if (cell == null)
                { cell = new UITableViewCell(UITableViewCellStyle.Default, LicenseCellIdentifier); }
                var data = _licenses.ToList()[indexPath.Row];
                cell.TextLabel.Text = data.Title;
                cell.DetailTextLabel.Text = data.License;
                cell.DetailTextLabel.Lines = (data.License.Length - data.License.Replace(Environment.NewLine, string.Empty).Length) * 4;
                cell.DetailTextLabel.AdjustsFontSizeToFitWidth = true;

				cell.BackgroundColor = _theme.CellBackgroundColour;
                cell.TintColor = _theme.TextColour;
                cell.TextLabel.TextColor = _theme.TextColour;
                cell.TextLabel.HighlightedTextColor = _theme.HighlightTextColour;
				cell.DetailTextLabel.TextColor = _theme.TextColour;
				cell.DetailTextLabel.HighlightedTextColor = _theme.HighlightTextColour;
                

                return cell;
            }
            return null;
        }
    }
}