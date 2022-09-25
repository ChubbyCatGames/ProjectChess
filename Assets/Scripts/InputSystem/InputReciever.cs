using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InputReciever : MonoBehaviour
{
    protected IInputHandler[] inputHandlers;

    public abstract void OnInputRecieved(InputAction.CallbackContext context);

    private void Awake()
    {
        inputHandlers = GetComponents<IInputHandler>();
    }
}
