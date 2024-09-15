using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] public float speed;
    [SerializeField] private float drag = 0;

    [SerializeField] private Transform orientation;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;

    private Rigidbody rb;
    private bool keyboardMode = false;

    private void Start()
    {
        //set rigidbody info
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.drag = drag;
    }

    private void Update()
    {
        getInput();
        if (Input.GetKeyDown(KeyCode.Space) && !keyboardMode)
        {
            rb.AddForce(Vector3.up * 500);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            keyboardMode = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            keyboardMode = false;
        }
    }
    private void FixedUpdate()
    {
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
        rb.velocity = speed * moveDirection + rb.velocity.y * Vector3.up;
        //rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
    }
}
