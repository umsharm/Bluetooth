using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UIKit;

namespace BleDemo
{
    public partial class BLEScannerViewController : UIViewController
    {
        private IAdapter adapter;
        private IBluetoothLE bluetoothLE;
        private IList<IDevice> deviceList = new List<IDevice>();

        public BLEScannerViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            bluetoothLE = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;
            adapter.ScanTimeout = 50000;
            adapter.ScanMode = ScanMode.Balanced;
            var state = bluetoothLE.State;

            uitableView.Source = new BleTableViewSource(deviceList, adapter);
            bluetoothLE.StateChanged += OnStateChanged;
            adapter.DeviceDiscovered += OnDeviceDiscovered;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            StartScanningBle();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            bluetoothLE.StateChanged -= OnStateChanged;
            adapter.DeviceDiscovered -= OnDeviceDiscovered;
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

        private void OnStateChanged(object sender, BluetoothStateChangedArgs stateChangedArgs)
        {
            Debug.WriteLine($"The bluetooth state changed to {stateChangedArgs.NewState}");
        }
    }
}