using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("COMPONENTS")]
    private Rigidbody rig = null;

    [Header("CORE")]
    [SerializeField] private LayerMask groundLayer = 0;
    [SerializeField] private float     moveSpeed   = 100.0f;
    [SerializeField] private float     airAcell    = 100.0f;
    [SerializeField] private float     jumpForce   = 100.0f;
    private Vector3 inputDirection = Vector3.zero;

    [Header("LIMITERS")]
    [SerializeField] private float groundMaxSpeed = 5.0f;
    [SerializeField] private float airMaxSpeed    = 8.0f;

    [Header("CHECKERS")]
    public bool onGround = true;
    public bool desacellerate = false;
    public bool canMove = true;
        
    [Header("INPUTS")]
    private float xMove = 0.0f;
    private float zMove = 0.0f;
    private bool  jump  = false;

    private void Awake() {
        rig = GetComponent<Rigidbody>();
    }

    private void Update() {
        CaptureInput();
        Move();
        Jump();
        SpeedLimiters();

        onGround = OnGround();
    }

    private void Move() {
        if (!canMove) return;

        inputDirection = new Vector3(xMove, 0f, zMove).normalized;
        Quaternion camRotation    = Quaternion.Euler(0f, Camera.main.transform.eulerAngles.y, 0f);
        Vector3    moveDirection  = camRotation * inputDirection;

        float speed = 0.0f;
        if (onGround) { speed = moveSpeed; }
        else { speed = airAcell; }

        rig.AddForce(moveDirection.normalized * speed * Time.fixedDeltaTime);
    }

    private void Jump() {
        if (jump && onGround) {
            rig.velocity = new Vector3(rig.velocity.x, 0f, rig.velocity.z);
            rig.AddForce(transform.up * jumpForce * Time.fixedDeltaTime);
        }
    }

    private void SpeedLimiters() {
        if (onGround) {
            if (inputDirection.magnitude < 0.1f) {
                rig.velocity = Vector3.MoveTowards(rig.velocity, new Vector3(0f, rig.velocity.y, 0f), 16f * Time.deltaTime);
                return;
            }

            Vector3 vel  = Vector3.ClampMagnitude(new Vector3(rig.velocity.x, 0f, rig.velocity.z), groundMaxSpeed);
            
            if (desacellerate) {
                canMove = false;

                rig.velocity = Vector3.MoveTowards(rig.velocity, new Vector3(vel.x, rig.velocity.y, vel.z), 32f * Time.deltaTime);
                if (Vector3.Distance(rig.velocity, new Vector3(vel.x, rig.velocity.y, vel.z)) < 0.1f) {
                    desacellerate = false;
                    canMove       = true;
                }
            }
            else {
                rig.velocity = new Vector3(vel.x, rig.velocity.y, vel.z);
            }
        }
        else {
            Vector3 vel = Vector3.ClampMagnitude(new Vector3(rig.velocity.x, 0f, rig.velocity.z), airMaxSpeed);
            rig.velocity = new Vector3(vel.x, rig.velocity.y, vel.z);

            desacellerate = true;
            canMove = true;
        }
    }

    private bool OnGround() {
        return Physics.Raycast(transform.position, Vector3.down, 1.0f, groundLayer);
    }

    private void CaptureInput() {
        xMove = Input.GetAxisRaw("Horizontal");
        zMove = Input.GetAxisRaw("Vertical");
        jump  = Input.GetButtonDown("Jump");
    }
}
