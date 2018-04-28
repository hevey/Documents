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
            throw new NotImplementedException();
        }
    }
}