using Plugin.DTLS.Enums;
using Plugin.DTLS.Records;
using ServerShared.IO;

namespace Plugin.DTLS.RecordProcessor;

public class InvalidProcessor : IRecordProcessor
{
    public DtlsRecordType Type => DtlsRecordType.Invalid;

    public void Process(DtlsSession session, ref IRecord record, in BinaryReaderBig reader)
    {

    }
}
