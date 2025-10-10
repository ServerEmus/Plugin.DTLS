using Plugin.DTLS.Enums;
using Plugin.DTLS.Extensions;
using Serilog;
using ServerShared.IO;
using System.Net.Security;
using System.Reflection.PortableExecutable;

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
    public List<IExtension> Extensions = [];

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
        Extensions.Clear();
        if (stream.BaseStream.Position == stream.BaseStream.Length)
            return;
        long extensionLength = stream.ReadUInt16();
        if (extensionLength == 0)
            return;
        long endLen = stream.BaseStream.Position + extensionLength;
        while (extensionLength != endLen)
        {
            ExtensionType extensionType = (ExtensionType)stream.ReadUInt16();
            IExtension extension = MainStorage.GetExtension(extensionType);
            extension.Deserialize(stream);
            Extensions.Add(extension);
            extensionLength = stream.BaseStream.Position;
        }
    }

    public readonly void Serialize(BinaryWriterBig stream)
    {
        
    }

    public readonly override string ToString()
    {
        return $"v:{Version} Random: {Convert.ToBase64String(Random)} ({Random.Length})" +
            $", SessionID: {Convert.ToBase64String(SessionID)} ({SessionID.Length})" +
            $", Cookie: {Convert.ToBase64String(Cookie)} ({Cookie.Length})" +
            $", CipherSuites: {string.Join(", ", CipherSuites)}" +
            $", CompressionMethods: {string.Join(", ", CompressionMethods)}" +
            $", Extensions: {string.Join(", ", Extensions)}";
    }
}
