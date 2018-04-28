using System;
using Foundation;
using UIKit;

namespace Documents.iOS.Actions
{
    public class ArchiveNavigationBarAction : ICustomAction
    {
        private UIViewController _view;
        public ArchiveNavigationBarAction(UIViewController view)
        {
            _view = view;
        }

        public void Action(NSUrl[] obj)
        {
            throw new NotImplementedException();
        }

        public UIDocumentBrowserAction SetupAction()
        {
            var archiveExt = new UIDocumentBrowserAction("com.glennhevey.archive", "Archive", UIDocumentBrowserActionAvailability.NavigationBar, Action);
            archiveExt.SupportedContentTypes = new string[] { "public.item" };
            return archiveExt;
        }
    }
}
