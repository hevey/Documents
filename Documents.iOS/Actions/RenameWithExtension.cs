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
        private UIViewController _view;

        public RenameWithExtension(UIViewController view)
        {
            _view = view;
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
                else if (!newName.Contains("."))
                {
                    var blankExtensionAlert = UIAlertController.Create("Extension Missing",
                        $"No Extension was given. Do you want to use the previous extension ({previousExtension}) or leave it blank?",
                        UIAlertControllerStyle.ActionSheet);

                    blankExtensionAlert.AddAction(UIAlertAction.Create("Blank", UIAlertActionStyle.Default,
                        (sen) => { Console.WriteLine($"New Name: {newName}"); }));

                    blankExtensionAlert.AddAction(UIAlertAction.Create($"{previousExtension}",
                        UIAlertActionStyle.Default, (sen) =>
                        {
                            newName += $".{previousExtension}";
                        }));

                    if (blankExtensionAlert.PopoverPresentationController != null)
                    {
                        //TODO: Fix where this appears on screen. Should sit under the filename
                        blankExtensionAlert.PopoverPresentationController.SourceView = _view.View;
                        blankExtensionAlert.PopoverPresentationController.PermittedArrowDirections =
                            UIPopoverArrowDirection.Up;
                        blankExtensionAlert.PopoverPresentationController.SourceRect =
                            new CGRect(_view.View.Bounds.GetMidX(), _view.View.Bounds.GetMidY(), 0, 0);
                    }


                    _view.PresentViewController(blankExtensionAlert, true, null);
                }
                
                Console.WriteLine($"Object Path: {obj[0].RelativePath}");
                File.Move(obj[0].RelativePath, $"{obj[0].RelativePath.Substring(obj[0].RelativePath.LastIndexOf('/'))}{newName}");
            }));

            renameAlertContoller.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, (sender) => { }));


            _view.PresentViewController(renameAlertContoller, true, null);
        }

        public UIDocumentBrowserAction SetupAction()
        {
            var renameWithExt = new UIDocumentBrowserAction("com.glennhevey.rename-with-extension", "Full Rename",
                UIDocumentBrowserActionAvailability.Menu, Action);

            renameWithExt.SupportedContentTypes = new string[] {"public.item"};

            return renameWithExt;
        }
    }
}