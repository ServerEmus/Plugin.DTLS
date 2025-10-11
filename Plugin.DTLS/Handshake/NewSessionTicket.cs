using Plugin.DTLS.Enums;
using ServerShared.IO;

namespace Plugin.DTLS.Handshake;

public struct NewSessionTicket() : IHandshake
{
    public uint LifetimeHint;
    public byte[] Ticket = [];
    public readonly HandshakeType Type => HandshakeType.NewSessionTicket;

    public void Deserialize(BinaryReaderBig reader)
    {
        LifetimeHint = reader.ReadUInt32();
        ushort len = reader.ReadUInt16();
        Ticket = reader.ReadBytes(len);
    }

    public readonly void Serialize(BinaryWriterBig writer)
    {
        writer.Write(LifetimeHint);
        writer.Write((ushort)Ticket.Length);
        writer.Write(Ticket);
    }

    public readonly override string ToString()
    {
        return $"LifetimeHint: {LifetimeHint}" +
            $", Ticket: {Convert.ToBase64String(Ticket)} ({Ticket.Length})";
    }
}
