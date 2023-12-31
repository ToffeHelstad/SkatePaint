using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoverboard : MonoBehaviour
{
    Rigidbody hb;
    public Animator anim;

    private float gravityConstant = -9.71f;
    public float gravityMultiplier = 1f;
    public float jumpHeight;

    public bool isGrounded;
    public bool spacePressed;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Vector3 velocity;
    void Start()
    {
        hb = GetComponent<Rigidbody>();
    }

    public float multiplier;
    public float moveForce, turnTorque, jumpForce;

    public Transform[] anchors = new Transform[4];
    RaycastHit[] hits = new RaycastHit[4];

    private void FixedUpdate()
    {
        for (int i = 0; i < 4; i++)
            ApplyForce(anchors[i], hits[i]);

        hb.AddForce(Input.GetAxis("Vertical") * moveForce * transform.forward);
        hb.AddTorque(Input.GetAxis("Horizontal") * turnTorque * transform.up);

    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.4f, groundLayer);

        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            //hb.AddForce(Input.GetAxis("Vertical") * moveForce * transform.forward);
            //hb.AddForce(transform.forward*moveForce);
        }
        else if (Input.GetKeyUp(KeyCode.Space) && isGrounded)
            Ollie();

        if(!isGrounded && Input.GetKeyDown(KeyCode.E))
        {
            KickFlip();
        }

    }
    
    void ApplyForce(Transform anchor, RaycastHit hit)
    {
        if(Physics.Raycast(anchor.position, -anchor.up, out hit))
        {
            float force = 0;
            force = Mathf.Abs(1 / (hit.point.y - anchor.position.y));
            hb.AddForceAtPosition(transform.up * force * multiplier, anchor.position, ForceMode.Acceleration);
        }
    }
    void Ollie()
    {
        Debug.Log("Ollie");
        hb.AddForce(transform.up * jumpForce);
    }

    void KickFlip()
    {
        anim.SetTrigger("Kickflip");
    }




}
