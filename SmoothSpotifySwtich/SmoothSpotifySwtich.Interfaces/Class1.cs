using System;

namespace SmoothSpotifySwtich.Interfaces
{
    public interface ISpotifyDevice
    {
        string Name { get; }
        string Id { get; }
        SpotifyDeviceType Type { get; }
    }

    public enum SpotifyDeviceType
    {
        Computer,
        Smartphone,
        Unknown
    }
}
