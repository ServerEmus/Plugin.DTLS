using Plugin.DTLS.Enums;
using Plugin.DTLS.Interfaces;
using ServerShared.IO;

namespace Plugin.DTLS.Extensions;

public struct SignatureAndHashAlgorithm() : IBigSerializable
{
    public static uint Size => sizeof(HashAlgorithm) + sizeof(HashAlgorithm);

    public HashAlgorithm Hash;
    public SignatureAlgorithm Signature;

    public void Deserialize(BinaryReaderBig reader)
    {
        Hash = (HashAlgorithm)reader.ReadByte();
        Signature = (SignatureAlgorithm)reader.ReadByte();
    }

    public readonly void Serialize(BinaryWriterBig writer)
    {
        writer.Write((byte)Hash);
        writer.Write((byte)Signature);
    }

    public readonly override string ToString()
    {
        return $"{Hash} {Signature}";
    }
}

public struct SignatureAlgorithmsExtension() : IExtension, ISize
{
    public List<SignatureAndHashAlgorithm> SignatureAlgorithms = [];
    public readonly ExtensionType Type => ExtensionType.SignatureAlgorithms;
    public ushort ExtensionLength { get; set; }
    public readonly ushort Size => (ushort)((SignatureAlgorithms.Count * SignatureAndHashAlgorithm.Size) + sizeof(ushort) + sizeof(ushort));

    public readonly void Deserialize(BinaryReaderBig reader)
    {
        SignatureAlgorithms.Clear();
        uint count = reader.ReadUInt16() / SignatureAndHashAlgorithm.Size;
        for (int i = 0; i < count; i++)
        {
            SignatureAndHashAlgorithm signatureAndHash = reader.ReadSerializable<SignatureAndHashAlgorithm>();
            SignatureAlgorithms.Add(signatureAndHash);
        }
    }

    public  void Serialize(BinaryWriterBig writer)
    {
        ushort len = (ushort)(SignatureAlgorithms.Count * SignatureAndHashAlgorithm.Size);
        ExtensionLength = (ushort)(len + sizeof(ushort));
        writer.Write(ExtensionLength);
        writer.Write(len);
        foreach (SignatureAndHashAlgorithm signatureAndHash in SignatureAlgorithms)
        {
            writer.WriteSerializable(signatureAndHash);
        }
    }

    public readonly override string ToString()
    {
        return $"\nSignatureAlgorithms: {string.Join(", ", SignatureAlgorithms)}";
    }
}
