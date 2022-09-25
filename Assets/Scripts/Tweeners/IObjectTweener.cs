using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectTweener 
{

    internal void MoveTo(Transform transform, Vector3 targetPosition);
    
}
