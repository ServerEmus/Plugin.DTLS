using Plugin.DTLS.Enums;
using Plugin.DTLS.Extensions;
using ServerShared.IO;
using System.Net.Security;

namespace Plugin.DTLS.Handshake;

public struct ServerHello : IHandshake
{
    public ServerHello() { }
    public readonly HandshakeType Type => HandshakeType.ServerHello;
    public ProtocolVersion Version = ProtocolVersion.DefaultVersion;
    public byte[] Random = [];
    public byte[] SessionID = [];
    public TlsCipherSuite CipherSuite = TlsCipherSuite.TLS_NULL_WITH_NULL_NULL;
    public byte CompressionMethod = 0;
    public List<IExtension> Extensions = [];

    public void Deserialize(BinaryReaderBig reader)
    {
        reader.ReadSerializable(ref Version);
        Random = reader.ReadBytes(32);
        byte length = reader.ReadByte();
        if (length > 0)
        {
            SessionID = new byte[length];
            reader.Read(SessionID, 0, length);
        }
        CipherSuite = (TlsCipherSuite)reader.ReadUInt16();
        CompressionMethod = reader.ReadByte();
        Extensions.Clear();
        if (reader.BaseStream.Position == reader.BaseStream.Length)
            return;
        long extensionLength = reader.ReadUInt16();
        if (extensionLength == 0)
            return;
        long endLen = reader.BaseStream.Position + extensionLength;
        while (extensionLength != endLen)
        {
            ExtensionType extensionType = (ExtensionType)reader.ReadUInt16();
            IExtension extension = MainStorage.GetExtension(extensionType);
            extension.Deserialize(reader);
            Extensions.Add(extension);
            extensionLength = reader.BaseStream.Position;
        }
    }

    public readonly void Serialize(BinaryWriterBig writer)
    {
        
    }

}
