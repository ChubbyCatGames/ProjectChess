using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ColliderInputReciever : InputReciever
{
    public Vector3 clickPosition;

    [SerializeField]
    private InputAction selection;
    private Camera mainCamera;

    InputReciever reciever;

    private void Awake()
    {
        mainCamera = Camera.main;
        inputHandlers = GetComponents<IInputHandler>();
    }
    private void OnEnable()
    {
        selection.Enable();
        selection.performed += OnInputRecieved;
    }

    private void OnDisable()
    {
        selection.performed -= OnInputRecieved;
        selection.Disable();
    }

    public override void OnInputRecieved(InputAction.CallbackContext context)
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, hitInfo: out RaycastHit hit) && hit.collider)
        {
            foreach(var handler in inputHandlers)
            {
                handler.ProcessInput(hit.point, null, null);
            }
        }
    }
}
