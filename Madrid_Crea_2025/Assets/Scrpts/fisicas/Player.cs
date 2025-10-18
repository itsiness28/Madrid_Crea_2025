using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(PlayerControler))]

public class Player : MonoBehaviour
{
    #region Componets
    [SerializeField]
    private InputSO inputSO;
    [SerializeField]
    private PlayerControler controller;
    [SerializeField]
    private TrailRenderer line;
    #endregion

    #region Statds
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float gravity;
    [SerializeField]
    private float terminalSpeed;
    [SerializeField]
    private float inputBuffer;
    [SerializeField]
    private float coyoteTime;
    [SerializeField]
    private float heigth2AirStand;
    [SerializeField]
    private float airStandGravityMod;
    [SerializeField]
    private float jumpVelocity;//calculas en Start o en playeEditor
    #endregion

    private Vector3 velocity;

    private Vector2 directionalInput;

    #region  timers

    private float inputBufferTimer = 0;
    private float coyoteTimer = 0;
    private float airStandTimer = 0;
    #endregion

    #region Actions
    public Action<float> OnMoveAction;
    #endregion

    public Vector3 Velocity { get => velocity; set => velocity = value; }
    public PlayerControler Controller { get => controller; }

    private void OnEnable()
    {
        inputSO.OnRunAction += SetDirectionalInput;

        inputSO.OnJumpAnction += OnJumpInput;
        inputSO.OnJumpCanceledAction += OnCanceledJumpInput;
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

        controller.Move(velocity * Time.deltaTime);
        if (controller.getInfoCollision().Below)
        {
            velocity.y = 0;
            airStandTimer = 0;
            coyoteTimer = coyoteTime;
        }
        else if (controller.getInfoCollision().Above)
        {
            velocity.y *= 0.5f;
        }
        ManagerTimers();
    }

    public void ManagerTimers()
    {
        if (airStandTimer > 0)
        {
            airStandTimer -= Time.deltaTime;
        }
        if (!controller.getInfoCollision().Below && coyoteTimer > 0)
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
        OnMoveAction.Invoke(directionalInput.x);

    }

    private void OnJumpInput()
    {

        inputBufferTimer = inputBuffer;

    }

    private void Jump()
    {
        if (controller.getInfoCollision().Below || coyoteTimer > 0)
        {
            velocity.y = jumpVelocity;
            coyoteTimer = 0;

            inputBufferTimer = 0;
        }
    }
    private void OnCanceledJumpInput()
    {

        if (Mathf.Sign(velocity.y) == 1 && airStandTimer <= 0 && !controller.getInfoCollision().Below)
        {
            velocity.y = Mathf.Sqrt(2 * 0.1f * Mathf.Abs(gravity * airStandGravityMod));
            airStandTimer = (velocity.y / Math.Abs(gravity * airStandGravityMod)) * 2;
        }
        inputBufferTimer = 0;
    }



    void CalculateVelocity()
    {

        velocity.x = directionalInput.x * moveSpeed;
        if (velocity.y < Mathf.Sqrt((jumpHeight - heigth2AirStand) * Mathf.Abs(gravity)) && velocity.y > 0 && airStandTimer <= 0)
        {
            velocity.y = Mathf.Sqrt(2 * (jumpHeight - heigth2AirStand) * Mathf.Abs(gravity * airStandGravityMod));
            airStandTimer = (velocity.y / Mathf.Abs(gravity * airStandGravityMod)) * 2;
        }

        if (airStandTimer <= 0)
        {
            line.startColor = Color.black;
            line.endColor = Color.black;
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            line.startColor = Color.white;
            line.endColor = Color.white;
            velocity.y += gravity * airStandGravityMod * Time.deltaTime;
        }

        velocity.y = Mathf.Clamp(velocity.y, -terminalSpeed, jumpVelocity);
    }

}
