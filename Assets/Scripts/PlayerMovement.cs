using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    [Header("Jumping")]
    public float jumpForce = 7f;
    public float fallMultiplier = 2.5f; 
    private bool isGrounded = true;

    [Header("Lane Switching")]
    public float laneWidth = 2.5f; 
    public float laneChangeSpeed = 15f; 
    private int currentLaneIndex = 0; 
    private float targetXPosition;    

    private Rigidbody rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("PlayerMovement: Rigidbody component not found on Player!");
        }

        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError("PlayerMovement: Animator component not found on Player's children!");
        }

        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }

        currentLaneIndex = 0;
        targetXPosition = 0;  
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeLane(-1);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeLane(1);  
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; 

            if (animator != null)
            {
                animator.SetTrigger("DoJump");
            }
        }

        if (animator != null)
        {
            animator.SetBool("IsGrounded", isGrounded);
        }
    }

    void FixedUpdate() 
    {
        Vector3 forwardMovement = transform.forward * moveSpeed * Time.fixedDeltaTime;

        float newX = Mathf.Lerp(rb.position.x, targetXPosition, Time.fixedDeltaTime * laneChangeSpeed);

        Vector3 newPositionHorizontalAndForward = new Vector3(newX, rb.position.y, rb.position.z + forwardMovement.z);
        rb.MovePosition(newPositionHorizontalAndForward); 
        if (rb.linearVelocity.y < 0 && !isGrounded) 
        {
            rb.AddForce(Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }

    void ChangeLane(int direction) 
    {
        int targetLane = currentLaneIndex + direction;

        if (targetLane >= -1 && targetLane <= 1)
        {
            currentLaneIndex = targetLane;
            targetXPosition = currentLaneIndex * laneWidth;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle")) 
        {
            Debug.Log("Player hit an Obstacle!");
            if (GameManager.Instance != null)
            {
                GameManager.Instance.PlayerHitObstacle();
            }
            if (this.enabled) 
            {
                this.enabled = false; 
            }
        }
        else if (collision.gameObject.CompareTag("Ground")) 
        {
            isGrounded = true;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (!isGrounded) 
            {
                isGrounded = true;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}