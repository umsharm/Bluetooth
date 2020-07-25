using System;
using Xamarin.Nordic.DFU.iOS;
using CoreBluetooth;
using Foundation;

namespace BleDemo.Nordic
{
    public class FirmwareUpdater : IDisposable
    {
        private readonly DFUServiceInitiator dfuServiceInitiator;
        private DFUServiceController dfuController;
        private DFUFirmware dfuFirmware;
        private DfuLogger dfuLogger;

        public FirmwareUpdater(CBPeripheral peripheral)
        {
            var cbManager = new CBCentralManager();
            dfuLogger = new DfuLogger();

            // what data need to send
            var path = NSBundle.MainBundle.PathForResource("softdevice_s140", ".zip");
            var urlPath = new NSUrl(path);
            dfuFirmware = new DFUFirmware(urlPath);

            dfuServiceInitiator = new DFUServiceInitiator(cbManager, peripheral);
            //_dfuServiceInitiator.SetPacketsReceiptNotificationsEnabled(true);
            //_dfuServiceInitiator.SetDisableNotification(true);

            dfuServiceInitiator.EnableUnsafeExperimentalButtonlessServiceInSecureDfu = true;
            dfuServiceInitiator.ProgressDelegate = new DfuServiceDelegateImplementation(dfuLogger);
            dfuServiceInitiator.Logger = dfuLogger;

            //_dfuServiceInitiator.SetZip(firmwareZipFile);
            dfuServiceInitiator.WithFirmware(dfuFirmware);

            //_dfuServiceInitiator.SetBinOrHex(DfuService.TypeApplication, firmwareZipFile);

        }

        public void Start()
        {
            dfuController = dfuServiceInitiator.Start();
        }

        public void Abort()
        {
            dfuController.Abort();
        }

        public void Pause()
        {
            dfuController.Pause();
        }

        public void Resume()
        {
            dfuController.Resume();
        }

        public void Dispose()
        {
        }
    }
}
