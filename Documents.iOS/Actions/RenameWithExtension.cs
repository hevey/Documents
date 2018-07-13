using System;
using System.IO;
using System.Linq;
using Foundation;
using UIKit;
using CoreGraphics;

namespace Documents.iOS.Actions
{
    public class RenameWithExtension : ICustomAction
    {
        private DocumentBrowserViewController _view;

        public RenameWithExtension(UIViewController view)
        {
            _view = (DocumentBrowserViewController)view;
        }


        public void Action(NSUrl[] obj)
        {
            var previousName = obj[0].LastPathComponent;
            var previousExtension = Path.GetExtension(obj[0].Path);

            var renameAlertContoller = UIAlertController.Create("Rename File", "Please enter new file name",
                UIAlertControllerStyle.Alert);
            renameAlertContoller.AddTextField((textField) => { textField.Text = previousName; });

            renameAlertContoller.AddAction(UIAlertAction.Create("Rename", UIAlertActionStyle.Default, (sender) =>
            {
                var oldName = Path.GetFileName(obj[0].Path);
                var newName = renameAlertContoller.TextFields.First().Text;

                if (newName == "")
                {
                    var blankAlertController = UIAlertController.Create("Error", $"File name can't be blank.",
                        UIAlertControllerStyle.Alert);

                    //Add Action
                    blankAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

                    // Present Alert
                    _view.PresentViewController(blankAlertController, true, null);
                }
                else if (!newName.Contains(".") && oldName.Contains("."))
                {
                    var blankExtensionAlert = UIAlertController.Create("Extension Missing",
                        $"No Extension was given. Do you want to use the previous extension ({previousExtension}) or leave it blank?",
                        UIAlertControllerStyle.ActionSheet);

                    blankExtensionAlert.AddAction(UIAlertAction.Create("Blank", UIAlertActionStyle.Default, (sen) =>
                    {
                        moveFile(obj[0], NSUrl.FromFilename($"{Path.Combine(Path.GetDirectoryName(obj[0].Path), newName)}"));
                    }));

                    blankExtensionAlert.AddAction(UIAlertAction.Create($"{previousExtension}",
                        UIAlertActionStyle.Default, (sen) =>
                        {
                            newName += $"{previousExtension}";
                        moveFile(obj[0], NSUrl.FromFilename($"{Path.Combine(Path.GetDirectoryName(obj[0].Path), newName)}"));
                        }));


                    if (blankExtensionAlert.PopoverPresentationController != null)
                    {
                        //TODO: Fix where this appears on screen. Should sit under the filename
                        blankExtensionAlert.PopoverPresentationController.SourceView = _view.View;
                        blankExtensionAlert.PopoverPresentationController.PermittedArrowDirections =
                            UIPopoverArrowDirection.Up;
                        blankExtensionAlert.PopoverPresentationController.SourceRect = new CGRect(_view.View.Bounds.Right - 147.5, _view.View.Bounds.Top + 55, 0, 0);
                    }


                    _view.PresentViewController(blankExtensionAlert, true, null);
                }
                else 
                {
                    moveFile(obj[0], NSUrl.FromFilename($"{Path.Combine(Path.GetDirectoryName(obj[0].Path), newName)}"));
                }
            }));

            renameAlertContoller.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, (sender) => { }));


            _view.PresentViewController(renameAlertContoller, true, null);
        }

        public UIDocumentBrowserAction SetupAction()
        {
            var renameWithExt = new UIDocumentBrowserAction("com.glennhevey.documents.rename-with-extension", "Rename with extension",
                UIDocumentBrowserActionAvailability.Menu, Action);

            renameWithExt.SupportedContentTypes = new string[] {"public.item"};

            return renameWithExt;
        }

        void moveFile(NSUrl oldFilename, NSUrl newFilename)
        {
            NSError error;
            NSFileCoordinator fileCoordinator = new NSFileCoordinator();

            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var tmpdir = Path.Combine(documents, "..", "tmp");

            var newFilePath = Path.Combine(tmpdir, newFilename.LastPathComponent);

            var tempUrl = NSUrl.FromFilename(newFilePath);
            fileCoordinator.CoordinateWrite(oldFilename, NSFileCoordinatorWritingOptions.ForReplacing, out error, (NSUrl obj) =>
            {
                oldFilename.StartAccessingSecurityScopedResource();



                if(File.Exists(tempUrl.Path))
                {
                    File.Delete(tempUrl.Path);
                }

                NSFileManager.DefaultManager.Copy(oldFilename, tempUrl, out error);


                _view.ImportDocument(tempUrl, oldFilename, UIDocumentBrowserImportMode.Move, (arg1, arg2) => {

                    if(arg2 != null)
                    {
                        Console.WriteLine(arg2);
                        File.Delete(tempUrl.Path);
                    }
                    else
                    {
                        NSFileManager.DefaultManager.Remove(oldFilename, out error);
                    }

                    oldFilename.StopAccessingSecurityScopedResource();
                });

                if (error != null)
                {
                    Console.WriteLine(error);
                }
            });

        }
    }
}