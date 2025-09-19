using Plugin.DTLS.Enums;
using ServerShared.IO;

namespace Plugin.DTLS.Records;

public interface IRecord : IBigSerializable
{
    public DtlsRecordType Type { get; }
    public ContentType ContentType { get; set; }
}
