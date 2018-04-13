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
            var button = new UIBarButtonItem();
            button.Title = "\u2699";
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