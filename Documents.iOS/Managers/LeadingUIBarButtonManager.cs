using System.Collections.Generic;
using Documents.iOS.Buttons;
using UIKit;

namespace Documents.iOS.Managers
{
    public class LeadingUIBarButtonManager : IUIBarButtonManager
    {
        public List<IUIBarButtonItem> GetButtons(UIViewController viewController)
        {
            return new List<IUIBarButtonItem>() {
                //new SettingsBarButton()
            };
        }
    }
}