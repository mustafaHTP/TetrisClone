using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public FrameInput FrameInput;

    private PlayerControls _playerControls;
    private InputAction _moveHorizontalAction;
    private InputAction _moveDownAction;
    private InputAction _rotateAction;

    private void Awake()
    {
        _playerControls = new();

        _moveHorizontalAction = _playerControls.Player.MoveHorizontal;
        _moveDownAction = _playerControls.Player.MoveDown;
        _rotateAction = _playerControls.Player.Rotate;

        FrameInput = new();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    private void Update()
    {
        //To achieve similar behavior of GetAxisRow
        FrameInput.MoveHorizontalInput = _moveHorizontalAction.ReadValue<float>() switch
        {
            > 0f => 1f,
            < 0f => -1f,
            _ => 0f
        };
        FrameInput.RotateInput = _rotateAction.WasPressedThisFrame();
        FrameInput.MoveDownInput = _moveDownAction.IsPressed();
    }
}

public class FrameInput
{
    public float MoveHorizontalInput { get; set; }
    public bool RotateInput { get; set; }
    public bool MoveDownInput { get; set; }
}
