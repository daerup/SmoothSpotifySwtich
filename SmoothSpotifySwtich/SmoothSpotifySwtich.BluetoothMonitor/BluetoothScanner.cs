using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmoothSpotifySwtich.Interfaces;
using Windows.Devices.Enumeration;
using InTheHand.Net.Sockets;

namespace SmoothSpotifySwtich.BluetoothApi
{
    public class BluetoothScanner
    {
        private readonly int numberOfDevices = 10;
        private BluetoothClient client;

        public BluetoothScanner()
        {
            this.client = new BluetoothClient();
        }
        public async Task<IEnumerable<IBluetoothDevice>> GetDevices()
        {
            return await Task.Run(() => this.client.DiscoverDevices(this.numberOfDevices, false, false, true, false).Select(this.GetIBluetoothDeviceFromDevice));
        }

        private IBluetoothDevice GetIBluetoothDeviceFromDevice(BluetoothDeviceInfo bluetoothDeviceInfo)
        {
            return new BluetoothDevice(bluetoothDeviceInfo.DeviceName, bluetoothDeviceInfo.DeviceAddress.ToString());
        }
    }

    internal class BluetoothDevice : IBluetoothDevice
    {
        public string Name { get; }
        public string MacAddress { get; }

        public BluetoothDevice(string name, string macAddress)
        {
            this.Name = name;
            this.MacAddress = macAddress;
        }
    }
}
