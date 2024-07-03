using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Server;
using UnityEngine.SceneManagement;
using Shared;
public class BuildChecker : MonoBehaviour
{
    private void Awake()
    {
        if(Application.isBatchMode) 
        {
            Printer.Log("Server application starting");
            NetworkManager.Main();
            Application.targetFrameRate = 60;
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
            SceneManager.LoadScene(2);
        } else 
        {
            Printer.Log("Client application starting");
            SceneManager.LoadScene(1);
        }
    }
}
