#if UNITY_EDITOR

using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildMenu : ScriptableObject
{
  [MenuItem("OMG Server/Run Server ...", false, 1000)]
  public static void RunServer()
  {
    if (Application.platform == RuntimePlatform.WindowsEditor)
    {
      Process p = new Process();
      p.StartInfo.FileName = "runserver.cmd";
      p.Start();

      return;
    }

    if (Application.platform == RuntimePlatform.OSXEditor)
    {
      Process p = new Process();
      //p.StartInfo.FileName = "/applications/utilities/terminal.app";
      p.StartInfo.FileName = "runserver.sh";
      p.StartInfo.UseShellExecute = true;
      p.Start();

        //ProcessStartInfo proc = new ProcessStartInfo();
        //proc.FileName = "/applications/utilities/terminal.app";
        //proc.WorkingDirectory = "/users/myUserName";
        //proc.Arguments = "runserver.sh";
        //proc.WindowStyle = ProcessWindowStyle.Minimized;
        //proc.CreateNoWindow = true;
        //Process.Start(proc);

      return;
    }
  }


  [MenuItem("OMG Server/Open Project Directory ...", false, 1000)]
  public static void OpenDirectoryProject()
  {
    if (Application.platform == RuntimePlatform.WindowsEditor)
    {
      Process p = new Process();
      p.StartInfo.FileName = "..";
      p.Start();
    }
  }

}

#endif