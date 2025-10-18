using UnityEngine;
using System;

[CreateAssetMenu(fileName = "InputSO", menuName = "Scriptable Objects/InputSO")]
public class InputSO : ScriptableObject
{
    public Action<Vector2> OnRunAction;
    public Action OnJumpAnction;
    public Action OnJumpCanceledAction;
}
