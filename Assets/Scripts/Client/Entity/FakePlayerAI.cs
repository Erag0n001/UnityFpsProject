using Shared;
using System.Collections;
using UnityEngine;

namespace Client 
{
    public class FakePlayerAI : MonoBehaviour 
    {
        public Player player;

        public IEnumerator LerpRot()
        {
            Quaternion rotation = Converter.Vector4ToQuaternion(player.stats.currentRotation);
            Quaternion startRot = transform.rotation;

            float currentTime = 0f;
            float endTime = 0.250f;
            while (currentTime < endTime)
            {
                currentTime += Time.deltaTime;
                transform.rotation = Quaternion.Lerp(startRot, rotation, currentTime / endTime);
                yield return null;
            }
            transform.rotation = rotation;
        }

        public IEnumerator Lerp()
        {
            Vector3 v = Converter.Vector3ToUnityVector3(player.stats.currentPosition);
            float currentTime = 0f;
            float endTime = 0.250f;
            Vector3 startPos = gameObject.transform.position;
            while (currentTime < endTime)
            {
                currentTime += Time.deltaTime;
                transform.position = Vector3.Lerp(startPos, v, currentTime / endTime);
                yield return null;
            }
            transform.position = v;
        }
    }
}