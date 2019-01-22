using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIChat : MonoBehaviour
{
  private const int CHAT_WIDTH  = 600;
  private const int CHAT_HEIGHT = 475;
  private const int USER_WIDTH  = 80;


  private string _Chat;
  private List<string> _Talk;
  private Dictionary<int, string> _User;
  private Dictionary<string, Stack<string>> text;
  private string playerDes = "group";
  private int userId = 0;

  protected void Start()
  {
    _Chat = "";

    _Talk = new List<string>();
    _User = new Dictionary<int,string>();
    text = new Dictionary<string, Stack<string>>();
  }

  public void AddChat(string user, string message, string talkPlayer)
  {
    string temp = user + ": " + message;

    if (talkPlayer == playerDes)
    {
        _Talk.Insert(0, temp);

        if (_Talk.Count > 25) _Talk.RemoveAt(25);
    }

    AddStackText(talkPlayer, temp);
  }
 
  public void AddStackText(string playerCurrent, string value)
  {
        if (!text.ContainsKey(playerCurrent))
        {
            Stack<string> stack = new Stack<string>();
            stack.Push(value);
            text.Add(playerCurrent, stack);
        }
        else
        {
            text[playerCurrent].Push(value);
        }
    }

  public void AddUser(int uid, string user, int selfUid)
  {
    if (!_User.ContainsKey(uid))
    {
        if(uid == selfUid)
        {
            userId = selfUid;
        }
    }

    _User[uid] = user;
  }

  public void RemoveUser(int uid)
  {
    if (_User.ContainsKey(uid))
    {
      _User.Remove(uid);

        if (playerDes != "group")
        {
            if (int.Parse(playerDes) == uid)
            {
                text.Remove(playerDes);
                RetrieveText("group");
            }
        }
    }
  }

  protected void Update()
  {
  }
    

  protected void OnGUI()
  {
    GUI.BeginGroup(new Rect((Screen.width - CHAT_WIDTH) / 2 - USER_WIDTH, (Screen.height - CHAT_HEIGHT) / 2, CHAT_WIDTH, CHAT_HEIGHT));
    GUI.Box(new Rect(0, 0, CHAT_WIDTH, CHAT_HEIGHT), "");

    for (int i = 0; i < 25; i++)
    {
        if (i >= _Talk.Count) break;

        GUI.Label(new Rect(2, 18 * (24 - i) - 2, CHAT_WIDTH - 4, 22), _Talk[i]);
    }
        
    _Chat = GUI.TextField(new Rect(0, CHAT_HEIGHT - 25, CHAT_WIDTH - 60, 25), _Chat, 300);
    if (GUI.Button(new Rect(CHAT_WIDTH - 60, CHAT_HEIGHT - 25, 60, 25), "ส่ง"))
    {
      Packet packet = Game.GetInstance().GetRemote().GetPacket();
      packet.SendChat(_Chat, userId.ToString(), playerDes);

      _Chat = "";
    }
    GUI.EndGroup();

    GUI.BeginGroup(new Rect((Screen.width - CHAT_WIDTH) / 2 + CHAT_WIDTH - USER_WIDTH, (Screen.height - CHAT_HEIGHT) / 2, USER_WIDTH, CHAT_HEIGHT));
    GUI.Box(new Rect(0, 0, USER_WIDTH, CHAT_HEIGHT), "");

    int idx = 1;

    if (GUI.Button(new Rect(2, 18 * 0 - 2, USER_WIDTH - 4, 22), "Group"))
    {
        RetrieveText("group");
    }

    foreach (var v in _User)
    {
      if (idx >= 24) break;

      if (v.Key == userId)
      {
        GUI.backgroundColor = Color.green;
        GUI.Button(new Rect(2, 18 * idx - 2, USER_WIDTH - 4, 22), v.Value);
        idx = idx + 1;
      }
      else
      {
        GUI.backgroundColor = Color.gray;
        if (GUI.Button(new Rect(2, 18 * idx - 2, USER_WIDTH - 4, 22), v.Value))
        {
            RetrieveText(v.Key.ToString());
        }

        idx = idx + 1;
      }
    }
    GUI.EndGroup();
  }

  public void RetrieveText(string key)
  {
        //Debug.Log("Retrive Data " + userId + " talk with " + key);

        playerDes = key;
        _Talk.Clear();

        if (text.ContainsKey(playerDes))
        {
            string[] temp = text[playerDes].ToArray();

            for (int i = temp.Length - 1; i >= 0; i--)
            {
                _Talk.Insert(0, temp[i]);

                if (_Talk.Count > 25) _Talk.RemoveAt(25);
            }

            text[playerDes].Clear();

            for (int i = _Talk.Count - 1; i >= 0; i--)
            {
                text[playerDes].Push(_Talk[i]);
            }

            Array.Clear(temp, 0, temp.Length);

            /*Debug.Log("Talk");
            foreach (var i in _Talk)
            {
                Debug.Log(i);
            }

            Debug.Log("Stack");
            string[] test = text[playerDes].ToArray();
            foreach (var i in test)
            {
                Debug.Log(i);
            }*/
        }
    }
}

