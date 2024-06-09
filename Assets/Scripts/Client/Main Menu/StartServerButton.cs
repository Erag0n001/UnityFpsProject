using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Server
{
    public class StartServerButton : MonoBehaviour
    {
        public void PlayButtonPressed()
        {
            NetworkManager.Main();
        }
    }
}