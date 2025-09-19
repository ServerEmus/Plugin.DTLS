using Plugin.DTLS.ContentTypes;
using Plugin.DTLS.Enums;
using Plugin.DTLS.Records;
using ServerShared.IO;

namespace Plugin.DTLS.ContentProcessor;

public class AlertProcessor : IContentProcessor
{
    public ContentType Type => ContentType.Alert;

    public void Process(DtlsSession session, ref DTLSPlaintext record, in IContent content)
    {

    }
}
