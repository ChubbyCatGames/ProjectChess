using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoWindow : MonoBehaviour
{
    private RectTransform rt;

    [SerializeField] private Vector2 finalPos;
    private Vector2 originalPos;

    private bool animating;

    [SerializeField] private float speed;
    [SerializeField] private float timeShown;

    private State windowState;

    public bool Animating { get => animating; set => animating = value; }
    public State WindowState { get => windowState; set => windowState = value; }

    public enum State { Outside, MovingIn, Inside, MovingOut };

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        originalPos = rt.anchoredPosition;
        WindowState = State.Outside;
        Animating = false;
    }

    void Update()
    {
        if (Animating)
        {
            if (WindowState == State.MovingIn)
            {
                rt.anchoredPosition = Vector3.MoveTowards(rt.anchoredPosition, finalPos, Time.deltaTime * 100 * speed);

                if (rt.anchoredPosition == finalPos)
                {
                    WindowState = State.Inside;
                    Animating = false;
                }
            }
            else if(WindowState == State.MovingOut)
            {
                rt.anchoredPosition = Vector3.MoveTowards(rt.anchoredPosition, originalPos, Time.deltaTime * 100 * speed);

                if (rt.anchoredPosition == originalPos)
                {
                    WindowState = State.Outside;
                    Animating = false;
                }
            }
        }
    }

    public void StartAnimation()
    {
        if (Animating) return;
        if (WindowState == State.Inside)
        {
            WindowState = State.MovingOut;
            Animating = true;
        }
        else if (WindowState == State.Outside)
        {
            WindowState = State.MovingIn;
            Animating = true;
        }
    }

}
