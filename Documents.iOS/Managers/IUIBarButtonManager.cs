using System.Collections.Generic;
using Documents.iOS.Buttons;
using UIKit;

namespace Documents.iOS.Managers
{
    public interface IUIBarButtonManager
    {
        List<IUIBarButtonItem> GetButtons(UIViewController viewController);
    }
}