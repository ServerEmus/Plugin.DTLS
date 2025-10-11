using Plugin.DTLS.Enums;
using ServerShared.IO;

namespace Plugin.DTLS.Extensions;

public interface IExtension : IBigSerializable
{
    public ExtensionType Type { get; }

    public ushort ExtensionLength { get; set; }
}
