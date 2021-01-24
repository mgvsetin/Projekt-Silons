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
    private float verInput;
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
        verInput = Input.GetAxis("Vertical");

        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + 180, Vector3.forward);

    }

    void FixedUpdate()
    {
        //Adding horizontal velocity for walking
        if (!behindCover)
        {
            rb.velocity = new Vector2(horInput * speed * Time.deltaTime, verInput * speed * Time.deltaTime);
        }

        //Adding velocity for running 
        if (!behindCover && Input.GetKey(KeyCode.LeftShift))
        {
            if(speed < maxSpeed)
            {
                rb.velocity += new Vector2(horInput * runningSpeed * Time.deltaTime, verInput * speed * Time.deltaTime);
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

        //Going out of cover
        if(behindCover && Input.GetKeyDown(KeyCode.S))
        {
            behindCover = false;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }

    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        //Going into cover
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
