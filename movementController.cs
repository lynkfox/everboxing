using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementController : MonoBehaviour
{
    [SerializeField] public float speed = 5f; //public in order to allow terrain to change the speed of the character (rough/ice/ect)
    [SerializeField] private float jumpHeight = 7f;
    [SerializeField] public GameObject level; // levels will change.



    private bool jumping = false;
    
    private float startDegree;


    private Rigidbody rb;
   

    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); // get the players RigidBody aspect for ease of typing

    }




    private void FixedUpdate()
    {
        // keeps the player looking in the direction of movement.
        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward);

        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            transform.rotation = Quaternion.LookRotation(-Vector3.forward);

        }

        // Moves the player.
        float move = Input.GetAxisRaw("Horizontal");
        Vector3 movement = new Vector3(move, 0, 0);
        //rb.AddForce(movement * speed);
        rb.MovePosition(transform.position + movement * speed * Time.deltaTime);


        if (!jumping)
        {
            if (Input.GetButtonDown("Jump"))
            {
                rb.AddForce(Vector3.up * jumpHeight);
                jumping = true;
                StartCoroutine(JumpLand());
            }
        }
    }

    IEnumerator JumpLand()
    {
        yield return new WaitForSeconds(1);
        jumping = false;
    }

    /*

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10) //layer 10 = walls. If the player is currently on a wall, disable jumping. Needs a little work (very crude)
        {
            jumping = false;
        }
    }

    */
    

}


