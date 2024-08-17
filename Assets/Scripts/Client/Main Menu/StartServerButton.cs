using UnityEngine;
namespace Server
{
    public class StartServerButton : MonoBehaviour
    {
        public void PlayButtonPressed()
        {
            MainManager.Startup();
        }
    }
}