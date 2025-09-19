using Plugin.DTLS.Enums;
using Plugin.DTLS.Records;
using ServerShared.IO;
using ServerShared.Server;

namespace Plugin.DTLS.RecordProcessor;

public interface IRecordProcessor
{
    public DtlsRecordType Type { get; }
    public void Process(DtlsSession session, ref IRecord record, in BinaryReaderBig reader);
}
