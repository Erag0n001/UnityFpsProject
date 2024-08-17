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
            Server.MainManager.Startup();
        }
        else
        {
            Printer.Log("Client application starting");
            Client.MainManager.IsServer = false;
            SceneManager.LoadScene(1);
        }
    }
}
