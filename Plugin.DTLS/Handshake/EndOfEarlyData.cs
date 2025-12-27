using Plugin.DTLS.Enums;
using ServerShared.IO;

namespace Plugin.DTLS.Handshake;

public struct EndOfEarlyData() : IHandshake
{
    public readonly HandshakeType Type => HandshakeType.EndOfEarlyData;

    public readonly void Deserialize(EndiannessReader reader)
    {
        
    }

    public readonly void Serialize(EndiannessWriter writer)
    {
        
    }
}
