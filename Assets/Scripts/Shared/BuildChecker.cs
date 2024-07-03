using UnityEngine;
using Server;
using UnityEngine.SceneManagement;
using Shared;
using System;
using System.Threading.Tasks;
public class BuildChecker : MonoBehaviour
{
    private void Awake()
    {
        if(Application.isBatchMode) 
        {
            Printer.Log("Server application starting");
            NetworkManager.Main();
            Task.Run(ConsoleManager.ConsoleListenForInput);
            Application.targetFrameRate = 60;
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
            Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.None);
            Application.SetStackTraceLogType(LogType.Error, StackTraceLogType.None);
            Client.MainManager.IsServer = true;
            SceneManager.LoadScene(2);
        }
        else
        {
            Printer.Log("Client application starting");
            Client.MainManager.IsServer = false;
            SceneManager.LoadScene(1);
        }
    }
    private void ConsoleListenForInput() 
    {
        while (true) 
        {
            string input = System.Console.ReadLine();
            if (input != "") 
            {
                Printer.LogError(input);
            }
        }
    }
}
