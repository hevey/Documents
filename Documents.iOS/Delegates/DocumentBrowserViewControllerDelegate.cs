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

            var newItemPopOver = UIAlertController.Create(null, null, UIAlertControllerStyle.ActionSheet);
            newItemPopOver.AddAction(UIAlertAction.Create("Word File (.docx)", UIAlertActionStyle.Default, CreateDocx));
            newItemPopOver.AddAction(UIAlertAction.Create("Excel File (.xlsx)", UIAlertActionStyle.Default, CreateXlsx));
            newItemPopOver.AddAction(UIAlertAction.Create("Powerpoint File (.pptx)", UIAlertActionStyle.Default, CreatePptx));
            newItemPopOver.AddAction(UIAlertAction.Create("Pages File (.pages)", UIAlertActionStyle.Default, CreatePages));
            newItemPopOver.AddAction(UIAlertAction.Create("Numbers File (.numbers)", UIAlertActionStyle.Default, CreateNumbers));
            newItemPopOver.AddAction(UIAlertAction.Create("Keynote File (.key)", UIAlertActionStyle.Default, CreateKeynote));
            newItemPopOver.AddAction(UIAlertAction.Create("Text File (.txt)", UIAlertActionStyle.Default, CreateTxt));
            newItemPopOver.AddAction(UIAlertAction.Create("Other", UIAlertActionStyle.Default, CreateOther));
            newItemPopOver.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, CancelPopup));

            if (newItemPopOver.PopoverPresentationController != null)
            {
                //FIXME: remove hacky SourceRect code!!!
                newItemPopOver.PopoverPresentationController.SourceView = controller.View;
                newItemPopOver.PopoverPresentationController.PermittedArrowDirections =
                    UIPopoverArrowDirection.Up;
                newItemPopOver.PopoverPresentationController.SourceRect = new CGRect(controller.View.Bounds.Right - 147.5, controller.View.Bounds.Top + 55, 0, 0);
                //newItemPopOver.PopoverPresentationController.BarButtonItem = controller.ChildViewControllers[0].NavigationItem.RightBarButtonItem;
            }

            controller.PresentViewController(newItemPopOver, true, null);


        }

        //Created Document
        public override void DidImportDocument(UIDocumentBrowserViewController controller, NSUrl sourceUrl, NSUrl destinationUrl)
        {

        }


        //Open Exisiting Document
        public override void DidPickDocumentUrls(UIDocumentBrowserViewController controller, NSUrl[] documentUrls)
        {
            NSError error;
            var fileCoordinator = new NSFileCoordinator();

            fileCoordinator.CoordinateRead(documentUrls[0], NSFileCoordinatorReadingOptions.WithoutChanges, out error, (NSUrl obj) =>
            {
                documentUrls[0].StartAccessingSecurityScopedResource();
                var docController = UIDocumentInteractionController.FromUrl(documentUrls[0]);

                docController.Delegate = new DocumentInteractionControllerDelegate(controller);
                docController.PresentOptionsMenu(new CGRect(controller.View.Bounds.Right - 147.5, controller.View.Bounds.Top + 55, 0, 0), controller.View, true);
                documentUrls[0].StopAccessingSecurityScopedResource();
            });


        }

        //Failed to create Document
        public override void FailedToImportDocument(UIDocumentBrowserViewController controller, NSUrl documentUrl, NSError error)
        {
            Console.WriteLine(error);
        }

        void CancelPopup(UIAlertAction uiAlertAction)
        {
            _importHandler(null, UIDocumentBrowserImportMode.None);
        }

        void CreateTxt(UIAlertAction uiAlertAction)
        {
            CreateFile("txt");
        }

        void CreateKeynote(UIAlertAction uiAlertAction)
        {
            CreateFile("key");
        }

        void CreateNumbers(UIAlertAction uiAlertAction)
        {
            CreateFile("numbers");
        }

        void CreatePages(UIAlertAction uiAlertAction)
        {
            CreateFile("pages");
        }

        void CreatePptx(UIAlertAction uiAlertAction)
        {
            CreateFile("pptx");
        }

        void CreateXlsx(UIAlertAction uiAlertAction)
        {
            CreateFile("xlsx");
        }

        void CreateDocx(UIAlertAction uiAlertAction)
        {
            CreateFile("docx");
        }

        void CreateOther(UIAlertAction uiAlertAction)
        {
            CreateBlankFile();
        }

        void CreateFile(string fileType)
        {
            var newFilenameAlert = UIAlertController.Create("File Name", "Please enter the name of your file", UIAlertControllerStyle.Alert);
            newFilenameAlert.AddTextField(textField =>
            {
            // If you need to customize the text field, you can do so here.
            });


            //Add Action
            newFilenameAlert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, (sender) =>
            {
                NSError error;
                _newDocumentUrl = NSBundle.MainBundle.GetUrlForResource("Untitled", fileType, "Library/TemplateFiles");


                var newFileName = newFilenameAlert.TextFields.First().Text;
                if (newFileName != "")
                {
                    var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    var tmpdir = Path.Combine(documents, "..", "tmp");

                    var newFilePath = Path.Combine(tmpdir, $"{newFileName}.{fileType}");

                    var tempUrl = NSUrl.FromFilename(newFilePath);
                    NSFileManager.DefaultManager.Copy(_newDocumentUrl, tempUrl, out error);

                    var creationAttributes = new NSFileAttributes();
                    creationAttributes.CreationDate = NSDate.Now;
                    creationAttributes.ModificationDate = NSDate.Now;

                    NSFileManager.DefaultManager.SetAttributes(creationAttributes, tempUrl.Path);

                    if (tempUrl == null)
                    {
                        _importHandler(null, UIDocumentBrowserImportMode.None);
                    }
                    else
                    {
                        _importHandler(tempUrl, UIDocumentBrowserImportMode.Move);
                    }
                }
                else
                {
                    var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    var tmpdir = Path.Combine(documents, "..", "tmp");

                    var newFilePath = Path.Combine(tmpdir, $"Untitled.{fileType}");

                    var tempUrl = NSUrl.FromFilename(newFilePath);
                    NSFileManager.DefaultManager.Copy(_newDocumentUrl, tempUrl, out error);

                    var creationAttributes = new NSFileAttributes();
                    creationAttributes.CreationDate = NSDate.Now;
                    creationAttributes.ModificationDate = NSDate.Now;

                    NSFileManager.DefaultManager.SetAttributes(creationAttributes, tempUrl.Path);

                    if (tempUrl == null)
                    {
                        _importHandler(null, UIDocumentBrowserImportMode.None);
                    }
                    else
                    {
                        _importHandler(tempUrl, UIDocumentBrowserImportMode.Move);
                    }
                }



            }));

            newFilenameAlert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, (sender) =>
            {
                
            }));
            // Present Alert
            _controller.PresentViewController(newFilenameAlert, true, null);

        }

        void CreateBlankFile()
        {
            var newFilenameAlert = UIAlertController.Create("File Name", "Please enter the name of your file", UIAlertControllerStyle.Alert);
            newFilenameAlert.AddTextField(textField =>
            {
                // If you need to customize the text field, you can do so here.
            });


            //Add Action
            newFilenameAlert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, (sender) =>
            {
                NSError error;
                _newDocumentUrl = NSBundle.MainBundle.GetUrlForResource("Untitled", "Blank", "Library/TemplateFiles");


                var newFileName = newFilenameAlert.TextFields.First().Text;
                if (newFileName != "")
                {
                    var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    var tmpdir = Path.Combine(documents, "..", "tmp");

                    var newFilePath = Path.Combine(tmpdir, newFileName);

                    var tempUrl = NSUrl.FromFilename(newFilePath);
                    NSFileManager.DefaultManager.Copy(_newDocumentUrl, tempUrl, out error);

                    var creationAttributes = new NSFileAttributes();
                    creationAttributes.CreationDate = NSDate.Now;
                    creationAttributes.ModificationDate = NSDate.Now;

                    NSFileManager.DefaultManager.SetAttributes(creationAttributes, tempUrl.Path);

                    if (tempUrl == null)
                    {
                        _importHandler(null, UIDocumentBrowserImportMode.None);
                    }
                    else
                    {
                        _importHandler(tempUrl, UIDocumentBrowserImportMode.Move);
                    }
                }
                else
                {
                    var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    var tmpdir = Path.Combine(documents, "..", "tmp");

                    var newFilePath = Path.Combine(tmpdir, "Untitled");

                    var tempUrl = NSUrl.FromFilename(newFilePath);
                    NSFileManager.DefaultManager.Copy(_newDocumentUrl, tempUrl, out error);

                    var creationAttributes = new NSFileAttributes();
                    creationAttributes.CreationDate = NSDate.Now;
                    creationAttributes.ModificationDate = NSDate.Now;

                    NSFileManager.DefaultManager.SetAttributes(creationAttributes, tempUrl.Path);

                    if (tempUrl == null)
                    {
                        _importHandler(null, UIDocumentBrowserImportMode.None);
                    }
                    else
                    {
                        _importHandler(tempUrl, UIDocumentBrowserImportMode.Move);
                    }
                }



            }));

            newFilenameAlert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, (sender) =>
            {

            }));

            _controller.PresentViewController(newFilenameAlert, true, null);
        }
    }
}