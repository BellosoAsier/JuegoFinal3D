using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "InputManager")]
public class InputManagerSO : ScriptableObject
{
    Controles myControls;
    public event Action OnJump;
    public event Action<Vector2> OnMove;
    private void OnEnable()
    {
        myControls = new Controles();
        myControls.Gameplay.Enable();
        myControls.Gameplay.Saltar.started += Saltar;
        myControls.Gameplay.Move.performed += Move;
        myControls.Gameplay.Move.canceled += Move;
    }

    private void OnDisable()
    {
        myControls.Gameplay.Disable();
    }

    private void Move(InputAction.CallbackContext obj)
    {
        OnMove?.Invoke(obj.ReadValue<Vector2>());
    }

    private void Saltar(InputAction.CallbackContext obj)
    {
        OnJump?.Invoke();
    }
}
