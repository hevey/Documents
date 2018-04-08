using System;
using UIKit;

namespace Documents.iOS.Buttons
{
    public interface IUIBarButtonItem
    {
        UIBarButtonItem SetUiBarButtonItem();
        void ClickedEvent(object sender, EventArgs args);
    }
}