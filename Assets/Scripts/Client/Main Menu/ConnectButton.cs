using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Client
{
    public class ConnectButton : MonoBehaviour
    {
        public void PlayButtonPressed()
        {
            NetworkManager.Main();
            SceneManager.LoadScene(1);
        }
    }
}
