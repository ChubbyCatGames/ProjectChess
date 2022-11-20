using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoWindow : MonoBehaviour
{
    private RectTransform rt;

    [SerializeField] private bool remain;

    [SerializeField] private Vector2 finalPos;
    private Vector2 originalPos;

    private bool animating;

    [SerializeField] private float speed;
    [SerializeField] private float timeShown;

    private State windowState;

    enum State { Outside, MovingIn, Inside, MovingOut };

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        originalPos = rt.anchoredPosition;
        windowState = State.Outside;
        animating = false;
    }

    void Update()
    {
        if (animating)
        {
            if (windowState == State.MovingIn)
            {
                rt.anchoredPosition = Vector3.MoveTowards(rt.anchoredPosition, finalPos, Time.deltaTime * 100 * speed);

                if (rt.anchoredPosition == finalPos)
                {
                    windowState = State.Inside;
                    animating = false;
                    if (!remain)
                    {
                        StartCoroutine(WaitAndRemoveWindow());
                    }
                }
            }
            else if(windowState == State.MovingOut)
            {
                rt.anchoredPosition = Vector3.MoveTowards(rt.anchoredPosition, originalPos, Time.deltaTime * 100 * speed);

                if (rt.anchoredPosition == originalPos)
                {
                    windowState = State.Outside;
                    animating = false;
                }
            }
        }
    }

    public void StartAnimation()
    {
        if (animating) return;
        if (windowState == State.Inside)
        {
            windowState = State.MovingOut;
            animating = true;
        }
        else if (windowState == State.Outside)
        {
            windowState = State.MovingIn;
            animating = true;
        }
    }

    IEnumerator WaitAndRemoveWindow()
    {
        yield return new WaitForSeconds(timeShown);

        StartAnimation();
    }
}
