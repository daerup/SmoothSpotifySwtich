using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SmoothSpotifySwtich.Interfaces;
using SpotifyAPI.Web;

namespace SmoothSpotifySwtich.SpotifyApi
{
    public class SpotifyApi
    {
        private SpotifyClient client;
        public SpotifyApi(string token)
        {
            this.client = new SpotifyClient(token);
        }

        public async Task<IEnumerable<ISpotifyDevice>> GetDevices()
        {
            DeviceResponse availableDevices = await this.client.Player.GetAvailableDevices();

            return availableDevices.Devices.Select(this.GetISpotifyDeviceFromDevice);
        }

        private ISpotifyDevice GetISpotifyDeviceFromDevice(Device device)
        {
            return new SpotifyDevice(device.Name, device.Id, this.GetDeviceTypeFromString(device.Type));
        }

        private SpotifyDeviceType GetDeviceTypeFromString(string deviceType)
        {
            switch (deviceType)
            {
                case "Smartphone": return SpotifyDeviceType.Smartphone;
                case "Computer": return SpotifyDeviceType.Computer;
                default: return SpotifyDeviceType.Unknown;
            }
        }
    }

    internal class SpotifyDevice : ISpotifyDevice
    {
        public SpotifyDevice(string name, string id, SpotifyDeviceType type)
        {
            this.Name = name;
            this.Id = id;
            this.Type = type;
        }

        public string Name { get; }
        public string Id { get; }
        public SpotifyDeviceType Type { get; }
    }
}
