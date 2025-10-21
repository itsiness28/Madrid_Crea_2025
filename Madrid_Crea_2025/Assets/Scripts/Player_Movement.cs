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
    float moveInput;

    [Header("Ground Check Values")]
    [SerializeField] Transform groundCheckTransform;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundLayer;

    bool isGrounded;


    Rigidbody2D rb;

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
                rb.linearVelocityY = jumpForce;
            }
        }
    }



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundLayer);
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
        if(collision.CompareTag("pSceneTrigger"))
        {
            gameManager.GoToPreviousScene();
        }
        else if (collision.CompareTag("nSceneTrigger"))
        {
            gameManager.GoToNextScene();
        }
    }
}
