using System;
using Foundation;
using UIKit;

namespace Documents.iOS.Actions
{
    public class ArchiveMenuAction : ICustomAction
    {
        private UIViewController _view;

        public ArchiveMenuAction(UIViewController view)
        {
            _view = view;
        }

        public void Action(NSUrl[] obj)
        {
            throw new NotImplementedException();
        }

        public UIDocumentBrowserAction SetupAction()
        {
            var archiveExt = new UIDocumentBrowserAction("com.glennhevey.archive", "Archive", UIDocumentBrowserActionAvailability.Menu, Action);
            archiveExt.SupportedContentTypes = new string[] { "public.item" };
            return archiveExt;
        }
    }
}
