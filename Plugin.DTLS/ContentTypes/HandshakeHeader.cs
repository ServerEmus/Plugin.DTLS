using Plugin.DTLS.Enums;
using Plugin.DTLS.Handshake;
using ServerShared.IO;
using ServerShared.Types;
using System.Net;

namespace Plugin.DTLS.ContentTypes;

public class HandshakeHeader : IContent
{
    public bool IsFragmented => FragmentLength != Length || FragmentOffset != 0;
    public HandshakeType HandshakeType;
    public UInt24 Length;
    public ushort MessageSequence;
    public UInt24 FragmentOffset;
    public UInt24 FragmentLength;
    public byte[] Payload = [];
    public ContentType Type => ContentType.Handshake;

    public void Deserialize(EndiannessReader stream)
    {
        HandshakeType = (HandshakeType)stream.ReadByte();
        Length = stream.ReadUInt24();
        MessageSequence = stream.ReadUInt16();
        FragmentOffset = stream.ReadUInt24();
        FragmentLength = stream.ReadUInt24();
        Payload = stream.ReadBytes((int)(uint)FragmentLength);
    }

    public void Serialize(EndiannessWriter stream)
    {
        stream.Write((byte)HandshakeType);
        stream.WriteUInt24(Length);
        stream.Write(MessageSequence);
        stream.WriteUInt24(FragmentOffset);
        stream.WriteUInt24(FragmentLength);
        foreach (var item in Payload)
        {
            stream.Write(item);
        }
    }
    public void Reset()
    {
        Length = 0;
        MessageSequence = 0;
        FragmentOffset = 0;
        FragmentLength = 0;
    }

    public override string ToString()
    {
        return $"HandshakeType: {HandshakeType}, Len: {(uint)Length}, MessageSeq: {MessageSequence}, IsFragmented: {IsFragmented} (FO:{(uint)FragmentOffset} FL:{(uint)FragmentLength})";
    }
}
