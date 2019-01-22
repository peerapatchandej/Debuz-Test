using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using System.Text.RegularExpressions;

public class UILogin : MonoBehaviour
{
  private const int PAGE_WIDTH  = 350;
  private const int PAGE_HEIGHT = 155;

  private string _Status;
  private string _User;

  protected void Start()
  {
    _User = "player01";
    _Status = "Not Connect";
  }

  private void LoginThread(string user)
  {
    Game game = Game.GetInstance();

    game.ClearRemote();

    Packet.Config pc = game.GetNetworkConfig();
    Remote remote = new Remote();

    remote.Connect(pc.host, pc.port);
    remote.ProcessEvents();
    Thread.Sleep(1000);
    for (int i = 0; i < 10; i++)
    {
      if (remote.Connected()) break;
      if (remote.ConnectFailed()) break;

      remote.ProcessEvents();
      Thread.Sleep(50 * i);
    }

    if (!remote.Connected())
    {
      return;
    }

    // send login
    Packet packet = remote.GetPacket();
    packet.SendLogin(user);

    game.SetRemote(remote);
  }

  private IEnumerator LoginCoroutine(string user)
  {
    _Status = "Connecting ...";

    Game game = Game.GetInstance();

    game.ClearRemote();

    Packet.Config pc = game.GetNetworkConfig();
    Remote remote = new Remote();

    remote.Connect(pc.host, pc.port);
    remote.ProcessEvents();
    yield return new WaitForSeconds(0.1f);
    for (int i = 0; i < 10; i++)
    {
      if (remote.Connected()) break;
      if (remote.ConnectFailed()) break;

      remote.ProcessEvents();
      yield return new WaitForSeconds(i * 0.1f);
    }

    if (remote.Connected())
    {
      _Status = "Connected ...";

      // send login
      Packet packet = remote.GetPacket();
      packet.SendLogin(user);

      game.SetRemote(remote);

      _Status = "Loading ...";
      Application.LoadLevel("Chat");
    }
    else
    {
      _Status = "Connect Fail ...";
    }
  }

  protected void OnGUI()
  {
    GUI.BeginGroup(new Rect((Screen.width - PAGE_WIDTH) / 2, 80 + (Screen.height - PAGE_HEIGHT) / 2, PAGE_WIDTH, PAGE_HEIGHT));
    GUI.Box(new Rect(0, 0, PAGE_WIDTH, PAGE_HEIGHT), "เข้าสู่ระบบ");
    GUI.Label(new Rect(14, 32, 50, 28), "ชื่อผู้ใช้");
    _User = GUI.TextField(new Rect(70, 35, 250, 24), _User, 32);
    GUI.Label(new Rect(70, 70, 150, 28), _Status);

    if (GUI.Button(new Rect(70, 100, 135, 40), "ตกลง"))
    {
        //LoginThread(_User, _Password);
        if (ValidateInput(_User))
        {
            StartCoroutine(LoginCoroutine(_User));
        }
        else
        {
            _Status = "ห้ามกรอกอักขระพิเศษ";
        }
    }

    if (GUI.Button(new Rect(218, 105, 100, 30), "ยกเลิก"))
    {
      Application.Quit();
    }

    GUI.EndGroup();
  }

    private bool ValidateInput(string input)
    {
        Regex nameValidator = new Regex(@"^[a-zA-Z0-9]*$");
        return nameValidator.IsMatch(_User);
    }

}
