using System;
using System.IO;
using UIKit;
using Documents.iOS.Managers;
using System.Collections.Generic;
using Documents.iOS.Buttons;
using Documents.iOS.Delegates;

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
            CustomActions = SetupActions();
            AdditionalLeadingNavigationBarButtonItems = SetupLeadingButtons();
            Delegate = new DocumentBrowserViewControllerDelegate();


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
            foreach(var action in actionManager.GetActions(this))
            {
                list.Add(action.SetupAction());
            }
            return list.ToArray();
        }

        
        UIBarButtonItem[] SetupLeadingButtons()
        {
            IUIBarButtonManager buttonManager = new LeadingUIBarButtonManager();
            var list = new List<UIBarButtonItem>();
            foreach(IUIBarButtonItem button in buttonManager.GetButtons(this))
            {
                list.Add(button.SetUiBarButtonItem());
            }
            return list.ToArray();
        }
    }
}