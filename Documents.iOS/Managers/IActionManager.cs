using System;
using System.Collections.Generic;
using Documents.iOS.Actions;
using UIKit;

namespace Documents.iOS.Managers
{
    public interface IActionManager
    {
        List<ICustomAction> GetActions(UIViewController viewController);
    }
}
