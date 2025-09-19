using ServerShared.IO;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Plugin.DTLS;

public class ProtocolVersion : IBigSerializable
{
    public static readonly ProtocolVersion DefaultVersion = new(1, 0);
    public static readonly ProtocolVersion Version1_0 = new(1, 0);
    public static readonly ProtocolVersion Version1_1 = new(1, 1);
    public static readonly ProtocolVersion Version1_2 = new(1, 2);
    public static readonly ProtocolVersion Version1_3 = new(1, 3);
    public byte Major { get; protected set; }
    public byte Minor { get; private set; }

    public ProtocolVersion()
    {

    }

    public ProtocolVersion(byte major, byte minor)
    {
        Major = major;
        Minor = minor;
    }

    public void Deserialize(BinaryReaderBig stream)
    {
        Major = (byte)(255 - stream.ReadByte());
        Minor = (byte)(255 - stream.ReadByte());
    }

    public void Serialize(BinaryWriterBig stream)
    {
        stream.Write((byte)(255 - Major));
        stream.Write((byte)(255 - Minor));
    }

    public override string ToString()
    {
        return $"{Major}.{Minor}";
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return Equals(obj as ProtocolVersion);
    }

    public bool Equals([NotNullWhen(true)] ProtocolVersion? obj)
    {
        return obj == this || (obj != null && Major == obj.Major && Minor == obj.Minor);
    }

    public override int GetHashCode()
    {
        return 0 | ((Major & 15) << 28) | ((Minor & 255) << 20);
    }

    public int CompareTo(ProtocolVersion? value)
    {
        if (value == this)
        {
            return 0;
        }
        if (value == null)
        {
            return 1;
        }
        if (Major == value.Major)
        {
            if (Minor == value.Minor)
            {
                return 0;
            }
            else
            {
                if (Minor <= value.Minor)
                {
                    return -1;
                }
                return 1;
            }
        }
        else
        {
            if (Major <= value.Major)
            {
                return -1;
            }
            return 1;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(ProtocolVersion? v1, ProtocolVersion? v2)
    {
        if (v2 == null)
        {
            return v1 == null;
        }
        return v2 == v1 || v2.Equals(v1);
    }

    public static bool operator !=(ProtocolVersion? v1, ProtocolVersion? v2)
    {
        return !(v1 == v2);
    }

    public static bool operator <(ProtocolVersion? v1, ProtocolVersion? v2)
    {
        if (v1 == null)
        {
            return v2 != null;
        }
        return v1.CompareTo(v2) < 0;
    }

    public static bool operator >(ProtocolVersion? v1, ProtocolVersion? v2)
    {
        return v2 < v1;
    }
}
