using Foundation;
using System;
using System.IO;
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

            var renameWithExt = new UIDocumentBrowserAction("com.glennhevey.rename-with-extension", "Full Rename", UIDocumentBrowserActionAvailability.Menu, RenameWithExtensionAction);

            renameWithExt.SupportedContentTypes = new string[] { "public.item" };

            CustomActions = new UIDocumentBrowserAction[] { renameWithExt };

            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var directoryname = Path.Combine(documents, "Downloads");

            Directory.CreateDirectory(directoryname);

            //BrowserUserInterfaceStyle = UIDocumentBrowserUserInterfaceStyle.Dark;
            //View.TintColor = UIColor.LightTextColor;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        void RenameWithExtensionAction(NSUrl[] obj)
        {
            Console.WriteLine("Rename with Extension Tapped");
            //Create Alert
            var okAlertController = UIAlertController.Create("Full Rename", "Rename filename with extension", UIAlertControllerStyle.Alert);

            //Add Action
            okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

            // Present Alert
            PresentViewController(okAlertController, true, null);


        }

    }
}