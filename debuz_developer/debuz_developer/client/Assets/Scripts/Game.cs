using System;
using System.Collections.Generic;
using UnityEngine;

class Game
{
  // Static instance
  private static readonly Game _Instance = new Game();
  public static Game GetInstance() { return _Instance; }
  public static Game Instance { get { return _Instance; } }

  private string _Version   = "alpha";

  // networkconfig
  public Packet.Config GetNetworkConfig()
  {
    if (_Version == "alpha")
      return new Packet.Config(HOST_ALPHA, PORT_ALPHA);

    if (_Version == "beta")
      return new Packet.Config(HOST_BETA, PORT_BETA);

    if (_Version == "release")
      return new Packet.Config(HOST_RELEASE, PORT_RELEASE);

    return new Packet.Config(HOST_ALPHA, PORT_ALPHA);
  }

  // Remote & Packet
  private Remote _Remote = null;
  public void SetRemote(Remote r)
  {
    _Remote = r;
  }
  public Remote GetRemote() { return _Remote; }
  public void ClearRemote()
  {
    Remote r = GetRemote();
    if (r != null)
    {
      if (r.Connected())
        r.Disconnect();

      SetRemote(null);
    }
  }

  public void ProcessNetworkEvents()
  {
    Remote r = GetRemote();
    if (r != null)
    {
      if (r.Connected())
        r.ProcessEvents();
    }
  }

  // global const network define
  public const string HOST_ALPHA          = "127.0.0.1";
  public const int    PORT_ALPHA          = 12000;
  public const string HOST_BETA           = "127.0.0.1";
  public const int    PORT_BETA           = 12000;
  public const string HOST_RELEASE        = "127.0.0.1";
  public const int    PORT_RELEASE        = 12000;
}
