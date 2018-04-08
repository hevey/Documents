using UIKit;

namespace Documents.iOS.Buttons
{
    public class NewItemBarButton : IUIBarButtonItem
    {
        public UIBarButtonItem SetUiBarButtonItem()
        {
            return new UIBarButtonItem(UIBarButtonSystemItem.Add);
        }
    }
}