using System;
using Foundation;
using UIKit;
using Documents.iOS.Managers;
using System.Linq;
using System.IO;
namespace Documents.iOS.Actions
{
    public class UnarchiveAction : ICustomAction
    {
        private UIViewController _view;
        public UnarchiveAction(UIViewController view)
        {
            _view = view;
        }

        public UIDocumentBrowserAction SetupAction()
        {
            var unarchiveExt = new UIDocumentBrowserAction("com.glennhevey.unarchive", "Extract", UIDocumentBrowserActionAvailability.Menu, Action);
            unarchiveExt.SupportedContentTypes = new string[] { "public.archive" };
            return unarchiveExt;
        }

        public void Action(NSUrl[] obj)
        {
            var am = new ArchiveManager();
            var path = obj[0].Path;
            //Create Alert
            var unarchiveLocationController = UIAlertController.Create("Extract", "Extract file to folder", UIAlertControllerStyle.ActionSheet);

            unarchiveLocationController.AddAction(UIAlertAction.Create("Current Folder", UIAlertActionStyle.Default, (actionparam) => Unarchive(actionparam, UnarchiveLocationEnum.CurrentDirectory, path)));
            unarchiveLocationController.AddAction(UIAlertAction.Create($"Sub Folder ({Path.GetFileNameWithoutExtension(path)})", UIAlertActionStyle.Default, (actionparam) => Unarchive(actionparam, UnarchiveLocationEnum.SubDirectoryWithArchiveName, path)));
            unarchiveLocationController.AddAction(UIAlertAction.Create("Sub Folder (Custom)", UIAlertActionStyle.Default, (actionparam) => {
                var folderName = "";
                var okAlertController = UIAlertController.Create("Folder Name", $"Please enter the folder to use?", UIAlertControllerStyle.Alert);
                okAlertController.AddTextField(textField => {
                    // If you need to customize the text field, you can do so here.
                });
                //Add Action
                okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, (sender) => {
                    folderName = okAlertController.TextFields.First().Text;
                    if (folderName != "")
                    {
                        Unarchive(actionparam, UnarchiveLocationEnum.SubDirectoryWithName, path, folderName);
                    }
                    else
                    {
                        var oakAlertController = UIAlertController.Create("Error", $"Folder name can't be blank.", UIAlertControllerStyle.Alert);

                        //Add Action
                        oakAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

                        // Present Alert
                        _view.PresentViewController(oakAlertController, true, null);
                    }

                }));

                okAlertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));
                // Present Alert
                _view.PresentViewController(okAlertController, true, null);



            }));
            unarchiveLocationController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));

            unarchiveLocationController.PopoverPresentationController.SourceView = _view.View;

            // Present Alert
            _view.PresentViewController(unarchiveLocationController, true, null);
        }

        private void Unarchive(UIAlertAction action, UnarchiveLocationEnum location, string filePath, string folderToSave = "")
        {
            var archiveManager = new ArchiveManager();
            if (location != UnarchiveLocationEnum.CurrentDirectory)
            {
                var completed = CheckFolderLocation(location, filePath, archiveManager, folderToSave);
                if(!completed)
                {
                    archiveManager.UnarchiveFile(filePath, location, folderToSave);
                }
            }
            else
            {
                archiveManager.UnarchiveFile(filePath, location, folderToSave);
            }
        }

        private bool CheckFolderLocation(UnarchiveLocationEnum location, string filePath, ArchiveManager archiveManager, string folderToSave = "")
        {
            var completed = false;
            var folderLocation = archiveManager.DetermineExtractLocation(filePath, location, folderToSave);
            if (Directory.Exists(folderLocation))
            {
                completed = true;
                var okAlertController = UIAlertController.Create("Folder Exists", $"{Path.GetFileNameWithoutExtension(folderLocation)} would you like to delete it?", UIAlertControllerStyle.Alert);

                //Add Action
                okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, (sender) =>
                {
                    Directory.Delete(folderLocation, true);
                    archiveManager.UnarchiveFile(filePath, location, folderToSave);

                }));

                okAlertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));

                // Present Alert
                _view.PresentViewController(okAlertController, true, null);

            }
            return completed;
        }
    }
}
