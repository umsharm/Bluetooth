﻿using System;
using System.Collections.Generic;
using BleDemo.Nordic;
using CoreBluetooth;
using Foundation;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using UIKit;



namespace BleDemo
{
    public class BleTableViewSource : UITableViewSource
    {
        IList<IDevice> deviceList;
        IList<Device> devices = new List<Device>();
        IAdapter adapter;
        INavigation navigation;

        public BleTableViewSource(IList<IDevice> deviceList, IAdapter adapter, INavigation navigation)
        {
            this.adapter = adapter;
            this.deviceList = deviceList;
            this.navigation = navigation;

            //devices.Add(new Device());
            //devices.Add(new Device());
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell("cell");
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Subtitle, "cell");
            }
            if (deviceList[indexPath.Row].State == Plugin.BLE.Abstractions.DeviceState.Connected )
            {
                cell.TextLabel.Text = string.Format("{0}", deviceList[indexPath.Row].Name);
                cell.DetailTextLabel.Text = "Connected";
                cell.DetailTextLabel.TextColor = UIColor.Green;
            }
            else
            {
                cell.TextLabel.Text = string.Format("{0}", deviceList[indexPath.Row].Name);
                cell.DetailTextLabel.Text = string.Format("{0}", deviceList[indexPath.Row].Id);
                cell.DetailTextLabel.TextColor = UIColor.Black;


            }

            //cell.TextLabel.Text = string.Format("{0}", devices[indexPath.Row].Name);
            //cell.DetailTextLabel.Text = string.Format("{0}", devices[indexPath.Row].Id);


            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            //if (deviceList == null || deviceList.Count == 0)
            //    return devices.Count;
            return deviceList == null ? 0 : deviceList.Count;
        }

        public async override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (adapter == null)
                return;

            try
            {
                if (deviceList[indexPath.Row].State == Plugin.BLE.Abstractions.DeviceState.Connected)
                {
                    var device = deviceList[indexPath.Row];
                    navigation.launchViewController(device);

                    //await adapter.DisconnectDeviceAsync(device);
                }
                else
                {
                    await adapter.ConnectToDeviceAsync(deviceList[indexPath.Row]);
                }

            }
            catch(DeviceConnectionException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }
    }

    class Device
    {
        public string Name => "some name";
        public string Id => Guid.NewGuid().ToString();
    }
}
