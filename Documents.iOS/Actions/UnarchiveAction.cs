using System;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using Documents.iOS.Managers;
using System.Linq;
using System.IO;
using CoreGraphics;
using Documents.iOS.Utilities;
using Documents.iOS.Enums;

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
            unarchiveExt.SupportedContentTypes = new string[] { "public.archive", "public.rar" };
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

            if (unarchiveLocationController.PopoverPresentationController != null)
            {
                unarchiveLocationController.PopoverPresentationController.SourceView = _view.View;
                unarchiveLocationController.PopoverPresentationController.SourceRect = new CGRect(_view.View.Bounds.GetMidX(), _view.View.Bounds.GetMidY(), 0, 0);
            }
            // Present Alert
            _view.PresentViewController(unarchiveLocationController, true, null);
        }

        private void Unarchive(UIAlertAction action, UnarchiveLocationEnum location, string filePath, string folderToSave = "")
        {
            try
            {
                var archiveManager = new ArchiveManager();
                var okAlertController = UIAlertController.Create("Action", $"Folder or files exists, what would you like to do?", UIAlertControllerStyle.Alert);
                var showAlert = false;
                var folderLocation = archiveManager.DetermineExtractLocation(filePath, location, folderToSave);
                if (location != UnarchiveLocationEnum.CurrentDirectory)
                {
                    var folderExists = CheckFolderLocation(folderLocation);
                    if (folderExists)
                    {
                        //Add Action
                        okAlertController.AddAction(UIAlertAction.Create("Overwrite folder", UIAlertActionStyle.Default, (sender) =>
                        {
                            var bounds = UIScreen.MainScreen.Bounds;
                            Func<Task> UnarchiveFunc = async () =>
                            {
                                var loadPop = new LoadingOverlay(bounds, "Deleting..."); // using field from step 2
                            _view.View.Add(loadPop);
                                Directory.Delete(folderLocation, true);
                                _view.View.SetNeedsDisplay();
                            //Needs to do this so folder is removed from display
                            await Task.Delay(2000);
                                loadPop.Hide();
                                PerformUnarchive(location, filePath, UnarchiveActionEnum.Overwrite, folderToSave);
                            };

                            UnarchiveFunc();
                        }));
                        showAlert = true;
                    }
                }
                if (archiveManager.CheckForArchiveFilesExists(filePath, location, folderToSave))
                {
                    showAlert = true;
                    okAlertController.AddAction(UIAlertAction.Create("Merge (overwrite existing files)", UIAlertActionStyle.Default, (sender) =>
                    {
                        PerformUnarchive(location, filePath, UnarchiveActionEnum.MergeWithOverwrite, folderToSave);
                    }));

                    okAlertController.AddAction(UIAlertAction.Create("Merge (keep existing files)", UIAlertActionStyle.Default, (sender) =>
                    {
                        PerformUnarchive(location, filePath, UnarchiveActionEnum.MergeWithoutOverwrite, folderToSave);
                    }));

                }

                if (showAlert)
                {
                    // Present Alert
                    okAlertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));
                    _view.PresentViewController(okAlertController, true, null);
                }
                else
                {
                    PerformUnarchive(location, filePath, UnarchiveActionEnum.Overwrite, folderToSave);
                }
            }
            catch (Exception ex)
            {
                var errorAlertController = UIAlertController.Create("Error", ex.Message, UIAlertControllerStyle.Alert);
                errorAlertController.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Cancel, null));
                _view.PresentViewController(errorAlertController, true, null);
            }
        }

        private void PerformUnarchive(UnarchiveLocationEnum location, string filePath, UnarchiveActionEnum action, string folderToSave = "")
        {
            var archiveManager = new ArchiveManager();

            var bounds = UIScreen.MainScreen.Bounds;

            // show the loading overlay on the UI thread using the correct orientation sizing
            var loadPop = new LoadingOverlay(bounds, "Extracting..."); // using field from step 2
            _view.View.Add(loadPop);

            archiveManager.UnarchiveFile(filePath, location, action, folderToSave);

            loadPop.Hide();
        }

        private bool CheckFolderLocation(string folderPath)
        {
            return Directory.Exists(folderPath);
        }
    }
}
