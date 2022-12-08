using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VideoController : MonoBehaviour
{
    [SerializeField] UnityEvent fadeBlackEvent;
    [SerializeField] UnityEvent endVideoEvent;


    private void Awake()
    {
        StartCoroutine(FadeBlack());
        StartCoroutine(EndVideo());
    }

    IEnumerator FadeBlack()
    {
        yield return new WaitForSeconds(62f);
        fadeBlackEvent?.Invoke();
    }

    IEnumerator EndVideo()
    {
        yield return new WaitForSeconds(71f);
        endVideoEvent?.Invoke();
    }


}
