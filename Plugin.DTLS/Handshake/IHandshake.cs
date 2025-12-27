using Plugin.DTLS.Enums;
using ServerShared.IO;

namespace Plugin.DTLS.Handshake;

public interface IHandshake : ICustomSerializable
{
    public HandshakeType Type { get; }
}
