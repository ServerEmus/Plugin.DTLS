using Plugin.DTLS.Enums;
using Plugin.DTLS.Interfaces;
using ServerShared.IO;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Plugin.DTLS.Extensions;

public struct ServerName() : ICustomSerializable, IEqualityComparer<ServerName>
{
    public byte NameType;
    public byte[] NameData = [];
    public static ServerName Empty => new();
    public readonly uint Size => (uint)(sizeof(byte) + sizeof(ushort) + NameData.Length);

    public void Deserialize(EndiannessReader reader)
    {
        NameType = reader.ReadByte();
        ushort len = reader.ReadUInt16();
        NameData = reader.ReadBytes((int)len);
    }

    public readonly bool Equals(ServerName x, ServerName y)
    {
        return x.NameType == y.NameType && x.NameData.SequenceEqual(y.NameData);
    }

    public readonly int GetHashCode([DisallowNull] ServerName obj)
    {
        return obj.NameType.GetHashCode() + 5 + obj.NameData.GetHashCode();
    }

    public readonly void Serialize(EndiannessWriter writer)
    {
        writer.Write(NameType);
        writer.Write((ushort)NameData.Length);
        writer.Write(NameData);
    }

    public readonly override string ToString()
    {
        return $"{NameType}: {Encoding.UTF8.GetString(NameData)}";
    }
}

public struct ServerNameExtension() : IExtension, ISize
{
    public List<ServerName> Names = [];
    public readonly ExtensionType Type => ExtensionType.ServerName;
    public ushort ExtensionLength { get; set; }
    public readonly ushort Size => (ushort)(Names.Sum(static name => name.Size) + sizeof(ushort) + sizeof(ushort));
    public readonly void Deserialize(EndiannessReader reader)
    {
        Names.Clear();
        List<short> NameTypesSeen = [];
        uint lengthOfTheList = reader.ReadUInt16(); // list Len.
        uint readed = 0;
        while (lengthOfTheList != readed)
        {
            ServerName name = reader.ReadSerializable<ServerName>();
            if (NameTypesSeen.Contains(name.NameType))
                throw new Exception($"This name type already seen! {string.Join(", ", NameTypesSeen)} {name.NameType}");

            NameTypesSeen.Add(name.NameType);
            Names.Add(name);
            readed += name.Size;
        } 
    }

    public void Serialize(EndiannessWriter writer)
    {
        ushort len = (ushort)Names.Sum(static name => name.Size);
        ExtensionLength = (ushort)(len + sizeof(ushort));
        writer.Write(ExtensionLength);
        writer.Write(len);
        foreach (ServerName name in Names)
        {
            writer.WriteSerializable(name);
        }
    }

    public readonly override string ToString()
    {
        return $"\nNames: {string.Join(", ", Names)}";
    }
}
