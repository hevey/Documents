using System;
using Foundation;
using UIKit;

namespace Documents.iOS.Actions
{
    public interface ICustomAction
    {
        UIDocumentBrowserAction SetupAction();
        void Action(NSUrl[] obj);
    }
}
