using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    public InputSystem_Actions actions;
    [SerializeField] GameManager gameManager;

    [Header("Movement Values")]
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] float gravityFactor;
    float moveInput;

    [Header("Jump Momentum Values")]
    [SerializeField] float jumpHeight;
    [SerializeField] float gravityFactorChanger;
    [SerializeField] float floatingTimer;
    float timerUsed;
    float heightToReach;
    float floatingGravity;

    [Header("Ground Check Values")]
    [SerializeField] Transform groundCheckTransform;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundLayer;

    bool isGrounded;


    Rigidbody2D rb;

    public bool IsGrounded { get => isGrounded; }

    private void Awake()
    {
        actions = new InputSystem_Actions();
    }


    private void OnEnable()
    {
        actions.Player.Enable();

        actions.Player.Move.performed += MoveAction;
        actions.Player.Jump.performed += JumpAction;

        actions.Player.Move.canceled += MoveAction;
        actions.Player.Jump.canceled += JumpAction;
    }

    private void OnDisable()
    {
        actions.Player.Disable();

        actions.Player.Move.performed -= MoveAction;
        actions.Player.Jump.performed -= JumpAction;
    }

    private void MoveAction(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>().x;
    }

    private void JumpAction(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (isGrounded)
            {
                float initialHeight = transform.position.y;
                heightToReach = initialHeight + jumpHeight;
                timerUsed = floatingTimer;
                //rb.gravityScale = gravityFactor;

                rb.linearVelocityY = jumpForce;
                isGrounded = false;
            }
        }
    }



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityFactor;
        floatingGravity = gravityFactor * gravityFactorChanger;
        timerUsed = floatingTimer;
    }


    void Update()
    {

        
        isGrounded = Physics2D.OverlapBox(groundCheckTransform.position, new Vector2(groundCheckRadius * 2, 0.015f), 0, groundLayer);
        if (!isGrounded)
        {
            if (transform.position.y >= heightToReach)
            {
                rb.gravityScale = floatingGravity;
                timerUsed -= Time.deltaTime;
                if (timerUsed <= 0)
                {
                    heightToReach = transform.position.y + 10;
                    timerUsed = floatingTimer;
                    rb.gravityScale = gravityFactor;
                }
            }
        }
        else
        {
            rb.gravityScale = gravityFactor;
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocityX = moveInput * speed;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.purple;
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("pSceneTrigger"))
        {
            gameManager.GoToPreviousScene();
        }
        else if (collision.CompareTag("nSceneTrigger"))
        {
            gameManager.GoToNextScene();
        }
    }
}
