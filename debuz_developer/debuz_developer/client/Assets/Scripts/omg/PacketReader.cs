using System;
using System.Collections.Generic;
using System.Text;

class PacketReader
{
  private byte[] _Content;
  private int _Length, _Cursor;

  public PacketReader(byte[] content)
  {
    Init(content, content.Length);
  }

  public PacketReader(byte[] content, int length)
  {
    Init(content, length);
  }

  public int GetContentSize() { return _Length; }

  private uint ReadUnsignedInt(int l)
  {
    if (_Cursor + l > _Length) throw new ArgumentOutOfRangeException();
    uint r = 0;
    for (int i = 0; i < l; ++i)
      r |= (uint)_Content[_Cursor++] << (i * 8);
    return r;
  }

  private ulong ReadUnsignedLong(int l)
  {
    if (_Cursor + l > _Length) throw new ArgumentOutOfRangeException();
    ulong r = 0;
    for (int i = 0; i < l; ++i)
      r |= (ulong)_Content[_Cursor++] << (i * 8);
    return r;
  }

  public int ReadUInt8()   { return (int)ReadUnsignedInt(1); }
  public int ReadUInt16()  { return (int)ReadUnsignedInt(2); }
  public int ReadUInt32()  { return (int)ReadUnsignedInt(4); }
  public long ReadUInt64() { return (long)ReadUnsignedLong(8); }

  private int ReadInt(int l)
  {
    if (_Cursor + l > _Length) throw new ArgumentOutOfRangeException();
    int r = 0;
    for (int i = 0; i < l; i++)
    {
      if (i == (l - 1))
      {
        r |= (int)(sbyte)_Content[_Cursor++] << (8 * i);
      }
      else
      {
        r |= (int)_Content[_Cursor++] << (8 * i);
      }
    }
    return r;
  }

  private long ReadLong(int l)
  {
    if (_Cursor + l > _Length) throw new ArgumentOutOfRangeException();
    long r = 0;
    for (int i = 0; i < l; i++)
    {
      if (i == (l - 1))
      {
        r |= (long)(sbyte)_Content[_Cursor++] << (8 * i);
      }
      else
      {
        r |= (long)_Content[_Cursor++] << (8 * i);
      }
    }
    return r;
  }

  public int ReadInt8()    { return ReadInt(1); }
  public int ReadInt16()  { return ReadInt(2); }
  public int ReadInt32()  { return ReadInt(4); }
  public long ReadInt64() { return ReadLong(8); }

  public int ReadPeekSize()
  {
    return _Length - _Cursor;
  }

  public string ReadString()
  {
    int i = Array.IndexOf<byte>(_Content, 0, _Cursor, _Length - _Cursor);
    if (i < 0) throw new Exception("null-terminate character not found");

    Encoding enc = Encoding.GetEncoding("utf-8");
    string r = enc.GetString(_Content, _Cursor, i - _Cursor);
    _Cursor += (i - _Cursor) + 1;
    return r;
  }

  public byte[] ReadData(int l)
  {
    if (_Cursor + l > _Length) throw new ArgumentOutOfRangeException();
    byte[] r = new byte [l];
    Buffer.BlockCopy(_Content, _Cursor, r, 0, l);
    _Cursor += l;
    return r;
  }

  private void Init(byte[] content, int length)
  {
    _Content = content;
    _Length = length;
    _Cursor = 0;
  }
}
