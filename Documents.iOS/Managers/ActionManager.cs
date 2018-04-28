using System;
using System.Collections.Generic;
using Documents.iOS.Actions;
using UIKit;

namespace Documents.iOS.Managers
{
    public class ActionManager : IActionManager
    {
        public ActionManager()
        {
        }

        public List<ICustomAction> GetActions(UIViewController viewController)
        {

            return new List<ICustomAction>() {
                new UnarchiveAction(viewController),
                new RenameWithExtension(viewController),
                new ArchiveMenuAction(viewController),
                new ArchiveNavigationBarAction(viewController)
            };
        }
    }
}
