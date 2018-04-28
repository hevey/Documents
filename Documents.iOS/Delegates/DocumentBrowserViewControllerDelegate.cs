using System;
using System.IO;
using System.Linq;
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
        private UIDocumentBrowserViewController _controller;

        public override void DidRequestDocumentCreation(UIDocumentBrowserViewController controller, Action<NSUrl, UIDocumentBrowserImportMode> importHandler)
        {
            _importHandler = importHandler;
            _controller = controller;

            var newItemPopOver = UIAlertController.Create(null,null,UIAlertControllerStyle.ActionSheet);
            newItemPopOver.AddAction(UIAlertAction.Create("Word File (.docx)",UIAlertActionStyle.Default,CreateDocx));
            newItemPopOver.AddAction(UIAlertAction.Create("Excel File (.xlsx)",UIAlertActionStyle.Default,CreateXlsx));
            newItemPopOver.AddAction(UIAlertAction.Create("Powerpoint File (.pptx)",UIAlertActionStyle.Default,CreatePptx));
            newItemPopOver.AddAction(UIAlertAction.Create("Pages File (.pages)",UIAlertActionStyle.Default,CreatePages));
            newItemPopOver.AddAction(UIAlertAction.Create("Numbers File (.numbers)",UIAlertActionStyle.Default,CreateNumbers));
            newItemPopOver.AddAction(UIAlertAction.Create("Keynote File (.key)",UIAlertActionStyle.Default,CreateKeynote));
            newItemPopOver.AddAction(UIAlertAction.Create("Text File (.txt)",UIAlertActionStyle.Default,CreateTxt));
            newItemPopOver.AddAction(UIAlertAction.Create("Other", UIAlertActionStyle.Default, CreateOther));
            newItemPopOver.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, CancelPopup));
            
            if (newItemPopOver.PopoverPresentationController != null)
            {
                //FIXME: remove hacky SourceRect code!!!
                newItemPopOver.PopoverPresentationController.SourceView = controller.View;
                newItemPopOver.PopoverPresentationController.PermittedArrowDirections =
                    UIPopoverArrowDirection.Up;
                newItemPopOver.PopoverPresentationController.SourceRect = new CGRect(controller.View.Bounds.Right - 97.5, controller.View.Bounds.Top + 55 , 0, 0);
                //newItemPopOver.PopoverPresentationController.BarButtonItem = controller.ChildViewControllers[0].NavigationItem.RightBarButtonItem;
            }
            
            controller.PresentViewController(newItemPopOver, true, null);


        }

        //Created Document
		public override void DidImportDocument(UIDocumentBrowserViewController controller, NSUrl sourceUrl, NSUrl destinationUrl)
		{
            var creationAttributes = new NSFileAttributes();
            creationAttributes.CreationDate = NSDate.Now;
            creationAttributes.ModificationDate = NSDate.Now;

            NSFileManager.DefaultManager.SetAttributes(creationAttributes, destinationUrl.RelativePath);

            var extension = Path.GetExtension(destinationUrl.AbsoluteString);

            var counter = 1;


            if(extension == ".Blank")
            {
                var newFilenameAlert = UIAlertController.Create("File Name", "Please enter the name of your file", UIAlertControllerStyle.Alert);
                newFilenameAlert.AddTextField(textField => {
                    // If you need to customize the text field, you can do so here.
                });

                //Add Action
                newFilenameAlert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, (sender) => {
                    var newFileName = newFilenameAlert.TextFields.First().Text;
                    if (newFileName != "")
                    {
                        var newFilePath = Path.Combine(Path.GetDirectoryName(destinationUrl.RelativePath), newFileName);
                        var fileNameOnly = Path.GetFileNameWithoutExtension(newFilePath);
                        var newExtension = Path.GetExtension(newFileName);


                        while (File.Exists(newFilePath))
                        {
                            string tempFileName = $"{fileNameOnly} {counter++}{newExtension}";
                            newFilePath = Path.Combine(Path.GetDirectoryName(destinationUrl.RelativePath), tempFileName);
                        }
                           
                        File.Move(destinationUrl.RelativePath, $"{newFilePath}");
                    }
                    else
                    {
                        var oakAlertController = UIAlertController.Create("Error", $"Filename can't be blank.", UIAlertControllerStyle.Alert);

                        //Add Action
                        oakAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

                        // Present Alert
                        _controller.PresentViewController(newFilenameAlert, true, null);
                    }

                }));

                newFilenameAlert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));
                // Present Alert
                _controller.PresentViewController(newFilenameAlert, true, null);

            } 
            else 
            {
                var newFilenameAlert = UIAlertController.Create("File Name", "Please enter the name of your file", UIAlertControllerStyle.Alert);
                newFilenameAlert.AddTextField(textField => {
                    // If you need to customize the text field, you can do so here.
                });

                //Add Action
                newFilenameAlert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, (sender) => {
                    var newFileName = newFilenameAlert.TextFields.First().Text;
                    if (newFileName != "")
                    {
                        var newFilePath = Path.Combine(Path.GetDirectoryName(destinationUrl.RelativePath), newFileName);
                        var fileNameOnly = Path.GetFileNameWithoutExtension(newFilePath);


                        while (File.Exists($"{newFilePath}{extension}"))
                        {
                            string tempFileName = $"{fileNameOnly} {counter++}";
                            newFilePath = Path.Combine(Path.GetDirectoryName(destinationUrl.RelativePath), tempFileName);
                        }

                        File.Move(destinationUrl.RelativePath, $"{newFilePath}{extension}");

                    }
                    else
                    {
                        var oakAlertController = UIAlertController.Create("Error", $"Filename can't be blank.", UIAlertControllerStyle.Alert);

                        //Add Action
                        oakAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

                        // Present Alert
                        _controller.PresentViewController(newFilenameAlert, true, null);
                    }

                }));

                newFilenameAlert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));
                // Present Alert
                _controller.PresentViewController(newFilenameAlert, true, null);

            }
		}

        //Open Exisiting Document
        public override void DidPickDocumentUrls(UIDocumentBrowserViewController controller, NSUrl[] documentUrls)
        {
            var docController = UIDocumentInteractionController.FromUrl(documentUrls[0]);

            docController.Delegate = new DocumentInteractionControllerDelegate(controller);
            docController.PresentOptionsMenu(new CGRect(controller.View.Bounds.Right - 97.5, controller.View.Bounds.Top + 55, 0, 0), controller.View, true);

        }

        //Failed to create Document
		public override void FailedToImportDocument(UIDocumentBrowserViewController controller, NSUrl documentUrl, NSError error)
        {
            
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

        private void CreateOther(UIAlertAction uiAlertAction)
        {
            CreateBlankFile();
        }

        private void CreateFile(string fileType)
        {
            _newDocumentUrl = NSBundle.MainBundle.GetUrlForResource("Untitled", fileType, "Library/TemplateFiles");

            if (_newDocumentUrl == null)
            {
                _importHandler(null, UIDocumentBrowserImportMode.None);
            }
            else
            {
                _importHandler(_newDocumentUrl, UIDocumentBrowserImportMode.Copy);
            }
           
        }

        private void CreateBlankFile()
        {
            _newDocumentUrl = NSBundle.MainBundle.GetUrlForResource("Untitled", "Blank", "Library/TemplateFiles");

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