using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArcTweener : MonoBehaviour, IObjectTweener
{
    [SerializeField] private float speed;
    [SerializeField] private float height;

    void IObjectTweener.MoveTo(Transform transform, Vector3 targetPosition)
    {
        float distance = Vector3.Distance(transform.position, targetPosition);
        transform.DOJump(targetPosition,height,1,speed/Mathf.Log(distance));

    }
}
