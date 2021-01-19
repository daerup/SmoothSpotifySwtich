using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using SmoothSpotifySwtich.Interfaces;
using Xunit;
namespace SmoothSpotifySwtich.SpotifyApiTest
{
    public class SpotifyApiTest
    {
        /// <summary>
        ///  Get Token from Here: https://developer.spotify.com/console/get-users-available-devices/
        /// user-read-playback-state & user-modify-playback-state are necessary 
        /// </summary>
        SpotifyApi.SpotifyApi testee = new SpotifyApi.SpotifyApi("look at summery");


        [Fact]
        public async Task GetDevicesShouldReturnAllConnectedDevices()
        {
            // Act
            IEnumerable<ISpotifyDevice> devices = await this.testee.GetDevices();

            // Assert
            devices.Should().NotBeEmpty();
        }

        [Fact]
        public async Task SwitchingOutputDeviceDoesSomething()
        {
            // Arrange
            var playingDevice = this.testee.GetPlayingISpotifyDevice();
            List<ISpotifyDevice> spotifyDevices = (await this.testee.GetDevices()).ToList();
            ISpotifyDevice deviceToSwitchTo = spotifyDevices.First(d => d.Id != playingDevice.Id);

            // Act
            bool switchingStatus = await this.testee.TrySwitchOutputTo(deviceToSwitchTo);

            // Assert
            switchingStatus.Should().BeTrue();
            playingDevice.Id.Should().NotBeEquivalentTo(deviceToSwitchTo.Id);
        }

        [Fact]
        public async Task AtLeastTwoDevicesAreConnected()
        {
            // Act
            List<ISpotifyDevice> spotifyDevices = (await this.testee.GetDevices()).ToList();

            // Arrange
            spotifyDevices.Count.Should().BeGreaterOrEqualTo(2, "but only contains {0}", spotifyDevices.First().Name);
        }
    }
}