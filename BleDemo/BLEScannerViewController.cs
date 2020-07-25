using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UIKit;

namespace BleDemo
{
    public partial class BLEScannerViewController : UIViewController
    {
        private IAdapter adapter;
        private IBluetoothLE bluetoothLE;
        private IList<IDevice> deviceList = new List<IDevice>();

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
