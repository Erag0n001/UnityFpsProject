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
            hitPointBar.sizeDelta = new Vector2(MainManager.playerStatManager.stats.hitPoint * (400 / MainManager.playerStatManager.stats.maxHitpoint), hitPointBar.rect.height);
        }
    }
}