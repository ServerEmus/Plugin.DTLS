using Plugin.DTLS.ContentTypes;
using Plugin.DTLS.Enums;
using Plugin.DTLS.Records;
using Serilog;
using ServerShared.IO;

namespace Plugin.DTLS.RecordProcessor;

internal class PlaintextProcessor : IRecordProcessor
{
    public DtlsRecordType Type => DtlsRecordType.PlainText;

    public void Process(DtlsSession session, ref IRecord record, in BinaryReaderBig reader)
    {
        if (record is not DTLSPlaintext plaintext)
            return;
        reader.BaseStream.Position = 0;
        plaintext.Deserialize(reader);
        session.ProtocolVersion = plaintext.Version;
        Log.Information("Readed Record: {record}", plaintext);
        IContent? content = MainStorage.GetContent(plaintext.ContentType);
        if (content == null)
        {
            Log.Information("Content type not found: {ContentType}", plaintext.ContentType);
            return;
        }
        using BinaryReaderBig plainTextReader = new(new MemoryStream(plaintext.Fragment));
        content.Deserialize(plainTextReader);
        Log.Information("Readed Content: {content}", content);
        MainStorage.ProcessContent(session, ref plaintext, content);
        /*
        IContent? response = 
        if (response == null)
            return;
        Log.Information("Processing response Content: {content}", response);
        using MemoryStream ms = new();
        using BinaryWriterBig writer = new(ms);
        response.Serialize(writer);
        plaintext.Fragment = ms.ToArray();
        return plaintext;
        */
    }
}
