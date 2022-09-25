using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField]
    private InputAction selection;
    private Camera mainCamera;

    [SerializeField]
    InputReciever reciever;

    private void Awake()
    {
        mainCamera = Camera.main;
    }
    private void OnEnable()
    {
        selection.Enable();
        selection.performed += Select;
    }

    private void OnDisable()
    {
        selection.performed -= Select;
        selection.Disable();
    }





    private void Select(InputAction.CallbackContext context)
    {
        Debug.Log("ey");
        Ray ray= mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if(Physics.Raycast(ray, hitInfo: out RaycastHit hit)&& hit.collider)
        {
            //Debug.Log(hit.collider.gameObject.name);
        }
    }
}
