using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    [SerializeField] UnityEvent fadeBlackEvent;
    [SerializeField] UnityEvent endVideoEvent;
    private VideoPlayer video;

    private void Awake()
    {
        StartCoroutine(FadeBlack());
        StartCoroutine(EndVideo());
        video = GetComponent<VideoPlayer>();
    }
    private void Start()
    {
        video.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Teaser Beta Chess Holy War.mp4");
        video.Play();
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
