using Plugin.DTLS.ContentProcessor;
using Plugin.DTLS.ContentTypes;
using Plugin.DTLS.Enums;
using Plugin.DTLS.Handshake;
using Plugin.DTLS.HanshakeProccessor;
using Plugin.DTLS.RecordProcessor;
using Plugin.DTLS.Records;
using ServerShared.IO;

namespace Plugin.DTLS;

public static class MainStorage
{
    static readonly HashSet<IHandshake> Handshakes =
    [
        new ClientHello(),
        new HelloVerifyRequest(),
    ];

    static readonly List<IRecord> Records =
    [
        new DTLSPlaintext(),

    ];

    static readonly List<IContent> Contents =
    [
        new Alert(),
        new HandshakeHeader(),
    ];

    static readonly List<IContentProcessor> ContentProcessor =
    [
        new AlertProcessor(),
        new HandshakeProcessor(),
    ];

    static readonly List<IRecordProcessor> RecordProcessor =
    [
        new InvalidProcessor(),
        new PlaintextProcessor(),
    ];

    static readonly List<IHandshakeProcessor> HandshakeProcessors =
    [
        new ClientHelloProcessor(),
    ];

    public static IHandshake? GetHandshake(HandshakeType type)
    {
        foreach (var item in Handshakes)
        {
            if (item.Type == type)
                return item;
        }
        return null;
    }

    public static IRecord? GetRecord(DtlsRecordType type)
    {
        foreach (var item in Records)
        {
            if (item.Type == type)
                return item;
        }
        return null;
    }

    public static IContent? GetContent(ContentType type)
    {
        foreach (var item in Contents)
        {
            if (item.Type == type)
                return item;
        }
        return null;
    }

    public static void ProcessRecord(DtlsSession session, ref IRecord? record, in BinaryReaderBig reader)
    {
        if (record == null)
            return;
        foreach (var item in RecordProcessor)
        {
            if (item.Type == record.Type)
                item.Process(session, ref record, reader);
        }
    }

    public static void ProcessContent(DtlsSession session, ref DTLSPlaintext record, in IContent? content)
    {
        if (content == null)
            return;
        foreach (var item in ContentProcessor)
        {
            if (item.Type == content.Type)
                item.Process(session, ref record, content);
        }
    }

    public static void ProcessHandshake(DtlsSession session, ref DTLSPlaintext record, ref HandshakeHeader content, in IHandshake handshake)
    {
        if (content == null)
            return;
        foreach (var item in HandshakeProcessors)
        {
            if (item.Type == handshake.Type)
                item.Process(session, ref record, ref content, handshake);
        }
    }
}
