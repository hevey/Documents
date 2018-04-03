using Foundation;
using System;
using UIKit;

namespace Documents.iOS
{
    public partial class DocumentBrowserViewController : UIDocumentBrowserViewController
    {
        public DocumentBrowserViewController (IntPtr handle) : base (handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            AllowsDocumentCreation = true;
            AllowsPickingMultipleItems = true;

            BrowserUserInterfaceStyle = UIDocumentBrowserUserInterfaceStyle.Dark;
            View.TintColor = UIColor.LightTextColor;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}