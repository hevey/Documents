using System;
using System.IO;
using UIKit;

namespace Documents.iOS.Buttons
{
    public class NewItemBarButton : IUIBarButtonItem
    {
        public UIBarButtonItem SetUiBarButtonItem()
        {
            var button = new UIBarButtonItem(UIBarButtonSystemItem.Add);
            button.Clicked += ClickedEvent;
            return button;

        }

        public void ClickedEvent(object sender, EventArgs args)
        {
            var docsPath = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
            var location = Path.Combine(docsPath, "Untitled.docx");

            var wordDocument = Path.Combine("TemplateFiles", "Untitled.docx");
            File.Copy(wordDocument,location);
        }
    }
}