using Plugin.DTLS.Enums;
using ServerShared.IO;
using System.Linq;
using System.Text;

namespace Plugin.DTLS.Extensions;

public struct ServerName() : IBigSerializable
{
    public short NameType;
    public byte[] NameData = [];

    public void Deserialize(BinaryReaderBig reader)
    {
        NameType = reader.ReadByte();
        uint len = reader.ReadUInt16();
        NameData = reader.ReadBytes((int)len);
    }

    public readonly void Serialize(BinaryWriterBig writer)
    {
        writer.Write(NameType);
        writer.Write((uint)NameData.Length);
        writer.Write(NameData);
    }

    public readonly override string ToString()
    {
        return $"{NameType}: {Encoding.UTF8.GetString(NameData)}";
    }
}

public struct ServerNameExtension() : IExtension
{
    public List<ServerName> Names = [];
    public readonly ExtensionType Type => ExtensionType.ServerName;

    public readonly void Deserialize(BinaryReaderBig reader)
    {
        Names.Clear();
        List<short> NameTypesSeen = [];
        long len = reader.ReadUInt16();
        long endLen = reader.BaseStream.Position + len;
        while (len != endLen)
        {
            ServerName name = reader.ReadSerializable<ServerName>();
            if (NameTypesSeen.Contains(name.NameType))
                throw new Exception("This name type already seen!");

            NameTypesSeen.Add(name.NameType);
            Names.Add(name);
            len = reader.BaseStream.Position;
        }
    }

    public readonly void Serialize(BinaryWriterBig writer)
    {
        using MemoryStream memoryStream = new();
        BinaryWriterBig writerBig = new(memoryStream);
        foreach (ServerName name in Names)
        {
            writerBig.WriteSerializable(name);
        }

        writer.Write((uint)memoryStream.Length);
        writer.Write(memoryStream.ToArray());
    }

    public readonly override string ToString()
    {
        return $"\nNames: {string.Join(", ", Names)}";
    }
}
