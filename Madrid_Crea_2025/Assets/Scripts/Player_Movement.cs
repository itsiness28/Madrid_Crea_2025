using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class Player_Movement : MonoBehaviour
{
    public InputSystem_Actions actions;
    [SerializeField] GameManager gameManager;
    [SerializeField]
    PlayerData playerData;

    [SerializeField]
    private VolumenConfigurationSO v;

    float moveInput;


    float airStandTimer;

    [Header("Ground Check Values")]
    [SerializeField] Transform groundCheckTransform;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundLayer;

    bool isGrounded;
    Vector3 startPosition;


    Rigidbody2D rb;
    private float inputBufferTimer;
    private float coyoteTimer;

    [Header("Audio")]
    [SerializeField] SoundController soundController;
    [SerializeField] AudioClip audioPasos;
    [SerializeField] AudioClip audioSalto;

    public bool IsGrounded { get => isGrounded; }
    public float MoveInput { get => moveInput; }
    public PlayerData PlayerData { get => playerData; }

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

        actions.Player.Reset.started += Reset_Action;
    }


    private void OnDisable()
    {
        actions.Player.Disable();

        actions.Player.Move.performed -= MoveAction;
        actions.Player.Jump.performed -= JumpAction;

        actions.Player.Reset.started -= Reset_Action;
    }

    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = playerData.Gravity;
        airStandTimer = 0;
    }
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapBox(groundCheckTransform.position, new Vector2(groundCheckRadius * 2, 0.015f), 0, groundLayer);


        rb.linearVelocityX = moveInput * playerData.MoveSpeed;
        if (rb.linearVelocityY < Mathf.Sqrt(2 * (playerData.JumpHeight - playerData.Heigth2AirStand) * Mathf.Abs(playerData.Gravity * 9.81f))
            && rb.linearVelocityY > 0
            && airStandTimer <= 0 && !isGrounded)
        {
            rb.linearVelocityY = Mathf.Sqrt(2 * (playerData.JumpHeight - playerData.Heigth2AirStand) * Mathf.Abs(playerData.Gravity * playerData.AirStandGravityMod) * 9.81f);
            airStandTimer = (rb.linearVelocityY / Mathf.Abs(playerData.Gravity * 9.81f * playerData.AirStandGravityMod)) * 2;
            rb.gravityScale = playerData.Gravity * playerData.AirStandGravityMod;

        }

        
        ManageTimers();

        rb.linearVelocityY = Mathf.Clamp(rb.linearVelocityY, -playerData.TerminalSpeed, playerData.JumpVelocity);
    }

    private void ManageTimers()
    {
        if (isGrounded)
        {
            airStandTimer = 0;
            coyoteTimer = playerData.CoyoteTime;
        }
        if (airStandTimer > 0)
        {
            airStandTimer -= Time.fixedDeltaTime;
        }
        if (airStandTimer <= 0)
        {

            rb.gravityScale = playerData.Gravity;
        }
        if (!isGrounded && coyoteTimer > 0)
        {
            coyoteTimer -= Time.fixedDeltaTime;
        }
        if (inputBufferTimer > 0)
        {
            inputBufferTimer -= Time.fixedDeltaTime;
            Jump();

        }
    }

    private void Reset_Action(InputAction.CallbackContext obj)
    {
        transform.position = startPosition;
    }
    private void MoveAction(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>().x;
    }

    private void JumpAction(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            inputBufferTimer = playerData.InputBuffer;
            
        }
    }

    private void Jump()
    {
        if (isGrounded || coyoteTimer > 0)
        {

            rb.linearVelocityY = playerData.JumpVelocity;
            isGrounded = false;
            coyoteTimer = 0;

            inputBufferTimer = 0;

            PlayAudioSalto();
        }
    }

    public void DisablePlayerMovement()
    {
        actions.Player.Disable();
    }

    public void EnablePlayerMovement()
    {
        actions.Player.Enable();
    }

    public void PlayAudioPasos()
    {
        float randomPitch = Random.Range(1.2f, 1.6f);
        AudioSource x = soundController.GetComponent<AudioSource>();
        x.pitch = randomPitch;
        x.volume = 0.9f * v.Volume;
        soundController.PlaySonido(audioPasos);
    }

    public void PlayAudioSalto()
    {
        float randomPitch = Random.Range(0.8f, 1.2f);
        AudioSource x = soundController.GetComponent<AudioSource>();
        x.pitch = randomPitch;
        x.volume = 0.1f * v.Volume;
        soundController.PlaySonido(audioSalto);
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
