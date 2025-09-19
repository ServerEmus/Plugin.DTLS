using Plugin.DTLS.Enums;
using ServerShared.IO;

namespace Plugin.DTLS.Handshake;

public interface IHandshake : IBigSerializable
{
    public HandshakeType Type { get; }
}
