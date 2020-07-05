using System;
using System.Collections.Generic;
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

        public BleTableViewSource(IList<IDevice> deviceList, IAdapter adapter)
        {
            this.adapter = adapter;
            this.deviceList = deviceList;
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

            //cell.TextLabel.Text = string.Format("{0}", devices[indexPath.Row].Name);
            //cell.DetailTextLabel.Text = string.Format("{0}", devices[indexPath.Row].Id);

            cell.TextLabel.Text = string.Format("{0}", deviceList[indexPath.Row].Name);
            cell.DetailTextLabel.Text = string.Format("{0}", deviceList[indexPath.Row].Id);

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
                await adapter.ConnectToDeviceAsync(deviceList[indexPath.Row]);
            }
            catch(DeviceConnectionException e)
            {

            }
        }
    }

    class Device
    {
        public string Name => "some name";
        public string Id => Guid.NewGuid().ToString();
    }
}
