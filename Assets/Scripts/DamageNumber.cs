using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DamageNumber : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;
    private RectTransform rt;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 0);
    }
    public void DamageNumberAnimation(float value)
    {
        textMesh.text = "-" + value.ToString();
        StartCoroutine(ShowNumberAnimation());
    }

    IEnumerator ShowNumberAnimation()
    {
        textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 1);

        for (float i=0; i<2; i+=0.02f)
        {
            yield return new WaitForSeconds(0.007f);
            rt.offsetMax = new Vector2(rt.offsetMax.x, i);
        }

        textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 0);

    }
}
