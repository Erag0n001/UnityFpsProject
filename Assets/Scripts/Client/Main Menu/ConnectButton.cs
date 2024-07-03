using UnityEngine;
using UnityEngine.SceneManagement;
namespace Client
{
    public class ConnectButton : MonoBehaviour
    {
        public void PlayButtonPressed()
        {
            NetworkManager.Main();
            SceneManager.LoadScene(2);
        }
    }
}
