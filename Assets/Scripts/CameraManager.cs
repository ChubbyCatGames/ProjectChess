using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Camera whiteCam;
    [SerializeField] Camera blackCam;
    private Camera activeCam;

    [SerializeField] ColliderInputReciever colliderInput;

    private void Awake()
    {
        activeCam = Camera.main;
    }

    public void ChangeCam()
    {
        activeCam.gameObject.SetActive(false);
        activeCam= (activeCam.Equals(whiteCam))? blackCam: whiteCam;
        activeCam.gameObject.SetActive(true);
        colliderInput.mainCamera = activeCam;
    }
}
