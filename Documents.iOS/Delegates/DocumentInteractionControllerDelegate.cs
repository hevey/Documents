using System;
using System.Security.Cryptography;
using UIKit;

namespace Documents.iOS.Delegates
{
    public class DocumentInteractionControllerDelegate : UIDocumentInteractionControllerDelegate
    {
        UIViewController _controller;
        public DocumentInteractionControllerDelegate(UIViewController controller)
        {
            _controller = controller;
        }

        public override UIViewController ViewControllerForPreview(UIDocumentInteractionController controller)
        {
            return _controller;
        }


        public override UIView ViewForPreview(UIDocumentInteractionController controller)
        {
            return _controller.View;
        }

    }
}
