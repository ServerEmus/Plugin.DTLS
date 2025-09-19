using Plugin.DTLS.Enums;
using ServerShared.IO;

namespace Plugin.DTLS.ContentTypes;

public interface IContent : IBigSerializable
{
    public ContentType Type { get; }
}
