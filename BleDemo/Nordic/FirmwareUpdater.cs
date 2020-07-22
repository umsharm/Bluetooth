using System;
using Xamarin.Nordic.DFU.iOS;
using CoreBluetooth;
using Foundation;

namespace BleDemo.Nordic
{
    public class FirmwareUpdater : IDisposable
    {
        private readonly DFUServiceInitiator _dfuServiceInitiator;
        private DFUServiceController _controller;
        private DFUFirmware _firmware;
        private DfuLogger dfuLogger;


        public FirmwareUpdater(CBPeripheral peripheral)
        {
            var cbManager = new CBCentralManager();
            dfuLogger = new DfuLogger();

            _dfuServiceInitiator = new DFUServiceInitiator(cbManager, peripheral);
            //_dfuServiceInitiator.SetPacketsReceiptNotificationsEnabled(true);
            //_dfuServiceInitiator.SetDisableNotification(true);
            // You need to tweak these parameters to match your device(SDK version and implementation details)
            //_dfuServiceInitiator.SetUnsafeExperimentalButtonlessServiceInSecureDfuEnabled(true);
            _dfuServiceInitiator.EnableUnsafeExperimentalButtonlessServiceInSecureDfu = true;
            _dfuServiceInitiator.ProgressDelegate = new DfuServiceDelegateImplementation(dfuLogger);

            //_dfuServiceInitiator.SetZip(firmwareZipFile);
            var path = NSBundle.MainBundle.PathForResource("", ".png");
            var url = new NSUrl(path);
            //_firmware.FileUrl
            _dfuServiceInitiator.WithFirmware(_firmware);

            //_dfuServiceInitiator.SetBinOrHex(DfuService.TypeApplication, firmwareZipFile);




        }

        public void Start()
        {
            _controller = _dfuServiceInitiator.Start();

        }

        public void Abort()
        {
            _controller.Abort();
        }

        public void Pause()
        {
            _controller.Pause();
        }

        public void Resume()
        {
            _controller.Resume();
        }

        public void Dispose()
        {
        }
    }
}
