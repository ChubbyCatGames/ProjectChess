using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScene : MonoBehaviour
{
    private Image image;

    [SerializeField] private Color fadeColor;
    
    void Awake()
    {
        image = GetComponent<Image>();
        image.color = fadeColor;

        StartCoroutine(FadeIn());
    }


    public void CallFadeOut(float time = 0)
    {
        StartCoroutine(FadeOut(time));
    }

    IEnumerator FadeIn()
    {
        for(float i=image.color.a; i>0.2; i -= 0.01f)
        {
            yield return new WaitForSeconds(0.01f);
            image.color = new Color(image.color.r, image.color.g, image.color.b, i);
        }
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);

        image.enabled = false;
    }

    IEnumerator FadeOut(float time)
    {
        yield return new WaitForSeconds(time);

        image.enabled = true;

        for (float i = image.color.a; i < 1; i += 0.02f)
        {
            yield return new WaitForSeconds(0.01f);
            image.color = new Color(image.color.r, image.color.g, image.color.b, i);
        }

        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
    }
}
