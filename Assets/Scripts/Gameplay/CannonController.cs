using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    private PlayerInput _input;

    private Vector2 _startPosition;
    private Vector2 _prevPosition;
    private Vector2 _currentPos;
    private Vector2 _delta;
    private bool _isPressed;

    public bool IsPressed => _isPressed;
    public Vector2 Delta => _delta;

    private void Awake()
    {
        _input = new PlayerInput();


        _input.Canon.Click.performed += PressStarted;
        _input.Canon.Click.canceled += PressCanceled;
        _input.Canon.Position.performed += Position_performed;
    }

    private void PressStarted(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _isPressed = true;
        _startPosition = _input.Canon.Position.ReadValue<Vector2>();
    }

    private void PressCanceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _isPressed = false;
        _prevPosition = Vector2.zero;
    }

    private void Position_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!_isPressed)
            return;
        if (_startPosition == Vector2.zero)
            return;

        if (_prevPosition != Vector2.zero)
            _prevPosition = _currentPos;
        else
            _prevPosition = _startPosition;

        _currentPos = obj.ReadValue<Vector2>();
        //_delta = _currentPos - _prevPosition;
        _delta = _currentPos - _startPosition;
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input?.Disable();
    }

    private void Update()
    {
        
    }
}
