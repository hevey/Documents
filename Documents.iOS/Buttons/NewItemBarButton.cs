using System;
using System.IO;
using CoreGraphics;
using UIKit;

namespace Documents.iOS.Buttons
{
    public class NewItemBarButton : IUIBarButtonItem
    {
        private UIViewController _view;
        
        public NewItemBarButton(UIViewController view)
        {
            _view = view;
        }
        
        public UIBarButtonItem SetUiBarButtonItem()
        {
            var button = new UIBarButtonItem(UIBarButtonSystemItem.Add);
            button.Clicked += ClickedEvent;
            return button;

        }

        public void ClickedEvent(object sender, EventArgs args)
        {
            var newItemPopOver = UIAlertController.Create(null,null,UIAlertControllerStyle.ActionSheet);
            newItemPopOver.AddAction(UIAlertAction.Create("Word File (.docx)",UIAlertActionStyle.Default,createDocx));
            newItemPopOver.AddAction(UIAlertAction.Create("Excel File (.xlsx)",UIAlertActionStyle.Default,createXlsx));
            newItemPopOver.AddAction(UIAlertAction.Create("Powerpoint File (.pptx)",UIAlertActionStyle.Default,createPptx));
            newItemPopOver.AddAction(UIAlertAction.Create("Pages File (.pages)",UIAlertActionStyle.Default,createPages));
            newItemPopOver.AddAction(UIAlertAction.Create("Numbers File (.numbers)",UIAlertActionStyle.Default,createNumbers));
            newItemPopOver.AddAction(UIAlertAction.Create("Keynote File (.key)",UIAlertActionStyle.Default,createKeynote));
            newItemPopOver.AddAction(UIAlertAction.Create("Text File (.txt)",UIAlertActionStyle.Default,createTxt));
            
            if (newItemPopOver.PopoverPresentationController != null)
            {
                //TODO: Fix where this appears on screen. Should sit under the filename
                newItemPopOver.PopoverPresentationController.SourceView = _view.View;
                newItemPopOver.PopoverPresentationController.PermittedArrowDirections =
                    UIPopoverArrowDirection.Up;
                newItemPopOver.PopoverPresentationController.SourceRect =
                    new CGRect(_view.View.Bounds.GetMidX(), _view.View.Bounds.GetMidY(), 0, 0);
            }
            else
            {
                newItemPopOver.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, action =>
                {
                    newItemPopOver.DismissViewController(true, null);
                }));
            }
            
            _view.PresentViewController(newItemPopOver, true, null);
            
        }

        private void createTxt(UIAlertAction uiAlertAction)
        {
            createFile("txt");
        }

        private void createKeynote(UIAlertAction uiAlertAction)
        {
            createFile("key");
        }

        private void createNumbers(UIAlertAction uiAlertAction)
        {
            createFile("numbers");
        }

        private void createPages(UIAlertAction uiAlertAction)
        {
            createFile("pages");
        }

        private void createPptx(UIAlertAction uiAlertAction)
        {
            createFile("pptx");
        }

        private void createXlsx(UIAlertAction uiAlertAction)
        {
            createFile("xlsx");
        }

        private void createDocx(UIAlertAction uiAlertAction)
        {
            createFile("docx");
        }


        private void createFile(string fileType)
        {
            var newFilename = "Untitled";
            var docsPath = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
            var location = Path.Combine(docsPath, $"{newFilename}.{fileType}");
            var file = Path.Combine("TemplateFiles", $"Untitled.{fileType}");
            var i = 0;
            while (File.Exists(location))
            {
                i++;
                newFilename = $"Untitled {i}";
                location = Path.Combine(docsPath, $"{newFilename}.{fileType}");
            }

            File.Copy(file,location); 
        }
    }
}