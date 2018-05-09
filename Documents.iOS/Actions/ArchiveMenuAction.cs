using System;
using Foundation;
using UIKit;
using System.Collections.Generic;
using Documents.iOS.Enums;
using System.IO;
using Documents.iOS.Managers;
using System.Linq;
using Documents.iOS.Utilities;
using CoreGraphics;
namespace Documents.iOS.Actions
{
    public class ArchiveMenuAction : ICustomAction
    {
        private UIViewController _view;

        public ArchiveMenuAction(UIViewController view)
        {
            _view = view;
        }

        public void Action(NSUrl[] obj)
        {
            var files = GetFiles(obj);
            if (files.Count() > 1)
            {
                var errorController =
                    UIAlertController.Create("Error", "Unable to archive multiple files, please select one file or folder.", UIAlertControllerStyle.Alert);
                errorController.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Cancel, null));
                _view.PresentViewController(errorController, true, null);

            }
            else
            {


                var archiveController =
                    UIAlertController.Create("Archive", "Archive Type", UIAlertControllerStyle.ActionSheet);

                archiveController.AddAction(UIAlertAction.Create("Zip", UIAlertActionStyle.Default,
                    (actionParam) => Archive(files, ArchiveTypeEnum.Zip)));
                //archiveController.AddAction(UIAlertAction.Create("Tar", UIAlertActionStyle.Default,
                //    (actionParam) => Archive(files, ArchiveTypeEnum.Tar)));
                //archiveController.AddAction(UIAlertAction.Create("GZip", UIAlertActionStyle.Default,
                //    (actionParam) => Archive(files, ArchiveTypeEnum.GZip)));
                archiveController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));

                if (archiveController.PopoverPresentationController != null)
                {
                    archiveController.PopoverPresentationController.SourceView = _view.View;
					archiveController.PopoverPresentationController.SourceRect = new CGRect(_view.View.Bounds.Right - 147.5, _view.View.Bounds.Top + 55, 0, 0);
                }
                
                _view.PresentViewController(archiveController, true, null);
            }
        }

        private IEnumerable<string> GetFiles(NSUrl[] urls)
        {
            var files = new List<string>();

            foreach (var url in urls)
            {
                files.Add(url.Path);
            }

            return files;
        }
        private void Archive(IEnumerable<string> files, ArchiveTypeEnum type)
        {
            if(!files.Any())
			{
				return;
			}
            var okAlertController = UIAlertController.Create("Archive Filename", $"Please enter the filename to use?", UIAlertControllerStyle.Alert);
            okAlertController.AddTextField(textField =>
            {
                // If you need to customize the text field, you can do so here.
            });
            //Add Action
            okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, (sender) =>
            {
                var archiveFilename = okAlertController.TextFields.First().Text;
                if (archiveFilename != "")
                {
					var bounds = UIScreen.MainScreen.Bounds;

                    // show the loading overlay on the UI thread using the correct orientation sizing
                    var loadPop = new LoadingOverlay(bounds, "Archiving..."); // using field from step 2
                    _view.View.Add(loadPop);

                   
					var dir = Directory.GetParent(files.FirstOrDefault()).FullName;

                    var addNumber = 0;
                    var ext = GetExtension(type);
                    var path = Path.Combine(dir, archiveFilename + ext);
                    while (File.Exists(path))
                    {
						addNumber++;
                        path = Path.Combine(dir, archiveFilename + " " + addNumber.ToString("D2") + ext);
                    }
                    var am = new ArchiveManager();
                    am.ArchiveFiles(files, type, path);
					loadPop.Hide();
                
                }
            }));
            okAlertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));

            _view.PresentViewController(okAlertController, true, null);
            
        }

        private object GetExtension(ArchiveTypeEnum type)
        {
            switch (type)
            {
                case ArchiveTypeEnum.GZip:
                    return ".gzip";
                case ArchiveTypeEnum.Tar:
                    return ".tar";
                case ArchiveTypeEnum.Zip:
                    return ".zip";
            }
            return "";
        }

        public UIDocumentBrowserAction SetupAction()
        {
            var archiveExt = new UIDocumentBrowserAction("com.glennhevey.archive", "Archive", UIDocumentBrowserActionAvailability.Menu, Action);
            archiveExt.SupportedContentTypes = new string[] { "public.item" };
            return archiveExt;
        }
    }
}
