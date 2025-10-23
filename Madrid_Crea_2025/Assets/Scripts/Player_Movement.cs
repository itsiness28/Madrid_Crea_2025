using Unity.Android.Gradle.Manifest;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    public InputSystem_Actions actions;
    [SerializeField] GameManager gameManager;
    [SerializeField]
    PlayerData playerData;

    //[Header("Movement Values")]
    //[SerializeField] float speed;
    //[SerializeField] float jumpForce;
    //[SerializeField] float gravityFactor;
    float moveInput;

    [Header("Jump Momentum Values")]
    //[SerializeField] float jumpHeight;
    //[SerializeField] float gravityFactorChanger;
    //[SerializeField] float floatingTimer;
    float airStandTimer;
    //float heightToReach;
    //float floatingGravity;

    [Header("Ground Check Values")]
    [SerializeField] Transform groundCheckTransform;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundLayer;

    bool isGrounded;


    Rigidbody2D rb;

    public bool IsGrounded { get => isGrounded; }
    public float MoveInput { get => moveInput; }

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
                //float initialHeight = transform.position.y;
                //heightToReach = initialHeight + playerData.Heigth2AirStand;
                //rb.gravityScale = gravityFactor;

                rb.linearVelocityY = playerData.JumpVelocity;
                isGrounded = false;
            }
        }
    }



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = playerData.Gravity;
        airStandTimer = 0;
    }


    void Update()
    {


        isGrounded = Physics2D.OverlapBox(groundCheckTransform.position, new Vector2(groundCheckRadius * 2, 0.015f), 0, groundLayer);
        Debug.Log((rb.linearVelocityY < Mathf.Sqrt((playerData.JumpHeight - playerData.Heigth2AirStand) * Mathf.Abs(playerData.Gravity * 9.81f))) + " / " + (rb.linearVelocityY > 0 )+ " / " + (airStandTimer <= 0));
        if (rb.linearVelocityY < Mathf.Sqrt((playerData.Heigth2AirStand - playerData.Heigth2AirStand) * Mathf.Abs(playerData.Gravity * 9.81f)) && rb.linearVelocityY > 0 && airStandTimer <= 0)
        {
            Debug.Log("hola");
            rb.linearVelocityY = Mathf.Sqrt(2 * (playerData.JumpHeight - playerData.Heigth2AirStand) * Mathf.Abs(playerData.Gravity * 9.81f * playerData.AirStandGravityMod));
            airStandTimer = (rb.linearVelocityY / Mathf.Abs(playerData.Gravity * 9.81f * playerData.AirStandGravityMod)) * 2;
            rb.gravityScale = playerData.Gravity * playerData.AirStandGravityMod;

        }
        if (airStandTimer > 0)
        {
            airStandTimer -= Time.deltaTime;
        }
        if (airStandTimer <= 0)
        {
            
            rb.gravityScale = playerData.Gravity;
        }

        rb.linearVelocityY = Mathf.Clamp(rb.linearVelocityY, -playerData.TerminalSpeed, playerData.JumpVelocity);
    }

    private void FixedUpdate()
    {
        rb.linearVelocityX = moveInput * playerData.MoveSpeed;
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
