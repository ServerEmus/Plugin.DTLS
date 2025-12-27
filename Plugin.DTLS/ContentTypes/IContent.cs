using Plugin.DTLS.Enums;
using ServerShared.IO;

namespace Plugin.DTLS.ContentTypes;

public interface IContent : ICustomSerializable
{
    public ContentType Type { get; }
}
