using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using SmoothSpotifySwtich.BluetoothApi;
using SmoothSpotifySwtich.Interfaces;
using Xunit;

namespace SmoothSpotifySwtich.BluetoothApiTest
{
    public class BluetoothScannerTest
    {
        BluetoothScanner testee = new BluetoothScanner();

        [Fact]
        public async Task GetDevicesShouldReturnAllBluetoothDevices()
        {
            // Act
            List<IBluetoothDevice> spotifyDevices = (await testee.GetDevices()).ToList();

            // Assert
            spotifyDevices.Any().Should().BeTrue();
        }
    }
}
