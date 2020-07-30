using System;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using UIKit;
using MobileCoreServices;
using Foundation;
using System.IO;
using BleDemo.Nordic;
using CoreBluetooth;

namespace BleDemo
{

    public partial class BLEDetailViewViewController : UIViewController
    {
        public  IDevice device;
        private string[] allowedUTIs =  {
                UTType.UTF8PlainText,
                UTType.PlainText,
                UTType.RTF,
                UTType.PNG,
                UTType.Text,
                UTType.PDF,
                UTType.ZipArchive
            };

        public BLEDetailViewViewController() : base("BLEDetailViewViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            status.Text = device.State.ToString();
            deviceName.Text = device.Name;
        var picker = new UIDocumentPickerViewController(allowedUTIs, UIDocumentPickerMode.Open);
            picker.WasCancelled += Picker_WasCancelled;

            uploadFile.TouchUpInside += delegate
            {
                picker.DidPickDocumentAtUrls += (object s, UIDocumentPickedAtUrlsEventArgs e) =>
                {
                    Console.WriteLine("url = {0}", e.Urls[0].AbsoluteString);
                    //bool success = await MoveFileToApp(didPickDocArgs.Url);  
                    var success = true;
                    string filename = e.Urls[0].LastPathComponent;
                    string extension = Path.GetExtension(filename); ;
                    string msg = success ? string.Format("Successfully imported file '{0}'", filename) : string.Format("Failed to import file '{0}'", filename);
                    // Some invaild file url returns null  
                    NSData data = NSData.FromUrl(e.Urls[0]);
                    if (data != null)
                    {
                        byte[] dataBytes = new byte[data.Length];

                        System.Runtime.InteropServices.Marshal.Copy(data.Bytes, dataBytes, 0, Convert.ToInt32(data.Length));

                        for (int i = 0; i < dataBytes.Length; i++)
                        {
                            Console.WriteLine(dataBytes[i]);
                        }
                    }

                    Console.WriteLine(data + "Completed");

                    var alertController = UIAlertController.Create("import", msg, UIAlertControllerStyle.Alert);
                    var okButton = UIAlertAction.Create("OK", UIAlertActionStyle.Default, (obj) =>
                    {
                        alertController.DismissViewController(true, null);
                    });
                    alertController.AddAction(okButton);
                    PresentViewController(alertController, true, null);
                };
                PresentViewController(picker, true, null);

            };

            updateFirmware.TouchUpInside += delegate {
                FirmwareUpdaterDelegate firmwareUpdater = new FirmwareUpdater(device.NativeDevice as CBPeripheral);
                firmwareUpdater.Start();
            };

        }

                private void Picker_WasCancelled(object sender, EventArgs e)
        {
            Console.WriteLine("Picker was Cancelled");
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

