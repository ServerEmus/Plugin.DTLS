using Plugin.DTLS.Enums;
using ServerShared.IO;

namespace Plugin.DTLS.Handshake;

public struct ServerHelloDone() : IHandshake
{
    public readonly HandshakeType Type => HandshakeType.ServerHelloDone;

    public readonly void Deserialize(EndiannessReader reader)
    {
        
    }

    public readonly void Serialize(EndiannessWriter writer)
    {
        
    }
}
