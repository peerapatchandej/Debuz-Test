using System.Collections.Generic;
using UnityEngine;

class Remote
{
  private enum State
  {
    DISCONNECTED = 0,
    DISCONNECTING,
    CONNECTED,
    CONNECTING,
  };

  private State _State;
  private Packet _Packet;
  private string _DialogMessage;

  public string DialogMessage { get { return _DialogMessage; } }
  public Remote()
  {
    _DialogMessage = "ไม่สามารถติดต่อเซิร์ฟเวอร์ได้";
    _Packet = new Packet(this);
    _State = State.DISCONNECTED;
  }

  public Packet GetPacket()
  {
    return _Packet;
  }

  public void Connect(string host, int port)
  {
    if (_State != State.DISCONNECTED) return;

    _State = State.CONNECTING;
    _Packet.Connect(host, port);
  }

  public void Disconnect()
  {
    if (_State != State.CONNECTED) return;

    _State = State.CONNECTING;
    _Packet.Disconnect();
  }

  public void OnConnected()
  {
    _State = State.CONNECTED;
  }

  public void OnDisconnected()
  {
    _State = State.DISCONNECTED;
  }

  public bool Connected()
  {
    return _Packet.Connected && _State == State.CONNECTED;
  }

  public bool ConnectFailed()
  {
    return _Packet.Failed;
  }

  public void ProcessEvents()
  {
    _Packet.ProcessEvents();
  }

  public void DoDialog(string message)
  {
    _DialogMessage = message;
  }

  public void DoChat(string user, string message, string talkPlayer)
  {
    UIChat chat = Camera.main.gameObject.GetComponent<UIChat>();
    if (chat != null)
    {
      chat.AddChat(user, message, talkPlayer);
    }
  }

  public void DoAddUser(int uid, string user, int selfUid)
  {
    UIChat chat = Camera.main.gameObject.GetComponent<UIChat>();
    if (chat != null)
    {
      chat.AddUser(uid, user, selfUid);
    }
  }

  public void DoRemoveUser(int uid)
  {
    UIChat chat = Camera.main.gameObject.GetComponent<UIChat>();
    if (chat != null)
    {
      chat.RemoveUser(uid);
    }
  }
}
