using Plugin.DTLS.Enums;
using ServerShared.IO;
using ServerShared.Types;

namespace Plugin.DTLS.Records;

public struct DTLSPlaintext : IRecord
{
    public DTLSPlaintext() { }
    public readonly DtlsRecordType Type =>  DtlsRecordType.PlainText;
    
    byte[] _Fragment = [];

    public ContentType ContentType { get; set; }
    public ProtocolVersion Version { get; set; } = ProtocolVersion.DefaultVersion;
    public ushort Epoch { get; set; }
    public Int48 SequenceNumber { get; set; }
    public ushort Length { get; private set; }
    public byte[] Fragment
    {
        readonly get => _Fragment;
        set
        {
            _Fragment = value;
            if (_Fragment != null)
            {
                Length = (ushort)_Fragment.Length;
            }
        }
    }


    public readonly void Serialize(BinaryWriterBig stream)
    {
        stream.Write((byte)ContentType);
        stream.WriteSerializable(Version);
        stream.Write(Epoch);
        stream.WriteInt48(SequenceNumber);
        stream.Write(Length);
        foreach (var item in _Fragment)
        {
            stream.Write(item);
        }
    }

    public void Deserialize(BinaryReaderBig stream)
    {
        ContentType = (ContentType)stream.ReadByte();
        Version = stream.ReadSerializable<ProtocolVersion>();
        Epoch = stream.ReadUInt16();
        SequenceNumber = stream.ReadInt48();
        Length = stream.ReadUInt16();
        _Fragment = stream.ReadBytes(Length);
    }

    public readonly override string ToString()
    {
        return $"Version: {Version}, Epoch: {Epoch}, Seq: {(int)SequenceNumber}, L: {Length}";
    }
}
