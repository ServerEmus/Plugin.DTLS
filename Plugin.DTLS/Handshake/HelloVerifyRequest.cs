using Plugin.DTLS.Enums;
using ServerShared.IO;

namespace Plugin.DTLS.Handshake;

public struct HelloVerifyRequest() : IHandshake
{
    public readonly HandshakeType Type => HandshakeType.HelloVerifyRequest;
    public ProtocolVersion ServerVersion = ProtocolVersion.DefaultVersion;
    public byte[] Cookie = [];

    public void Deserialize(BinaryReaderBig stream)
    {
        stream.ReadSerializable(ref ServerVersion);
        byte len = stream.ReadByte();
        Cookie = stream.ReadBytes(len);
    }

    public readonly void Serialize(BinaryWriterBig stream)
    {
        stream.WriteSerializable(ServerVersion);
        stream.Write((byte)Cookie.Length);
        stream.Write(Cookie);
    }

    public readonly override string ToString()
    {
        return $"ServerVersion: {ServerVersion}" +
            $", Cookie: {Convert.ToBase64String(Cookie)} ({Cookie.Length})";
    }
}
