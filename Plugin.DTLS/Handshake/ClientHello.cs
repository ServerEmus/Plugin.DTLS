using Plugin.DTLS.Enums;
using Plugin.DTLS.Extensions;
using Plugin.DTLS.Interfaces;
using ServerShared.IO;
using System.Net.Security;

namespace Plugin.DTLS.Handshake;

public struct ClientHello() : IHandshake
{
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
        SessionID = stream.ReadBytes(length);

        length = stream.ReadByte();
        Cookie = stream.ReadBytes(length);

        ushort cipherSuitesLength = (ushort)(stream.ReadUInt16() / sizeof(TlsCipherSuite));
        if (cipherSuitesLength > 0)
        {
            CipherSuites = new TlsCipherSuite[cipherSuitesLength];
            for (ushort index = 0; index < cipherSuitesLength; index++)
            {
                CipherSuites[index] = (TlsCipherSuite)stream.ReadUInt16();
            }
        }
        length = stream.ReadByte();
        CompressionMethods = stream.ReadBytes(length);

        Extensions.Clear();
        if (stream.BaseStream.Position == stream.BaseStream.Length)
            return;
        ushort ExtensionsLength = stream.ReadUInt16();
        if (ExtensionsLength == 0)
            return;

        long endLen = stream.BaseStream.Position + ExtensionsLength;
        while (endLen != stream.BaseStream.Position)
        {
            ExtensionType extensionType = (ExtensionType)stream.ReadUInt16();
            ushort extensionLength = stream.ReadUInt16();
            IExtension extension = MainStorage.GetExtension(extensionType);
            extension.ExtensionLength = extensionLength;
            extension.Deserialize(stream);
            Extensions.Add(extension);
        }
    }

    public readonly void Serialize(BinaryWriterBig writer)
    {
        writer.WriteSerializable(Version);
        writer.Write(Random);
        writer.Write((byte)SessionID.Length);
        writer.Write(SessionID);
        writer.Write((byte)Cookie.Length);
        writer.Write(Cookie);
        writer.Write((ushort)(CipherSuites.Length * sizeof(TlsCipherSuite)));
        foreach (TlsCipherSuite cipherSuite in CipherSuites)
        {
            writer.Write((ushort)cipherSuite);
        }
        writer.Write((byte)CompressionMethods.Length);
        writer.Write(CompressionMethods);

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
        return $"v:{Version} Random: {Convert.ToBase64String(Random)} ({Random.Length})" +
            $", SessionID: {Convert.ToBase64String(SessionID)} ({SessionID.Length})" +
            $", Cookie: {Convert.ToBase64String(Cookie)} ({Cookie.Length})" +
            $", CipherSuites: {string.Join(", ", CipherSuites)}" +
            $", CompressionMethods: {string.Join(", ", CompressionMethods)}" +
            $", Extensions: {string.Join(", ", Extensions)}";
    }
}
