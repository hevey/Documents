using Foundation;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Documents.iOS.Models;
using System.Linq;
using Documents.iOS.Enums;
using Documents.iOS.Managers;


namespace Documents.iOS.Utilities
{
    public class SettingsDataSource : UITableViewSource
    {
		private const string VisualCellIdentifier = "SettingsCell";
		private const string SwitchCellIdentifier = "SwitchCell";
        private const string LicensesCellIdentifier = "LicensesCell";
		private const int VisualSettingsSection = 0;
        private const int AcknowledgementsSection = 1;

		private List<SettingsCell> _visualUISettings; 
        private List<string> _otherSettingsDetails;
        private UITableViewController _viewController;
		private Theme _theme;

        public SettingsDataSource(UITableViewController controller)
        {
			_visualUISettings = new List<SettingsCell>{ new SettingsCell { Title = "Dark Theme", Type = SettingsCellTypeEnum.Switch, EventHandler = ChangeTheme}};
            _otherSettingsDetails = new List<string>{ "Open Source Licenses" };
            _viewController = controller;
			_theme = ThemeManager.GetTheme();
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
                    return _visualUISettings.Count;
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

				cell.BackgroundColor = _theme.CellBackgroundColour;
                cell.TintColor = _theme.TextColour;
				cell.TextLabel.TextColor = _theme.TextColour;
				cell.TextLabel.HighlightedTextColor = _theme.HighlightTextColour;


                var data = _otherSettingsDetails.ToList()[indexPath.Row];
                cell.TextLabel.Text = data;
                return cell;
            }

            if (indexPath.Section == VisualSettingsSection)
            {
				UITableViewCell cell = null;
				var data = _visualUISettings.ToList()[indexPath.Row];
				switch(data.Type)
				{
					case SettingsCellTypeEnum.Switch:
						cell = CreateSwitchCell(tableView, data);
						break;
				}

                return cell;
            }
            return null;
        }

		private UITableViewCell CreateSwitchCell(UITableView tableView, SettingsCell settingsCell)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(SwitchCellIdentifier);
            var cellSwitch = new UISwitch();

			switch(settingsCell.Title)
			{
				case "Dark Theme":
					cellSwitch.On = ThemeManager.GetThemeKey() == "dark";
					break;
			}
            

            //---- if there are no cells to reuse, create a new one
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, SwitchCellIdentifier);
            }
            if (settingsCell.EventHandler != null)
			{
				cellSwitch.ValueChanged += settingsCell.EventHandler;

			}
            cell.TextLabel.Text = settingsCell.Title;
            cell.AccessoryView = cellSwitch;
            cell.SelectionStyle = UITableViewCellSelectionStyle.None;

			cell.BackgroundColor = _theme.CellBackgroundColour;
            cell.TintColor = _theme.TextColour;
            cell.TextLabel.TextColor = _theme.TextColour;
            cell.TextLabel.HighlightedTextColor = _theme.HighlightTextColour;

			return cell;
		}

        private void ChangeTheme(object sender, EventArgs e)
        {
			var uiSwitch = sender as UISwitch;
			if(uiSwitch.On)
			{
				ThemeManager.SetThemeKey("dark");
			}
			else
			{
				ThemeManager.SetThemeKey("light");
			}
            
			_theme = ThemeManager.GetTheme();

         
			NSNotificationCenter.DefaultCenter.PostNotificationName("theme_changed", null);
        }   

	}
}
