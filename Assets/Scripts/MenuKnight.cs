using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuKnight : MonoBehaviour
{
    private IObjectTweener tweener;
    private bool hasAlreadySelected;

    private void Awake()
    {
        tweener=GetComponent<IObjectTweener>();
    }

    public void MoveHorse(Transform target)
    {
        if (hasAlreadySelected) return;

        tweener.MoveTo(transform, target.position);
        hasAlreadySelected = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GetComponent<AudioSource>().Play();
    }

}
