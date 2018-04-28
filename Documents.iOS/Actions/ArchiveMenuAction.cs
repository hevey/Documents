using System;
using Foundation;
using UIKit;
using System.Collections.Generic;
using Documents.iOS.Enums;
using System.IO;
using Documents.iOS.Managers;
using System.Linq;
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
            var archiveController = UIAlertController.Create("Archive", "Archive Type", UIAlertControllerStyle.ActionSheet);

            archiveController.AddAction(UIAlertAction.Create("Zip", UIAlertActionStyle.Default, (actionParam) => Archive(files, ArchiveTypeEnum.Zip)));
            archiveController.AddAction(UIAlertAction.Create("Tar", UIAlertActionStyle.Default, (actionParam) => Archive(files, ArchiveTypeEnum.Tar)));
            archiveController.AddAction(UIAlertAction.Create("GZip", UIAlertActionStyle.Default, (actionParam) => Archive(files, ArchiveTypeEnum.GZip)));
            archiveController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));

            _view.PresentViewController(archiveController, true, null);
        }

        private IEnumerable<string> GetFiles(NSUrl[] urls)
        {
            var files = new List<string>();

            foreach (var url in urls)
            {
                if (File.Exists(url.Path))
                {
                    files.Add(url.Path);
                }
                else if (Directory.Exists(url.Path))
                {
                    foreach (var file in Directory.GetFiles(url.Path, "*", SearchOption.AllDirectories))
                    {
                        files.Add(file);
                    }
                }
            }

            return files;
        }
        private void Archive(IEnumerable<string> files, ArchiveTypeEnum type)
        {
            var finished = false;

            while (!finished)
            {

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
                        var dir = Directory.GetCurrentDirectory();
                        var addNumber = 0;
                        var ext = GetExtension(type);
                        var path = Path.Combine(dir, archiveFilename + ext);
                        while (File.Exists(path))
                        {
                            path = Path.Combine(dir, archiveFilename + " " + addNumber.ToString("XX") + ext);
                        }
                        finished = true;
                        var am = new ArchiveManager();
                        am.ArchiveFiles(files, type, path);

                    }
                }));
                okAlertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, (sender) =>
                {
                    finished = true;
                }));

                _view.PresentViewController(okAlertController, true, null);
            }
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
