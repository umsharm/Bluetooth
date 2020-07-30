// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace BleDemo
{
    [Register ("BLEDetailViewViewController")]
    partial class BLEDetailViewViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel deviceId { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel deviceName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel status { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton updateFirmware { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton uploadFile { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (deviceId != null) {
                deviceId.Dispose ();
                deviceId = null;
            }

            if (deviceName != null) {
                deviceName.Dispose ();
                deviceName = null;
            }

            if (status != null) {
                status.Dispose ();
                status = null;
            }

            if (updateFirmware != null) {
                updateFirmware.Dispose ();
                updateFirmware = null;
            }

            if (uploadFile != null) {
                uploadFile.Dispose ();
                uploadFile = null;
            }
        }
    }
}