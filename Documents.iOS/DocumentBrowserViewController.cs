using Foundation;
using System;
using System.IO;
using UIKit;
using System.IO.Compression;
using System.Linq;
using Documents.iOS.Managers;
namespace Documents.iOS
{
    public partial class DocumentBrowserViewController : UIDocumentBrowserViewController
    {
        public DocumentBrowserViewController (IntPtr handle) : base (handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            AllowsDocumentCreation = true;
            AllowsPickingMultipleItems = true;

            var renameWithExt = new UIDocumentBrowserAction("com.glennhevey.rename-with-extension", "Full Rename", UIDocumentBrowserActionAvailability.Menu, RenameWithExtensionAction);
            var unarchiveExt = new UIDocumentBrowserAction("com.glennhevey.unarchive", "Unarchive", UIDocumentBrowserActionAvailability.Menu, Unarchive);

            renameWithExt.SupportedContentTypes = new string[] { "public.item" };
            unarchiveExt.SupportedContentTypes = new string[] { "public.archive" };


            CustomActions = new UIDocumentBrowserAction[] { renameWithExt, unarchiveExt };

            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var directoryname = Path.Combine(documents, "Downloads");

            Directory.CreateDirectory(directoryname);

            //BrowserUserInterfaceStyle = UIDocumentBrowserUserInterfaceStyle.Dark;
            //View.TintColor = UIColor.LightTextColor;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        void RenameWithExtensionAction(NSUrl[] obj)
        {
            Console.WriteLine("Rename with Extension Tapped");
            //Create Alert
            var okAlertController = UIAlertController.Create("Full Rename", "Rename filename with extension", UIAlertControllerStyle.Alert);

            //Add Action
            okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

            // Present Alert
            PresentViewController(okAlertController, true, null);


        }

        void Unarchive(NSUrl[] obj)
        {
            var am = new ArchiveManager();
            var path = obj[0].Path;
            //Create Alert
            var unarchiveLocationController = UIAlertController.Create("Unarchive", "Unarchive file to folder", UIAlertControllerStyle.ActionSheet);

            unarchiveLocationController.AddAction(UIAlertAction.Create("Current Folder", UIAlertActionStyle.Default, (Action) => {

                am.UnarchiveFile(path, UnarchiveLocationEnum.CurrentDirectory);
            }));

            unarchiveLocationController.AddAction(UIAlertAction.Create($"Sub Folder ({Path.GetFileNameWithoutExtension(path)})", UIAlertActionStyle.Default, (Action) =>
            {
                var location = am.DetermineExtractLocation(path, UnarchiveLocationEnum.SubDirectoryWithArchiveName, "");

                if (Directory.Exists(location))
                {
                    var okAlertController = UIAlertController.Create("Folder Exists", $"{Path.GetFileNameWithoutExtension(path)} would you like to delete it?", UIAlertControllerStyle.Alert);

                    //Add Action
                    okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, (sender) => {
                        Directory.Delete(location, true);
                        am.UnarchiveFile(path, UnarchiveLocationEnum.SubDirectoryWithArchiveName);

                    }));

                    okAlertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));

                    // Present Alert
                    PresentViewController(okAlertController, true, null);

                }
                else
                {
                    am.UnarchiveFile(path, UnarchiveLocationEnum.SubDirectoryWithArchiveName);
                }
            }));
            unarchiveLocationController.AddAction(UIAlertAction.Create("Sub Folder (Custom)", UIAlertActionStyle.Default, (Action) => {
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


                        var location = am.DetermineExtractLocation(path, UnarchiveLocationEnum.SubDirectoryWithName, folderName);

                        if (Directory.Exists(location))
                        {
                            var oakAlertController = UIAlertController.Create("Folder Exists", $"{folderName} would you like to delete it?", UIAlertControllerStyle.Alert);

                            //Add Action
                            oakAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, (saender) =>
                            {
                                Directory.Delete(location, true);
                                am.UnarchiveFile(path, UnarchiveLocationEnum.SubDirectoryWithName, folderName);

                            }));

                            oakAlertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));

                            // Present Alert
                            PresentViewController(oakAlertController, true, null);

                        }
                        else
                        {
                            am.UnarchiveFile(path, UnarchiveLocationEnum.SubDirectoryWithName, folderName);
                        }
                    }
                    else
                    {
                        var oakAlertController = UIAlertController.Create("Error", $"Folder name can't be blank.", UIAlertControllerStyle.Alert);

                        //Add Action
                        oakAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

                        // Present Alert
                        PresentViewController(oakAlertController, true, null);
                    }

                }));

                okAlertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));

                // Present Alert
                PresentViewController(okAlertController, true, null);



            }));
            unarchiveLocationController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));

            // Present Alert
            PresentViewController(unarchiveLocationController, true, null);
        }



    }
}