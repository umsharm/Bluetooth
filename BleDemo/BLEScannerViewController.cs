using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UIKit;
using MobileCoreServices;
using Foundation;

namespace BleDemo
{
    public partial class BLEScannerViewController : UIViewController
    {
        private IAdapter adapter;
        private IBluetoothLE bluetoothLE;
        private IList<IDevice> deviceList = new List<IDevice>();
        private string[] allowedUTIs =  {
                    UTType.UTF8PlainText,
                    UTType.PlainText,
                    UTType.RTF,
                    UTType.PNG,
                    UTType.Text,
                    UTType.PDF,
                    UTType.ZipArchive
                };

        public BLEScannerViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            bluetoothLE = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;
            // todo need to figure out time 
            adapter.ScanTimeout = 50000;
            adapter.ScanMode = ScanMode.Balanced;
            var state = bluetoothLE.State;
            Debug.WriteLine($"Current device state :: {state}");

            uitableView.Source = new BleTableViewSource(deviceList, adapter);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            bluetoothLE.StateChanged += OnStateChanged;
            adapter.DeviceDiscovered += OnDeviceDiscovered;
            adapter.DeviceConnected += OnDeviceConnected;
            adapter.DeviceDisconnected += OnDeviceDisConnected;

            StartScanningBle();
        }

        private void Picker_WasCancelled(object sender, EventArgs e)
        {
            Console.WriteLine("Picker was Cancelled");
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            bluetoothLE.StateChanged -= OnStateChanged;
            adapter.DeviceDiscovered -= OnDeviceDiscovered;
            adapter.DeviceConnected -= OnDeviceConnected;
            adapter.DeviceDisconnected -= OnDeviceDisConnected;

        }

        private void StartScanningBle()
        {
            Task.Run(async () => await adapter.StartScanningForDevicesAsync());
        }

        private void OnDeviceDiscovered(object sender, DeviceEventArgs eventArgs)
        {
            deviceList.Add(eventArgs.Device);
            uitableView.ReloadData();
        }

        private async void OnDeviceConnected(object sender, DeviceEventArgs eventArgs)
        {
            var connectedDevice = eventArgs.Device;
            var services = await connectedDevice.GetServicesAsync();
            var service = services.FirstOrDefault();
            var characteristics = await service.GetCharacteristicsAsync();
            var characterstic = characteristics.FirstOrDefault();
            var bytes = await characterstic.ReadAsync();
            uitableView.ReloadData();
            var picker = new UIDocumentPickerViewController(allowedUTIs, UIDocumentPickerMode.Open);
            picker.WasCancelled += Picker_WasCancelled;
          
            picker.DidPickDocumentAtUrls += (object s, UIDocumentPickedAtUrlsEventArgs e) =>
            {
                Console.WriteLine("url = {0}", e.Urls[0].AbsoluteString);
                //bool success = await MoveFileToApp(didPickDocArgs.Url);  
                var success = true;
                string filename = e.Urls[0].LastPathComponent;
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
        }

        private  void OnDeviceDisConnected(object sender, DeviceEventArgs eventArgs)
        {
            uitableView.ReloadData();

        }

        private void OnStateChanged(object sender, BluetoothStateChangedArgs stateChangedArgs)
        {
            Debug.WriteLine($"The bluetooth state changed to {stateChangedArgs.NewState}");
        }
    }
}
