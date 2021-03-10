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
    private bool crouched = false;
    public bool behindCover;

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
                walkingSoundParticle.gameObject.SetActive(false);
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
            if (!crouched)
            {
                walkingSoundParticle.gameObject.SetActive(true);
                walkingSoundParticle.startSize = 7f;
                walkingSoundParticle.GetComponent<CircleCollider2D>().radius = 3f;
            }
        }

        if (rb.velocity.x == 0 && rb.velocity.y == 0)
        {
            walkingSoundParticle.gameObject.SetActive(false);
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
            rb.velocity = new Vector2(horInput * speed * Time.deltaTime, verInput * speed * Time.deltaTime);
        }

        //Adding velocity for running 
        if (!behindCover && Input.GetKey(KeyCode.LeftShift))
        {
            if(speed < maxSpeed)
            {
                rb.velocity += new Vector2(horInput * runningSpeed * Time.deltaTime, verInput * speed * Time.deltaTime);
                walkingSoundParticle.startSize = 12f;
                walkingSoundParticle.GetComponent<CircleCollider2D>().radius = 4.5f;
            }
        }

    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        //Going into cover
        if (collider.CompareTag("Cover"))
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (behindCover)
                {
                    behindCover = false;
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y - 10, 0);
                    }
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y + 10, 0);
                    }
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        transform.position = new Vector3(transform.position.x + 10, transform.position.y, 0);
                    }
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        transform.position = new Vector3(transform.position.x - 10, transform.position.y, 0);
                    }

                    transform.GetComponent<CircleCollider2D>().enabled = true;
                }
                else
                {
                    behindCover = true;
                    transform.GetComponent<CircleCollider2D>().enabled = false;
                    rb.velocity = Vector2.zero;
                    transform.position = new Vector3(collider.transform.position.x, collider.transform.position.y, 0);
                }
            }
        }
    }

}
