using ServerShared.IO;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Plugin.DTLS;

public struct ProtocolVersion : ICustomSerializable, IEquatable<ProtocolVersion>
{
    public static readonly ProtocolVersion DefaultVersion = new(1, 0);
    public static readonly ProtocolVersion Version1_0 = new(1, 0);
    public static readonly ProtocolVersion Version1_1 = new(1, 1);
    public static readonly ProtocolVersion Version1_2 = new(1, 2);
    public static readonly ProtocolVersion Version1_3 = new(1, 3);
    public byte Major;
    public byte Minor;

    public ProtocolVersion()
    {

    }

    public ProtocolVersion(byte major, byte minor)
    {
        Major = major;
        Minor = minor;
    }

    public void Deserialize(EndiannessReader stream)
    {
        Major = (byte)(255 - stream.ReadByte());
        Minor = (byte)(255 - stream.ReadByte());
    }

    public void Serialize(EndiannessWriter stream)
    {
        stream.Write((byte)(255 - Major));
        stream.Write((byte)(255 - Minor));
    }

    public readonly override string ToString()
    {
        return $"{Major}.{Minor}";
    }

    public readonly override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj == null) 
            return false;
        if (obj is not ProtocolVersion version)
            return false;
        return Equals(version);
    }

    public readonly bool Equals([NotNullWhen(true)] ProtocolVersion? obj)
    {
        if (!obj.HasValue)
            return false;
        return Major == obj.Value.Major && Minor == obj.Value.Minor;
    }

    public readonly bool Equals(ProtocolVersion other)
    {
        return Major == other.Major && Minor == other.Minor;
    }


    public readonly override int GetHashCode()
    {
        return 0 | ((Major & 15) << 28) | ((Minor & 255) << 20);
    }

    public readonly int CompareTo(ProtocolVersion? value)
    {
        if (value == this)
        {
            return 0;
        }
        if (!value.HasValue)
        {
            return 1;
        }
        if (Major == value.Value.Major)
        {
            if (Minor == value.Value.Minor)
            {
                return 0;
            }
            else
            {
                if (Minor <= value.Value.Minor)
                {
                    return -1;
                }
                return 1;
            }
        }
        else
        {
            if (Major <= value.Value.Major)
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
        if (!v1.HasValue)
        {
            return v2 != null;
        }
        return v1.Value.CompareTo(v2) < 0;
    }

    public static bool operator >(ProtocolVersion? v1, ProtocolVersion? v2)
    {
        return v2 < v1;
    }
}
