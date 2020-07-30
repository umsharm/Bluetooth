using System;
using Plugin.BLE.Abstractions.Contracts;

namespace BleDemo
{
    public interface INavigation
    {
        void launchViewController(IDevice device);

    }
}
