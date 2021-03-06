using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Packet : PacketManager
{
  public class Config
  {
    public string host;
    public int port;

    public Config(string h, int p)
    {
      host = h;
      port = p;
    }
  };

  private enum PacketId
  {
    // client -> server
    // enter game
    CS_LOGIN                  = 0x0000,
    CS_CHAT                   = 0x0001,

    // server -> client packet
    SC_DIALOG                 = 0x0000,
    SC_CHAT                   = 0x0001,

    SC_ADD_USER               = 0x0100,
    SC_REMOVE_USER            = 0x0101,
  };

  private Remote _Remote;

  public Packet(Remote remote) : base()
  {
    _Remote = remote;

    PacketMapper();
  }

  protected override void OnConnected()     { _Remote.OnConnected(); }
  protected override void OnDisconnected()  { _Remote.OnConnected(); }

  private void PacketMapper()
  {
    // _Mapper
    _Mapper[(int)PacketId.SC_DIALOG]          = RecvDialog;
    _Mapper[(int)PacketId.SC_CHAT]            = RecvChat;
    _Mapper[(int)PacketId.SC_ADD_USER]        = RecvAddUser;
    _Mapper[(int)PacketId.SC_REMOVE_USER]     = RecvRemoveUser;
  }

  // Send Function
  public void SendLogin(string user)
  {
    PacketWriter pw = BeginSend((int)PacketId.CS_LOGIN);
    pw.WriteString(user);
    EndSend();
  }

  public void SendChat(string message, string playerSrc, string playerDes)
  {
    PacketWriter pw = BeginSend((int)PacketId.CS_CHAT);
    pw.WriteString(message);
    pw.WriteString(playerSrc);
    pw.WriteString(playerDes);
    EndSend();
  }

  private void RecvDialog(int packet_id, PacketReader pr)
  {
    string message = pr.ReadString();

    _Remote.DoDialog(message);
  }

  private void RecvChat(int packet_id, PacketReader pr)
  {
    string user    = pr.ReadString();
    string message = pr.ReadString();
    string talkPlayer = pr.ReadString();

    _Remote.DoChat(user, message, talkPlayer);
  }

  private void RecvAddUser(int packet_id, PacketReader pr)
  {
    int uid     = pr.ReadUInt16();
    string user = pr.ReadString();
    int selfUid = pr.ReadUInt16();

    _Remote.DoAddUser(uid, user, selfUid);
  }

  private void RecvRemoveUser(int packet_id, PacketReader pr)
  {
    int uid     = pr.ReadUInt16();
        
    _Remote.DoRemoveUser(uid);
  }
}
