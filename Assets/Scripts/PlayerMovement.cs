using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField] private Rigidbody rig = null;
        
    [Header("INPUTS")]
    float xMove = 0.0f;
    float zMove = 0.0f;

    Awake

    private void Update() {
        CaptureInput();
    }

    private void Move() {

    }

    private void CaptureInput() {
        xMove = Input.GetAxisRaw("Horizontal");
        zMove = Input.GetAxisRaw("Vertical");
    }
}
