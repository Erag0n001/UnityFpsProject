using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CameraControl : MonoBehaviour
{

    float oldMousePX;
    float oldMousePY;

    public GameObject head;
    public GameObject character;
    public bool isPlayerBusy;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isPlayerBusy)
        {
            Camera();
        }
    }

    void Camera()
    {
        float mousePX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * 500;
        float mousePY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * 500;

        oldMousePY += mousePX;
        oldMousePX -= mousePY;
        oldMousePX = Mathf.Clamp(oldMousePX, -90, 90);
        head.transform.rotation = Quaternion.Euler(oldMousePX, oldMousePY, 0);
        character.transform.rotation = Quaternion.Euler(0, oldMousePY, 0);
        MainManager.PlayerRot = character.transform.rotation;
    }
}
