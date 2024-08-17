using UnityEngine;
using UnityEngine.SceneManagement;
namespace Client
{
    public class ConnectButton : MonoBehaviour
    {
        public void PlayButtonPressed()
        {
            NetworkManager.StartConnections();
            SceneManager.LoadScene(2);
        }
    }
}
