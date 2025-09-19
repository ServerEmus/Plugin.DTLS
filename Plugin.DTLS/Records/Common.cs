using Plugin.DTLS.Enums;
using ServerShared.IO;

namespace Plugin.DTLS.Records;

public class Common
{
    public DtlsRecordType RecordType { get; protected set; }
    public ContentType ContentType { get; protected set; }

    public static Common Parse(BinaryReaderBig reader)
    {
        byte b = reader.ReadByte();
        Common common = new()
        {
            RecordType = Extension.GetRecordType(b),
            ContentType = (ContentType)b,
        };
        return common;
    }
}
