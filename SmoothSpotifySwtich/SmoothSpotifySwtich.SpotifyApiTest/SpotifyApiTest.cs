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
        SpotifyApi.SpotifyApi testee = new SpotifyApi.SpotifyApi("");


        [Fact]
        public async Task GetDevicesReturnsAllConnectedDevices()
        {
            // Act
            IEnumerable<ISpotifyDevice> devices = await this.testee.GetDevices();

            // Assert
            devices.Should().NotBeEmpty();
        }
    }
}