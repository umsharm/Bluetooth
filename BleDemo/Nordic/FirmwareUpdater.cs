using System;
using Xamarin.Nordic.DFU.iOS;
using CoreBluetooth;
using Foundation;

namespace BleDemo.Nordic
{
    public class FirmwareUpdater : FirmwareUpdaterDelegate, IDisposable
    {
        private DFUServiceInitiator dfuServiceInitiator;
        private DFUServiceController dfuController;
        private DFUFirmware dfuFirmware;
        private DfuLogger dfuLogger;
        private CBCentralManager cbCentralManager;

        public FirmwareUpdater(CBPeripheral peripheral)
        {
            cbCentralManager = new CBCentralManager();

            /**
             * Creates the DFU Firmware object from a Distribution packet (ZIP).
             * returns: The DFU firmware object or `nil` in case of an error.
            */
            var path = NSBundle.MainBundle.PathForResource("softdevice_s140", ".zip");
            var urlPath = new NSUrl("file://" + path);
            dfuFirmware = new DFUFirmware(urlPath);


            dfuServiceInitiator = new DFUServiceInitiator(cbCentralManager, peripheral);
            dfuServiceInitiator.PacketReceiptNotificationParameter = 12;

            dfuServiceInitiator.EnableUnsafeExperimentalButtonlessServiceInSecureDfu = true;
            dfuServiceInitiator.ProgressDelegate = new DfuServiceDelegateImplementation(dfuLogger);
            dfuServiceInitiator.Logger = new DfuLogger();

            dfuServiceInitiator.WithFirmware(dfuFirmware);
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
            if (dfuController != null)
            {
                dfuController.Dispose();
                dfuController = null;
            }

            if (dfuServiceInitiator != null)
            {
                dfuServiceInitiator.Dispose();
                dfuServiceInitiator = null;
            }

            if (dfuFirmware != null)
            {
                dfuFirmware.Dispose();
                dfuFirmware = null;
            }

            if (cbCentralManager != null)
            {
                cbCentralManager.Dispose();
                cbCentralManager = null;
            }
        }
    }
}
