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
    private float horInput;
    private float verInput;
    public bool crouched;
    public bool behindCover;
    private bool nearCover;
    public Transform coverImIn;
    public bool canMove;

    public Animator anim;

    public ParticleSystem walkingSoundParticle;


    private void Start()
    {
        Physics2D.queriesStartInColliders = false;
    }

    private void Update()
    {
        //Assigning player direction
        horInput = Input.GetAxis("Horizontal");
        verInput = Input.GetAxis("Vertical");

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

        //Walking Particle

        if (rb.velocity.x >= 0.1f || rb.velocity.y >= 0.1f || rb.velocity.x <= -0.1f || rb.velocity.y <= -0.1f)
        {
            walkingSoundParticle.gameObject.SetActive(true);
            if (!crouched)
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    walkingSoundParticle.startSize = 7f;
                    walkingSoundParticle.GetComponent<CircleCollider2D>().radius = 3f;
                }
            }
        }

        //Crouching Particles

        if (crouched)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                walkingSoundParticle.startSize = 3f;
                walkingSoundParticle.GetComponent<CircleCollider2D>().radius = 1.75f;
            }
            else
            {
                walkingSoundParticle.gameObject.SetActive(false);
            }
        }

        //Stoping Particles when not moving

        if (rb.velocity.x == 0 && rb.velocity.y == 0)
        {
            walkingSoundParticle.gameObject.SetActive(false);
        }

        //Cover System

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (nearCover)
            {
                if (behindCover)
                {
                    if (Input.GetKey(KeyCode.S))
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y - 1, 0);
                    }
                    if (Input.GetKey(KeyCode.W))
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y + 1, 0);
                    }
                    if (Input.GetKey(KeyCode.D))
                    {
                        transform.position = new Vector3(transform.position.x + 1.5f, transform.position.y, 0);
                    }
                    if (Input.GetKey(KeyCode.A))
                    {
                        transform.position = new Vector3(transform.position.x - 1.5f, transform.position.y, 0);
                    }

                    behindCover = false;
                    transform.GetComponent<CircleCollider2D>().enabled = true;
                    gameObject.GetComponent<MeshRenderer>().enabled = true;

                }
                else
                {
                    behindCover = true;
                    gameObject.GetComponent<MeshRenderer>().enabled = false;
                    transform.GetComponent<CircleCollider2D>().enabled = false;
                    rb.velocity = Vector2.zero;
                    transform.position = new Vector3(coverImIn.position.x, coverImIn.position.y, 0f);
                }
            }
        }

    }

    void FixedUpdate()
    {

        //Looking with Mouse
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + 180, Vector3.forward);

        //Adding velocity for walking
        if (!behindCover)
        {
            if (canMove)
            {
                rb.velocity = new Vector2(horInput * speed * Time.deltaTime, verInput * speed * Time.deltaTime);
            }
        }

        //Adding velocity for running 
        if (!behindCover && Input.GetKey(KeyCode.LeftShift))
        {
            if (canMove)
            {
                if (speed <= maxSpeed)
                {
                    rb.velocity += new Vector2(horInput * runningSpeed * Time.deltaTime, verInput * speed * Time.deltaTime);
                    if (!crouched)
                    {
                        walkingSoundParticle.startSize = 12f;
                        walkingSoundParticle.GetComponent<CircleCollider2D>().radius = 4.5f;
                    }
                }
            }
        }

    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        //Going into cover
        if (collider.CompareTag("Cover"))
        {
            nearCover = true;
            coverImIn = collider.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Cover"))
        {
            nearCover = false;
        }
    }

}
