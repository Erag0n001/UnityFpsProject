using UnityEngine;
namespace Client
{
    public class Hitpointbar : MonoBehaviour
    {
        private RectTransform hitPointBar;
        private GameObject player;
        void Start()
        {
            player = GameObject.Find("Player");
        }
        void Update()
        {
            hitPointBar = gameObject.transform.GetComponent<RectTransform>();
            hitPointBar.sizeDelta = new Vector2(MainManager.mainPlayer.stats.hitPoint * (400 / MainManager.mainPlayer.stats.maxHitpoint), hitPointBar.rect.height);
        }
    }
}