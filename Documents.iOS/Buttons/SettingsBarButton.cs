using System;
using UIKit;

namespace Documents.iOS.Buttons
{
    public class SettingsBarButton : IUIBarButtonItem
    {
        private UIViewController _view;
        
        public SettingsBarButton(UIViewController view)
        {
            _view = view;
        }
        
        public UIBarButtonItem SetUiBarButtonItem()
        {
            var image = UIImage.FromBundle("SettingsIcon");
            var button = new UIBarButtonItem();
            button.Image = image;
            button.Style = UIBarButtonItemStyle.Plain;
            button.Clicked += ClickedEvent;
            return button;
        }

        public void ClickedEvent(object sender, EventArgs args)
        {
            var storyboard = UIStoryboard.FromName("Main", null);

            if (storyboard == null)
                return;

            var viewController = storyboard.InstantiateViewController("Settings");

            if (viewController == null)
                return;

            _view.PresentViewController(viewController, true, null);
        
        }
    }
}