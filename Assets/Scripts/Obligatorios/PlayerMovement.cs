using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f; // Movement speed
    [SerializeField] float jumpForce = 5f; // Jump force
    [SerializeField] LayerMask groundLayer; // Layer to define what is ground
    [SerializeField] Transform groundCheck; // Empty GameObject to check if player is grounded
    [SerializeField] float groundCheckRadius = 0.2f; // Radius for ground checking
    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
        Jump();

        if(Time.timeScale == 1)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");


        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        rb.MovePosition(transform.position + move * moveSpeed * Time.deltaTime);
    }

    void Jump()
    {
        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
