﻿// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Documents.iOS
{
    [Register ("ImportTableViewController")]
    partial class ImportTableViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem CancelButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem NewFolderButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem SaveButton { get; set; }

        [Action ("CancelButton_Activated:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void CancelButton_Activated (UIKit.UIBarButtonItem sender);

        [Action ("NewFolderButton_Activated:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void NewFolderButton_Activated (UIKit.UIBarButtonItem sender);

        [Action ("SaveButton_Activated:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void SaveButton_Activated (UIKit.UIBarButtonItem sender);

        void ReleaseDesignerOutlets ()
        {
            if (CancelButton != null) {
                CancelButton.Dispose ();
                CancelButton = null;
            }

            if (NewFolderButton != null) {
                NewFolderButton.Dispose ();
                NewFolderButton = null;
            }

            if (SaveButton != null) {
                SaveButton.Dispose ();
                SaveButton = null;
            }
        }
    }
}