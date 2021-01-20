using System;

namespace SmoothSpotifySwtich.Interfaces
{
    public interface IBluetoothDevice
    {
        string Name { get; }
        string MacAddress { get; }
    }
}
