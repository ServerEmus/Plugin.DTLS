using Plugin.DTLS.ContentTypes;
using Plugin.DTLS.Enums;
using Plugin.DTLS.Records;
using ServerShared.IO;
using ServerShared.Server;

namespace Plugin.DTLS.ContentProcessor;

public interface IContentProcessor
{
    public ContentType Type { get; }
    public void Process(DtlsSession session, ref DTLSPlaintext record, in IContent content);
}
