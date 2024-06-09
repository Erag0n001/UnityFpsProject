using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using UnityEngine.UIElements;
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