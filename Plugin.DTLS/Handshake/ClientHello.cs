using Plugin.DTLS.Enums;
using ServerShared.IO;
using System.Net.Security;
namespace Plugin.DTLS.Handshake;

public struct ClientHello : IHandshake
{
    public ClientHello() { }
    public readonly HandshakeType Type => HandshakeType.ClientHello;
    public ProtocolVersion Version = ProtocolVersion.DefaultVersion;
    public byte[] Random = [];
    public byte[] SessionID = [];
    public byte[] Cookie = [];
    public TlsCipherSuite[] CipherSuites = [];
    public byte[] CompressionMethods = [];
    // Extensions!!

    public void Deserialize(BinaryReaderBig stream)
    {
        stream.ReadSerializable(ref Version);
        Random = stream.ReadBytes(32);
        byte length = stream.ReadByte();
        if (length > 0)
        {
            SessionID = new byte[length];
            stream.Read(SessionID, 0, length);
        }

        length = stream.ReadByte();
        if (length > 0)
        {
            Cookie = new byte[length];
            stream.Read(Cookie, 0, length);
        }
        ushort cipherSuitesLength = (ushort)(stream.ReadUInt16() / 2);
        if (cipherSuitesLength > 0)
        {
            CipherSuites = new TlsCipherSuite[cipherSuitesLength];
            for (uint index = 0; index < cipherSuitesLength; index++)
            {
                CipherSuites[index] = (TlsCipherSuite)stream.ReadUInt16();
            }
        }
        length = stream.ReadByte();
        if (length > 0)
        {
            CompressionMethods = new byte[length];
            stream.Read(CompressionMethods, 0, length);
        }
    }

    public void Serialize(BinaryWriterBig stream)
    {
        
    }

    public readonly override string ToString()
    {
        return $"v:{Version} Random: {Convert.ToBase64String(Random)} ({Random.Length})" +
            $", SessionID: {Convert.ToBase64String(SessionID)} ({SessionID.Length})" +
            $", Cookie: {Convert.ToBase64String(Cookie)} ({Cookie.Length})" +
            $", CipherSuites: {string.Join(", ", CipherSuites)}" +
            $", CompressionMethods: {string.Join(", ", CompressionMethods)}";
    }
}
