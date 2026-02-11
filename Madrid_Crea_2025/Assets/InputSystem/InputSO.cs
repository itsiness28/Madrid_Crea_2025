using UnityEngine;
using System;

[CreateAssetMenu(fileName = "InputSO", menuName = "Scriptable Objects/InputSO")]
public class InputSO : ScriptableObject
{
    private Controller controller;

    public Action<Vector2> OnRunAction;
    public Action OnJumpAnction;
    public Action OnJumpCanceledAction;

    public Action TimeTrigger;

    private void OnEnable()
    {
        controller = new Controller();
        controller.Player.Enable();

        controller.Player.Run.performed += OnRunPerformed;
        controller.Player.Run.canceled += OnRunCanceled;

        controller.Player.Jump.started += OnJump;
        controller.Player.Jump.canceled += OnJumpCanceled;

        controller.Player.TimeTrigger.started += OnTimeTrigger;
    }


    private void OnDisable()
    {
        controller.Player.Run.performed -= OnRunPerformed;
        controller.Player.Run.canceled -= OnRunCanceled;

        controller.Player.Jump.started -= OnJump;
        controller.Player.Jump.canceled -= OnJumpCanceled;
        controller.Player.Disable();

        controller.Player.TimeTrigger.started -= OnTimeTrigger;
    }

    private void OnRunCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnRunAction?.Invoke(Vector2.zero);
    }
    private void OnRunPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnRunAction?.Invoke(obj.ReadValue<Vector2>().normalized);
    }

    private void OnJump(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnJumpAnction?.Invoke();
    }
    private void OnJumpCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnJumpCanceledAction?.Invoke();
    }
    private void OnTimeTrigger(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {

        TimeTrigger?.Invoke();
    }

    public void SwitchMode(InputMode mode)
    {
        switch (mode)
        {
            case InputMode.moveMode:

                controller.Player.Enable();

                break;

            case InputMode.disableMode:

                controller.Player.Disable();

                break;

            default:
                break;
        }
    }
}


public enum InputMode { moveMode, disableMode}