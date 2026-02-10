using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
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
    private float jumpVelocity;

    public float MoveSpeed { get => moveSpeed; }
    public float JumpHeight { get => jumpHeight; }
    public float Gravity { get => gravity; }
    public float TerminalSpeed { get => terminalSpeed; }
    public float InputBuffer { get => inputBuffer; }
    public float CoyoteTime { get => coyoteTime; }
    public float Heigth2AirStand { get => heigth2AirStand; }
    public float AirStandGravityMod { get => airStandGravityMod; }
    public float JumpVelocity { get => jumpVelocity; set => jumpVelocity = value; }
}
