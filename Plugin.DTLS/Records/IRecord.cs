using Plugin.DTLS.Enums;
using ServerShared.IO;

namespace Plugin.DTLS.Records;

public interface IRecord : ICustomSerializable
{
    public DtlsRecordType Type { get; }
    public ContentType ContentType { get; set; }
}
