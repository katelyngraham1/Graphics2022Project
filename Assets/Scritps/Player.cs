using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform groundCheckTransform = null;
    [SerializeField] private LayerMask playerMask;
    private bool jumpKeyPressed;
    private float horizontalInput;
    private Rigidbody rigidbodyComponent;
    private int superJumpsRemaining;

    // Start is called before the first frame update
    void Start()
    {
       rigidbodyComponent = GetComponent<Rigidbody>();    
    }

    // Update is called once per frame
    void Update()
    {
        // checking if space bar pressed 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpKeyPressed = true;
        }

        horizontalInput = Input.GetAxis("Horizontal");
    }

    // FixedUpdate is called once every physics update
    private void FixedUpdate()
    {       
        rigidbodyComponent.velocity = new Vector3(horizontalInput, rigidbodyComponent.velocity.y, 0);

        if (Physics.OverlapSphere(groundCheckTransform.position, 0.1f, playerMask).Length == 0)
        {
            return;
        }

        // How high the player jumps
        if (jumpKeyPressed)
        {
            float jumpPower = 1f;
            if (superJumpsRemaining > 0)
            {
                // double jump
                jumpPower *= 2.5f;
                superJumpsRemaining--;
            }
            rigidbodyComponent.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
            jumpKeyPressed = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            // destroys yellow coins and adds one to the amount of double jumps left
            Destroy(other.gameObject);
            superJumpsRemaining++;
        }

        if (other.gameObject.layer == 8)
        {
            // UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
    }
}
