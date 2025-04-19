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
    public event Action<bool> OnAim;
    public event Action OnShoot;
    private void OnEnable()
    {
        myControls = new Controles();
        myControls.Gameplay.Enable();

        myControls.Gameplay.Saltar.started += Saltar;

        myControls.Gameplay.Move.performed += Move;
        myControls.Gameplay.Move.canceled += Move;

        myControls.Gameplay.Aim.performed += Aim;
        myControls.Gameplay.Aim.canceled += Aim;

        myControls.Gameplay.Shoot.started += Shoot;
    }

    private void OnDisable()
    {
        myControls.Gameplay.Disable();
    }

    private void Shoot(InputAction.CallbackContext obj)
    {
        OnShoot?.Invoke();
    }

    private void Aim(InputAction.CallbackContext obj)
    {
        bool isAiming = obj.performed;
        OnAim?.Invoke(isAiming);
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
