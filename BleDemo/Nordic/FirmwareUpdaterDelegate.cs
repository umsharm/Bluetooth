using System;
namespace BleDemo.Nordic
{
    interface FirmwareUpdaterDelegate
    {
        void Start();
        void Abort();
        void Pause();
        void Resume();
    }
}
