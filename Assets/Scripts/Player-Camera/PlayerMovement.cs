using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float accel;

    [SerializeField] private Transform orientation;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;

    private Rigidbody rb;

    private void Start()
    {
        //set rigidbody info
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        //getInput();
    }
    private void FixedUpdate()
    {
        getInput();
        MovePlayer();
    }
    private void getInput()
    {
        //get mouse inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        //always move in direction that camera is facing
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.velocity = speed * moveDirection + Vector3.up * rb.velocity.y;
    }
}
