using Plugin.DTLS.Enums;
using Plugin.DTLS.Extensions;
using Plugin.DTLS.Interfaces;
using ServerShared.IO;
using System.Net.Security;

namespace Plugin.DTLS.Handshake;

public struct ServerHello() : IHandshake
{
    public readonly HandshakeType Type => HandshakeType.ServerHello;
    public ProtocolVersion Version = ProtocolVersion.DefaultVersion;
    public byte[] Random = [];
    public byte[] SessionID = [];
    public TlsCipherSuite CipherSuite = TlsCipherSuite.TLS_NULL_WITH_NULL_NULL;
    public byte CompressionMethod = 0;
    public List<IExtension> Extensions = [];

    public void Deserialize(EndiannessReader reader)
    {
        Version = reader.ReadSerializable<ProtocolVersion>();
        Random = reader.ReadBytes(32);

        byte length = reader.ReadByte();
        SessionID = reader.ReadBytes(length);

        CipherSuite = (TlsCipherSuite)reader.ReadUInt16();
        CompressionMethod = reader.ReadByte();

        Extensions.Clear();
        if (reader.BaseStream.Position == reader.BaseStream.Length)
            return;
        ushort ExtensionsLength = reader.ReadUInt16();
        if (ExtensionsLength == 0)
            return;

        long endLen = reader.BaseStream.Position + ExtensionsLength;
        while (endLen != reader.BaseStream.Position)
        {
            ExtensionType extensionType = (ExtensionType)reader.ReadUInt16();
            ushort extensionLength = reader.ReadUInt16();
            IExtension extension = MainStorage.GetExtension(extensionType);
            extension.ExtensionLength = extensionLength;
            extension.Deserialize(reader);
            Extensions.Add(extension);
        }
    }

    public readonly void Serialize(EndiannessWriter writer)
    {
        writer.WriteSerializable(Version);
        writer.Write(Random);
        writer.Write((byte)SessionID.Length);
        writer.Write(SessionID);
        writer.Write((ushort)CipherSuite);
        writer.Write(CompressionMethod);

        if (Extensions.Count == 0)
        {
            writer.Write((ushort)0);
            return;
        }
        ushort ExtensionsLength = (ushort)Extensions.Sum(static ext =>
        {
            if (ext is ISize size)
                return size.Size + sizeof(ushort);
            return ext.ExtensionLength + sizeof(ushort);
        });
        writer.Write(ExtensionsLength);

        foreach (var extension in Extensions)
        {
            writer.Write((ushort)extension.Type);
            extension.Serialize(writer);
        }
    }

    public readonly override string ToString()
    {
        return $"({Type}) " +
            $"v:{Version} Random: {Convert.ToBase64String(Random)} ({Random.Length})" +
            $", SessionID: {Convert.ToBase64String(SessionID)} ({SessionID.Length})" +
            $", CipherSuite: {CipherSuite}" +
            $", CompressionMethod: {CompressionMethod}" +
            $", Extensions: {string.Join(", ", Extensions)}";
    }
}
