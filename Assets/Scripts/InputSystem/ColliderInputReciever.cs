using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ColliderInputReciever : InputReciever
{
    public Vector3 clickPosition;

    [SerializeField]
    private InputAction selection;
    [SerializeField]
    private InputAction selectionTouch;
    public Camera mainCamera;

    InputReciever reciever;

    private void Awake()
    {
        mainCamera = Camera.main;
        inputHandlers = GetComponents<IInputHandler>();
    }
    private void OnEnable()
    {
        selection.Enable();
        selectionTouch.Enable();
        selection.performed += OnInputRecieved;
        selectionTouch.performed += OnInputRecievedTouch;
    }

    private void OnDisable()
    {
        selection.performed -= OnInputRecieved;
        selectionTouch.performed -= OnInputRecievedTouch;
        selection.Disable();
        selectionTouch.Disable();
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
    public override void OnInputRecievedTouch(InputAction.CallbackContext context)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.GetTouch(0).position);
        
        if (Physics.Raycast(ray, hitInfo: out RaycastHit hit) && hit.collider)
        {
            foreach(var handler in inputHandlers)
            {
                handler.ProcessInput(hit.point, null, null);
            }
        }


    }
}
