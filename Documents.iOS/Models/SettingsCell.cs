using System;
using Documents.iOS.Enums;

namespace Documents.iOS.Models
{
    public class SettingsCell
    {
		public string Title { get; set; }
        public SettingsCellTypeEnum Type { get; set; }
		public EventHandler EventHandler { get; set; }
    }
}
