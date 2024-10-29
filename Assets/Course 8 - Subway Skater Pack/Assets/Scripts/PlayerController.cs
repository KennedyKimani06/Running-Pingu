using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    #region Movement Variables
    public float speed = 7.0f;
    public AudioClip clip;
    public bool isOnGround;
    private float jumpForce = 5.0f;
    private float gravity = 10.0f;
    public int desiredLane = 0;
    public Vector3 targetPos;
    private float verticalVelocity;
    public const float LANE_DISTANCE = 3.0f;
    private Vector3 moveVector;
    #endregion

    #region Components
    private CharacterController controller;
    public Animator anim;
    #endregion

    #region Mobile Input Variables
    private Vector2 swipeDelta;
    private Vector2 firstTouch;
    private Vector2 lastTouch;
    private bool swipeRight, swipeLeft, swipeUp, swipeDown;
    private bool isTouching;
    private float SWIPE_THRESHOLD = 50f;
    public float i = 5;
    #endregion

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        ResetSwipeFlags();
        if (!GameObject.FindObjectOfType<GameManager>().isGameStarted) {
            InvokeRepeating("SpeedModification", 30f*i, 35*i);
        }
    }

    void Update()
    {
        if (!GameObject.FindObjectOfType<GameManager>().isGameStarted)
            return;
        // Handle Input
        HandleInput();

        // Process Movement
        ProcessMovement();

        // Reset swipe flags at the end of frame
        ResetSwipeFlags();
    }

    void HandleInput()
    {
        // Handle Touch Input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                isTouching = true;
                firstTouch = touch.position;
                swipeDelta = Vector2.zero;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isTouching = false;
                lastTouch = touch.position;
                swipeDelta = lastTouch - firstTouch;
                
                ProcessSwipe();
            }
        }
        // Handle Mouse Input (for testing in editor)
        else if (Input.GetMouseButtonDown(0))
        {
            firstTouch = Input.mousePosition;
            swipeDelta = Vector2.zero;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            lastTouch = Input.mousePosition;
            swipeDelta = lastTouch - firstTouch;
            
            ProcessSwipe();
        }

        // Handle Keyboard Input (for testing)
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            HorizontalMove(false);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            HorizontalMove(true);
        }
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Jump();
            if (controller.height != 1) {
                StopSliding();
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && !IsGrounded())
        {
            verticalVelocity = -jumpForce;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && IsGrounded())
        {
            Sliding();
        }
    }

    void ProcessSwipe()
    {
        if (swipeDelta.magnitude > SWIPE_THRESHOLD)
        {
            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
            {
                // Horizontal swipe
                if (swipeDelta.x < 0)
                {
                    swipeLeft = true;
                    HorizontalMove(false);
                }
                else
                {
                    swipeRight = true;
                    HorizontalMove(true);
                }
            }
            else
            {
                // Vertical swipe
                if (swipeDelta.y > 0 && IsGrounded())
                {
                    swipeUp = true;
                    Jump();
                    if (controller.height != 1) {
                        StopSliding();
                    }
                }
                else if (swipeDelta.y < 0 && !IsGrounded())
                {
                    swipeDown = true;
                    verticalVelocity = -jumpForce;
                }
                if (swipeDelta.y < 0 && IsGrounded())
                {
                    Sliding();
                }
            }
        }
    }

    void ProcessMovement()
    {
        // Calculate target position
        targetPos = transform.position.z * Vector3.forward;
        if (desiredLane == -1)
        {
            targetPos += Vector3.left * LANE_DISTANCE;
        }
        else if (desiredLane == 1)
        {
            targetPos += Vector3.right * LANE_DISTANCE;
        }

        // Apply gravity
        if (!IsGrounded())
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        // Calculate movement vector
        moveVector = new Vector3(
            (targetPos - transform.position).normalized.x * speed,
            verticalVelocity,
            speed
        );

        // Move character
        controller.Move(moveVector * Time.deltaTime);

        // Update character rotation
        Vector3 dir = controller.velocity;
        dir.y = 0;
        transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * 10f);
    }

    void SpeedModification () {
        speed+=i;
        i+=0.3f;
    }
    void HorizontalMove(bool right)
    {
        desiredLane += right ? 1 : -1;
        desiredLane = Mathf.Clamp(desiredLane, -1, 1);
    }

    void Jump()
    {
        verticalVelocity = jumpForce;
        isOnGround = false;
        if (anim != null)
        {
            anim.SetTrigger("Jump");
        }
    }
    void Sliding () {
        anim.SetBool("Sliding", true);
        controller.height /= 2;
        controller.center = new Vector3(controller.center.x, controller.center.y / 1.5f, controller.center.z);
        Invoke("StopSliding", 1.0f);
        controller.detectCollisions = false;
    }
    void OnControllerColliderHit (ControllerColliderHit hit) {
        switch (hit.gameObject.tag)
        {
            case "Obstacle":
                if (isOnGround) {
                    Crash();
                    FindObjectOfType<GameManager>().isGameStarted = false;
                    controller.Move(Vector3.back*0.2f);
                    controller.Move(Vector3.up * 0.2f);
                }
            break;
            case "Ground":
                isOnGround = true;
            break;
            case "Ramp":
                isOnGround = false;
            break;
        }
        
    }
    void StopSliding () {
        anim.SetBool("Sliding", false);
        controller.height *= 2;
        controller.center = new Vector3(controller.center.x, controller.center.y * 1.5f, controller.center.z);
        controller.detectCollisions = true;
    }
    bool IsGrounded()
    {
        float rayLength = 0.3f;
        Vector3 rayStart = new Vector3(
            controller.bounds.center.x,
            controller.bounds.min.y + 0.1f,
            controller.bounds.center.z
        );

        // Draw debug ray in scene view
        Debug.DrawRay(rayStart, Vector3.down * rayLength, Color.red);
        
        bool grounded = Physics.Raycast(rayStart, Vector3.down, rayLength);
        if (anim != null)
        {
            anim.SetBool("Ground", grounded);
        }
        return grounded;
    }

    // Create a crash function
    void Crash () {
        anim.SetTrigger("Death");
        GetComponent<AudioSource>().PlayOneShot(clip);
        FindObjectOfType<WorldMovement>().isRunning = false;
        FindObjectOfType<CreateGoups>().isRunning = false;
        Invoke("MenuSceneLoad", 3f);
    }
    void ResetSwipeFlags()
    {
        swipeRight = swipeLeft = swipeUp = swipeDown = false;
    }

    void MenuSceneLoad () {
        SceneManager.LoadScene(0);
    }
}