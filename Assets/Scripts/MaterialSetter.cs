using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MaterialSetter: MonoBehaviour 
{
    private MeshRenderer _meshRenderer;
    private MeshRenderer _meshRendererChild;

    private MeshRenderer meshRenderer
    {
        get
        {
            if (_meshRendererChild == null)
                _meshRendererChild = GetComponentInChildren<MeshRenderer>();
                
            return _meshRendererChild;
        }
    }

    public void SetSingleMaterial(Material mat)
    {
        meshRenderer.material = mat;
    }
}