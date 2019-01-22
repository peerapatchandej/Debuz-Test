using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour
{
  protected void Update()
  {
    Game.Instance.ProcessNetworkEvents();
  }

  protected void OnApplicationQuit()
  {
    Game.GetInstance().ClearRemote();
  }
}
