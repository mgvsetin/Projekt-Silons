using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    //Variables

    public Rigidbody2D rb;

    [SerializeField] float speed;
    [SerializeField] float runningSpeed;
    [SerializeField] float crouchSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] float coverDistance;
    private Vector3 coverCheckDir;
    private float horInput;
    private bool crouched = false;
    public bool behindCover;

    public Transform groundCheck;
    private bool grounded;
    public float checkRadius;
    public LayerMask whatIsGround;

    public Animator anim;


    private void Start()
    {
        Physics2D.queriesStartInColliders = false;
    }

    private void Update()
    {
        //Assigning player direction
        horInput = Input.GetAxis("Horizontal");

        //Checking if player is on the ground
        grounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

    }

    void FixedUpdate()
    {
        //Adding horizontal velocity for walking
        if (!behindCover)
        {
            rb.velocity = new Vector2(horInput * speed * Time.deltaTime, rb.velocity.y);
        }

        //Adding velocity for running 
        if (!behindCover && Input.GetKey(KeyCode.LeftShift))
        {
            if(speed < maxSpeed)
            {
                rb.velocity += new Vector2(horInput * runningSpeed * Time.deltaTime, 0);
            }
        }

        //Adding velocty for crouching and crouch toggle
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (!crouched)
            {
                speed = speed / 2;
                crouched = true;
                anim.SetBool("Crouched", true);
            }
            else
            {
                speed = speed * 2;
                crouched = false;
                anim.SetBool("Crouched", false);
            }
        }

        //Adding vertical velocity
        if (Input.GetButtonDown("Jump") && grounded)
        {
            rb.velocity = Vector2.up * jumpHeight * Time.deltaTime;
        }

        //Going out of cover
        if(behindCover && Input.GetKeyDown(KeyCode.S))
        {
            behindCover = !behindCover;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }

    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("Cover"))
        {
            if (!behindCover && Input.GetKeyDown(KeyCode.W))
            {
                behindCover = !behindCover;
                rb.velocity = Vector2.zero;
                transform.position = new Vector3(collider.transform.position.x, collider.transform.position.y, 6);
            }
        }
    }

}
