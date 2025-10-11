using Plugin.DTLS.Enums;
using ServerShared.IO;

namespace Plugin.DTLS.Handshake;

public struct EndOfEarlyData() : IHandshake
{
    public readonly HandshakeType Type => HandshakeType.EndOfEarlyData;

    public readonly void Deserialize(BinaryReaderBig reader)
    {
        
    }

    public readonly void Serialize(BinaryWriterBig writer)
    {
        
    }
}
