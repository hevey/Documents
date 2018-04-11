using System;
using System.IO;
using CoreGraphics;
using Documents.iOS.Managers;
using Foundation;
using UIKit;

namespace Documents.iOS.Delegates
{
    public class DocumentBrowserViewControllerDelegate : UIDocumentBrowserViewControllerDelegate
    {
        private NSUrl _newDocumentUrl;
        private Action<NSUrl, UIDocumentBrowserImportMode> _importHandler;

        public override void DidRequestDocumentCreation(UIDocumentBrowserViewController controller, Action<NSUrl, UIDocumentBrowserImportMode> importHandler)
        {
            _importHandler = importHandler;
            var newItemPopOver = UIAlertController.Create(null,null,UIAlertControllerStyle.ActionSheet);
            newItemPopOver.AddAction(UIAlertAction.Create("Word File (.docx)",UIAlertActionStyle.Default,CreateDocx));
            newItemPopOver.AddAction(UIAlertAction.Create("Excel File (.xlsx)",UIAlertActionStyle.Default,CreateXlsx));
            newItemPopOver.AddAction(UIAlertAction.Create("Powerpoint File (.pptx)",UIAlertActionStyle.Default,CreatePptx));
            newItemPopOver.AddAction(UIAlertAction.Create("Pages File (.pages)",UIAlertActionStyle.Default,CreatePages));
            newItemPopOver.AddAction(UIAlertAction.Create("Numbers File (.numbers)",UIAlertActionStyle.Default,CreateNumbers));
            newItemPopOver.AddAction(UIAlertAction.Create("Keynote File (.key)",UIAlertActionStyle.Default,CreateKeynote));
            newItemPopOver.AddAction(UIAlertAction.Create("Text File (.txt)",UIAlertActionStyle.Default,CreateTxt));
            
            if (newItemPopOver.PopoverPresentationController != null)
            {
                //TODO: Fix where this appears on screen. Should sit under the filename
                newItemPopOver.PopoverPresentationController.SourceView = controller.View;
                newItemPopOver.PopoverPresentationController.PermittedArrowDirections =
                    UIPopoverArrowDirection.Up;
                newItemPopOver.PopoverPresentationController.SourceRect =
                    new CGRect(controller.View.Bounds.GetMidX(), controller.View.Bounds.GetMidY(), 0, 0);
            }
            else
            {
                newItemPopOver.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, CancelPopup));
            }
            
            controller.PresentViewController(newItemPopOver, true, null);


        }

        private void CancelPopup(UIAlertAction uiAlertAction)
        {
            _importHandler(null, UIDocumentBrowserImportMode.None);
        }

        private void CreateTxt(UIAlertAction uiAlertAction)
        {
            CreateFile("txt");
        }

        private void CreateKeynote(UIAlertAction uiAlertAction)
        {
            CreateFile("key");
        }

        private void CreateNumbers(UIAlertAction uiAlertAction)
        {
            CreateFile("numbers");
        }

        private void CreatePages(UIAlertAction uiAlertAction)
        {
            CreateFile("pages");
        }

        private void CreatePptx(UIAlertAction uiAlertAction)
        {
            CreateFile("pptx");
        }

        private void CreateXlsx(UIAlertAction uiAlertAction)
        {
            CreateFile("xlsx");
        }

        private void CreateDocx(UIAlertAction uiAlertAction)
        {
            CreateFile("docx");
        }


        private void CreateFile(string fileType)
        {
            _newDocumentUrl = NSBundle.MainBundle.GetUrlForResource("Untitled", fileType);
            if (_newDocumentUrl == null)
            {
                _importHandler(null, UIDocumentBrowserImportMode.None);
            }
            else
            {
                _importHandler(_newDocumentUrl, UIDocumentBrowserImportMode.Copy);
            }
           
        }
    }
}