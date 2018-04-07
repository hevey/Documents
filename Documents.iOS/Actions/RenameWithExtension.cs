using System;
using Foundation;
using UIKit;

namespace Documents.iOS.Actions
{
    public class RenameWithExtension : ICustomAction
    {
        private UIViewController _view;

        public RenameWithExtension(UIViewController view)
        {
            _view = view;
        }


        public void Action(NSUrl[] obj)
        {
            //Create Alert
            var okAlertController = UIAlertController.Create("Full Rename", "Rename filename with extension", UIAlertControllerStyle.Alert);

            //Add Action
            okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

            // Present Alert
            _view.PresentViewController(okAlertController, true, null);

        }

        public UIDocumentBrowserAction SetupAction()
        {
            var renameWithExt = new UIDocumentBrowserAction("com.glennhevey.rename-with-extension", "Full Rename", UIDocumentBrowserActionAvailability.Menu, Action);

            renameWithExt.SupportedContentTypes = new string[] { "public.item" };

            return renameWithExt;
        }
    }
}
