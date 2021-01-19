using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using SmoothSpotifySwtich.Interfaces;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;


namespace SmoothSpotifySwtich.SpotifyApi
{
    public class SpotifyApi
    {
        private static EmbedIOAuthServer authServer;
        readonly ManualResetEvent waitForAuthProcess = new ManualResetEvent(false);
        private SpotifyClient client;
        private readonly string cliendId;
        private readonly int callBackPort;
        private readonly string callBackUrl;

        public SpotifyApi()
        {
            this.cliendId = ":P";
            this.callBackPort = 6969;
            this.callBackUrl = $"http://localhost:{this.callBackPort}/callback";
            this.AskUserForPermission();
        }

        public async Task<IEnumerable<ISpotifyDevice>> GetDevices()
        {
            DeviceResponse availableDevices = await this.client.Player.GetAvailableDevices();

            return availableDevices.Devices.Select(this.GetISpotifyDeviceFromDevice);
        }

        public ISpotifyDevice GetPlayingISpotifyDevice()
        {
            return this.GetISpotifyDeviceFromDevice((this.client.Player.GetAvailableDevices().Result).Devices.First(d => d.IsActive));
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

        public async Task<bool> TrySwitchOutputTo(ISpotifyDevice device)
        {
            return await this.client.Player.TransferPlayback(new PlayerTransferPlaybackRequest(new List<string>{device.Id}));
        }

        public void AskUserForPermission()
        {
            this.waitForAuthProcess.Reset();
            authServer = new EmbedIOAuthServer(new Uri(this.callBackUrl), this.callBackPort);
            authServer.Start().GetAwaiter().GetResult();

            authServer.ImplictGrantReceived += this.OnImplicitGrantReceived;

            LoginRequest request = new LoginRequest(authServer.BaseUri, this.cliendId, LoginRequest.ResponseType.Token)
            {
                Scope = new List<string> { Scopes.UserModifyPlaybackState, Scopes.UserReadPlaybackState }
            };
            BrowserUtil.Open(request.ToUri());

            this.waitForAuthProcess.WaitOne();
        }

        private async Task OnImplicitGrantReceived(object sender, ImplictGrantResponse response)
        {
            await authServer.Stop();
            this.client = new SpotifyClient(response.AccessToken);
            this.waitForAuthProcess.Set();
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
