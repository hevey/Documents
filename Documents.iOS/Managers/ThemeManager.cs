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

        public static string GetTintKey()
        {
            return Preferences.Get("tint_key", "blue");
        }

        public static void SetTintKey(string colour)
        {
            Preferences.Set("tint_key", colour);
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
    
        public static UIColor GetTintColour()
        {
            var tintKey = GetTintKey();

            switch(tintKey)
            {
                case "blue":
                    return UIColor.FromRGBA(0f, 0.35f, 1f, 1f);
                case "orange":
                    return UIColor.Orange;
                default:
                    return UIColor.FromRGBA(0f, 0.35f, 1f, 1f);
            }
             
        }
    }
}
