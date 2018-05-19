using System;
using System.IO;
using UIKit;
using Documents.iOS.Managers;
using System.Collections.Generic;
using Documents.iOS.Buttons;
using Documents.iOS.Delegates;
using Foundation;

namespace Documents.iOS
{
    public partial class DocumentBrowserViewController : UIDocumentBrowserViewController
    {
        
        public DocumentBrowserViewController (IntPtr handle) : base (handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			SetTheme();
            SetTint();
            
		}

		public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            
            AllowsDocumentCreation = true;
            AllowsPickingMultipleItems = true;


			NSNotificationCenter.DefaultCenter.AddObserver((NSString)"theme_changed", SetTheme);
            NSNotificationCenter.DefaultCenter.AddObserver((NSString)"tint_changed", SetTint);

            CustomActions = SetupActions();
            AdditionalTrailingNavigationBarButtonItems = SetupTrailingButtons();
            Delegate = new DocumentBrowserViewControllerDelegate();


            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            
            var directoryname = Path.Combine(documents, "Downloads");

            if(!Directory.Exists(directoryname))
            {
                Directory.CreateDirectory(directoryname);
            }
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

        UIBarButtonItem[] SetupTrailingButtons()
        {
            IUIBarButtonManager buttonManager = new TrailingUIBarButtonManager();
            var list = new List<UIBarButtonItem>();
            foreach (IUIBarButtonItem button in buttonManager.GetButtons(this))
            {
                list.Add(button.SetUiBarButtonItem());
            }
            return list.ToArray();
        }

       
		void SetTheme()
		{
			if (ThemeManager.GetThemeKey() == "light")
            {
                BrowserUserInterfaceStyle = UIDocumentBrowserUserInterfaceStyle.Light;
            }
            else
            {
                BrowserUserInterfaceStyle = UIDocumentBrowserUserInterfaceStyle.Dark;
            }
		}

        void SetTint()
        {
            View.TintColor = ThemeManager.GetTintColour();
        }

		void SetTheme(NSNotification obj)
        {
			UIView.Animate(0.3, () =>
			{
				SetTheme();
			});
        }


        void SetTint(NSNotification obj)
        {
            UIView.Animate(0.3, () =>
            {
                SetTint();
            });
        }

    }
}