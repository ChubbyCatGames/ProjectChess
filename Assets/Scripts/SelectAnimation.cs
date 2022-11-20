using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAnimation : MonoBehaviour
{
    private Vector3 originalScale;
    private void Awake()
    {
        originalScale = gameObject.transform.localScale;
    }

    public void StartSelectAnimation(float sizeIncreaseMultiplier)
    {
        StartCoroutine(Animate(sizeIncreaseMultiplier * originalScale.x));
    }

    IEnumerator Animate(float size)
    {
        print(originalScale.x + ", " + size);
        for(float i=originalScale.x; i<size; i += 0.1f * originalScale.x)
        {
            yield return new WaitForSeconds(0.015f);
            gameObject.transform.localScale = new Vector3(i, i, i);
        }

        transform.localScale = new Vector3(size, size, size);

        print(originalScale.x + ", " + transform.localScale.x);

        for (float i = transform.localScale.x; i > originalScale.x; i -= 0.1f * originalScale.x)
        {
            print("HOLAAA");
            yield return new WaitForSeconds(0.015f);
            gameObject.transform.localScale = new Vector3(i, i, i);
        }
        transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
    }
}
