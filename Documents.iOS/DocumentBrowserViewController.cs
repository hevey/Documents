using Foundation;
using System;
using System.IO;
using UIKit;
using System.IO.Compression;
using System.Linq;
using Documents.iOS.Managers;
using Documents.iOS.Actions;
using System.Collections.Generic;
using Documents.iOS.Buttons;

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

            AllowsDocumentCreation = false;
            AllowsPickingMultipleItems = true;
            BrowserUserInterfaceStyle = UIDocumentBrowserUserInterfaceStyle.Dark;
            AdditionalTrailingNavigationBarButtonItems = setupTrailingButtons();

            CustomActions = SetupActions();

            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            
            var directoryname = Path.Combine(documents, "Downloads");

            if(!Directory.Exists(directoryname))
            {
                Directory.CreateDirectory(directoryname);
            }

            //BrowserUserInterfaceStyle = UIDocumentBrowserUserInterfaceStyle.Dark;
            //View.TintColor = UIColor.LightTextColor;
        }

        UIDocumentBrowserAction[] SetupActions()
        {
            IActionManager actionManager = new ActionManager();
            var list = new List<UIDocumentBrowserAction>();
            foreach(ICustomAction action in actionManager.GetActions(this))
            {
                list.Add(action.SetupAction());
            }
            return list.ToArray();
        }

        UIBarButtonItem[] setupTrailingButtons()
        {
            IUIBarButtonManager buttonManager = new UIBarButtonManager();
            var list = new List<UIBarButtonItem>();
            foreach(IUIBarButtonItem button in buttonManager.GetButtons())
            {
                list.Add(button.SetUiBarButtonItem());
            }
            return list.ToArray();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }




    }
}