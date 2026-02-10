using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    #region Componets
    [SerializeField]
    private InputSO inputSO;

    [SerializeField]
    PlayerData playerData;

    [SerializeField]
    private Rigidbody2D rb;
    #endregion

    [SerializeField]
    private LayerMask layerMask;

    #region  timers

    private float inputBufferTimer = 0;
    private float coyoteTimer = 0;
    private float airStandTimer = 0;
    #endregion

    private Vector2 directionalInput;
    [SerializeField]
    private bool below;

    private Vector2 velocity;

    private void OnEnable()
    {
        inputSO.OnRunAction += SetDirectionalInput;

        inputSO.OnJumpAnction += OnJumpInput;
        inputSO.OnJumpCanceledAction += OnCanceledJumpInput;
        playerData.JumpVelocity = Mathf.Sqrt(2 * playerData.JumpHeight * Mathf.Abs(playerData.Gravity));

    }
    private void OnDisable()
    {
        inputSO.OnRunAction -= SetDirectionalInput;

        inputSO.OnJumpAnction -= OnJumpInput;
        inputSO.OnJumpCanceledAction -= OnCanceledJumpInput;
    }

    void Update()
    {

        CalculateVelocity();
        if(velocity.y <= 0)
        {
            below = Physics2D.OverlapBox(transform.position, new Vector2(0.375f-0.030f, 0.015f), 0, layerMask);

        }
        else
        {
            below = false;
        }
        if (below)
        {

            velocity.y = 0;
            airStandTimer = 0;
            coyoteTimer = playerData.CoyoteTime;
        }

        rb.linearVelocity = velocity;
        ManagerTimers();
    }

    public void ManagerTimers()
    {
        if (airStandTimer > 0)
        {
            airStandTimer -= Time.deltaTime;
        }
        if (!below && coyoteTimer > 0)
        {
            coyoteTimer -= Time.deltaTime;
        }
        if (inputBufferTimer > 0)
        {
            inputBufferTimer -= Time.deltaTime;
            Jump();
        }
    }

    private void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }

    private void OnJumpInput()
    {

        inputBufferTimer = playerData.InputBuffer;

    }

    private void Jump()
    {
        if (below || coyoteTimer > 0)
        {
            velocity.y = playerData.JumpVelocity;
            coyoteTimer = 0;
            inputBufferTimer = 0;
        }
    }
    private void OnCanceledJumpInput()
    {

        if (Mathf.Sign(velocity.y) == 1 && airStandTimer <= 0 && !below)
        {
            velocity.y = Mathf.Sqrt(2 * 0.1f * Mathf.Abs(playerData.Gravity * playerData.AirStandGravityMod));
            airStandTimer = (velocity.y / Mathf.Abs(playerData.Gravity * playerData.AirStandGravityMod)) * 2;

        }
        inputBufferTimer = 0;
    }



    void CalculateVelocity()
    {

        velocity.x = directionalInput.x * playerData.MoveSpeed;
        if (!below)
        {
            if (velocity.y < Mathf.Sqrt((playerData.JumpHeight - playerData.Heigth2AirStand) * Mathf.Abs(playerData.Gravity)) && velocity.y > 0 && airStandTimer <= 0)
            {
                velocity.y = Mathf.Sqrt(2 * (playerData.JumpHeight - playerData.Heigth2AirStand) * Mathf.Abs(playerData.Gravity * playerData.AirStandGravityMod));
                airStandTimer = (velocity.y / Mathf.Abs(playerData.Gravity * playerData.AirStandGravityMod)) * 2;
            }

            if (airStandTimer <= 0)
            {

                velocity.y += playerData.Gravity * Time.deltaTime;
            }
            else
            {

                velocity.y += playerData.Gravity * Time.deltaTime * playerData.AirStandGravityMod;
            }
        }


        velocity.y = Mathf.Clamp(velocity.y, -playerData.TerminalSpeed, playerData.JumpVelocity);
    }
}
