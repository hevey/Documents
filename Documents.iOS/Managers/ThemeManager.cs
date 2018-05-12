using System;
using Documents.iOS.Models;
using Xamarin.Essentials;
using UIKit;
namespace Documents.iOS.Managers
{
    public class ThemeManager
	{
		public static string GetThemeKey()
		{
			return Preferences.Get("theme_key", "light");
		}

		public static void SetThemeKey(string themeKey) 
		{
			Preferences.Set("theme_key", themeKey);
		}

		public static Theme GetTheme()
		{
			var themeKey = GetThemeKey();

            switch (themeKey)
			{
				case "light":
					return new Theme { 
						TableBackgroundColour = UIColor.GroupTableViewBackgroundColor, 
						CellBackgroundColour = UIColor.White,
                        TextColour = UIColor.DarkTextColor,
                        HighlightTextColour = UIColor.LightTextColor,
                        SeperatorColour = UIColor.Gray,
                        NavigationBarStyle = UIBarStyle.Default
					};
				case "dark":
					return new Theme
					{
						TableBackgroundColour = UIColor.Black,
						CellBackgroundColour = new UIColor(0.07f, 1),
						TextColour = UIColor.LightTextColor,
						HighlightTextColour = UIColor.DarkTextColor,
						SeperatorColour = UIColor.LightGray,
                        NavigationBarStyle = UIBarStyle.Black
                    };
				default:
					throw new NotSupportedException($"Theme {themeKey} not supported");
			}


		}
    }
}
