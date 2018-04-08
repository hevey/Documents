using System.Collections.Generic;
using Documents.iOS.Buttons;
using UIKit;

namespace Documents.iOS.Managers
{
    public class UIBarButtonManager : IUIBarButtonManager
    {
        public List<IUIBarButtonItem> GetButtons()
        {
            return new List<IUIBarButtonItem>() {
                new NewItemBarButton()
            };
        }
    }
}