using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonBrackeys : MonoBehaviour
{
    [Header("Gravity")]
    [Tooltip("Adds gravity. Default is: 1")]
    public float gravityMultiplier = 1f;                 //Sets a gravity multiplier
    [Tooltip("Drag GroundCheck object here")]
    public Transform groundCheck;                       //Reference to object that checks if the ground is under the player
    [Tooltip("Select layer that is set as ground level")]
    public LayerMask groundLayer;                       //Reference to which layer is "ground"
    [Tooltip("Is the player on the ground?")]
    public bool isGrounded;                             //bool that confirms if the player is grounded

    [Header("Speed")]
    [Tooltip("Sets walkspeed. Default is: 6")]
    public float walkSpeed = 6f;                                             //Variable for walkSpeed

    private float gravityConstant = -9.71f;             //Sets a constant for gravity calculations
    private CharacterController cc;                     //Reference to Character Controller component on object
    private Vector3 velocity;                           //Variables for velocity

    [Header("Rotation")]
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    [Header("Jump")]
    public float jumpHeight;
    private float jumpYPos;

    


    public CharacterController controller;
    public Transform cam;
    public Vector3 moveDir;

    public Vector3 startPos;

    public Animator moveAnim;
    public bool moving;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        startPos = transform.position;

    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.4f, groundLayer);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2;
        }
        else
        {
            velocity.y += gravityConstant * gravityMultiplier * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);

        float horizontal = Input.GetAxisRaw("Horizontal");                          //Input for horizontal movement
        float vertical = Input.GetAxisRaw("Vertical");                              //Input for vertical movement
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;       //Creates a directional variable bazed on which axes is used and normalizes it

        if(direction.magnitude >= 0.1f)                                             //if the direction variable is greater than 0.1
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;      //returns rotational value
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * walkSpeed * Time.deltaTime);                    //Moves the character controller
            moving = true;
        }
        else
        {
            moving = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = startPos;
        }

        if (moving == true)
        {
            moveAnim.SetBool("Running", true);
        }
        else
        {
            moveAnim.SetBool("Running", false);
        }

        if (isGrounded == false)
        {
            moveAnim.SetBool("Falling", true);
        }
        else
        {
            moveAnim.SetBool("Falling", false);
        }
    }

    void Jump()
    {
        Debug.Log("Jumping");
        jumpYPos = transform.position.y;
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityConstant * gravityMultiplier);
        moveAnim.SetTrigger("Jump");
    }
}
