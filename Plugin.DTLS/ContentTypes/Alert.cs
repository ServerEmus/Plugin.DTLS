using Plugin.DTLS.Enums;
using ServerShared.IO;

namespace Plugin.DTLS.ContentTypes;

internal class Alert : IContent
{
    public AlertLevel Level;
    public AlertDescription Description;

    public ContentType Type => ContentType.Alert;

    public void Deserialize(BinaryReaderBig stream)
    {
        Level = (AlertLevel)stream.ReadByte();
        Description = (AlertDescription)stream.ReadByte();
    }

    public void Serialize(BinaryWriterBig stream)
    {
        stream.Write((byte)Level);
        stream.Write((byte)Description);
    }

    public override string ToString()
    {
        return $"Level: {Level}, Description: {Description}";
    }
}
