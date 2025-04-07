using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController controller; // reference to the character controller

    public float speed = 12f;
    public float gravity = -10 * 2f; // make it twice as strong to feel snappy
    public float jumpHeight = 3f;

    // a transform is a point in the world, usually an invisible object,
    // placed just below the playyer to check if the player is on the ground
    public Transform groundCheck; 
    // ground distance is the distance between the ground check and the ground
    public float groundDistance = 0.4f;

    // a layer mask is used to check which objects the player is interacting with
    // it is like a bitmask
    public LayerMask groundMask;

    Vector3 velocity; // the velocity of the player
    bool isGrounded; // is the player on the ground
    bool isMoving; // is the player moving

    // will be used later
    private Vector3 lastPosition = new Vector3(0f,0f,0f); // the last position of the player


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>(); // get the character controller
    }

    // Update is called once per frame
    void Update()
    {
        // check if the player is on the ground using a check sphere
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // reset the y velocity if the player is on the y
        // prevent velocity from accumulating, want it back at base
        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }

        // get the input from the player
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // create the movement vector
        // transform.right is the right vector of the player (red axis in unity)
        // transform.forward is the forward vector of the player (blue axis in unity)
        Vector3 move = transform.right * x + transform.forward * z;

        // want to move the player at the same speed regardless of the frame rate
        controller.Move(move * speed * Time.deltaTime);

        // only allow the player to jump if they are on the ground
        if (Input.GetButtonDown("Jump") && isGrounded) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // falling down
        velocity.y += gravity * Time.deltaTime;

        // make the jump
        controller.Move(velocity * Time.deltaTime);

        if (lastPosition != gameObject.transform.position && isGrounded) {
            isMoving = true;
            // for later use
        } 
        else {
            isMoving = false;
            // for later use
        }

        lastPosition = gameObject.transform.position;
    }
}
