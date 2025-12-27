using Plugin.DTLS.Enums;
using ServerShared.IO;

namespace Plugin.DTLS.Handshake;

public struct HelloVerifyRequest() : IHandshake
{
    public readonly HandshakeType Type => HandshakeType.HelloVerifyRequest;
    public ProtocolVersion ServerVersion = ProtocolVersion.DefaultVersion;
    public byte[] Cookie = [];

    public void Deserialize(EndiannessReader stream)
    {
        ServerVersion = stream.ReadSerializable<ProtocolVersion>();
        byte len = stream.ReadByte();
        Cookie = stream.ReadBytes(len);
    }

    public readonly void Serialize(EndiannessWriter stream)
    {
        stream.WriteSerializable(ServerVersion);
        stream.Write((byte)Cookie.Length);
        stream.Write(Cookie);
    }

    public readonly override string ToString()
    {
        return $"({Type}) " +
               $"ServerVersion: {ServerVersion}" +
               $", Cookie: {Convert.ToBase64String(Cookie)} ({Cookie.Length})";
    }
}
