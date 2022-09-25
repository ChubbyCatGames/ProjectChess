using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IInputHandler
{
    void ProcessInput(Vector3 inputPosition, GameObject selectedObject, InputAction callback);
}
