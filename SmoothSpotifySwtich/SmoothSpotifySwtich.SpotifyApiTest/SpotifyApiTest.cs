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
    }
}