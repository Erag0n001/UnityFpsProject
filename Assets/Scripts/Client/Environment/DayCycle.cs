using UnityEngine;
namespace Client
{
    public class DayCycle : MonoBehaviour
    {
        private float time;
        // Start is called before the first frame update
        void Start()
        {
            time = 9000;
        }

        // Update is called once per frame
        void Update()
        {
            time += 10 * Time.deltaTime;
            if (time < 18000)
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
            else if (time >= 18000)
            {
                transform.GetChild(0).gameObject.SetActive(false);
            }
            if (time >= 36000)
            {
                time = 0;
            }
            transform.rotation = Quaternion.Euler(time / 100, 0, 0);
        }
    }
}