using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [SerializeField] private float sensitivity = 100.0f;

    float x, y;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update() {
        x += Input.GetAxis("Mouse X") * Time.fixedDeltaTime * sensitivity;
        y -= Input.GetAxis("Mouse Y") * Time.fixedDeltaTime * sensitivity;
        y  = Mathf.Clamp(y, -90, 90);

        transform.rotation = Quaternion.Euler(y, x, 0f);
    }
}
