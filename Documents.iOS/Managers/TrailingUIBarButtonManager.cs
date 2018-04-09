using System.Collections.Generic;
using Documents.iOS.Buttons;
using UIKit;

namespace Documents.iOS.Managers
{
    public class TrailingUIBarButtonManager : IUIBarButtonManager
    {
        public List<IUIBarButtonItem> GetButtons(UIViewController viewController)
        {
            return new List<IUIBarButtonItem>() {
                new NewItemBarButton(viewController)
            };
        }
    }
}