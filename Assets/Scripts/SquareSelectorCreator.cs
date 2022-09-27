using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareSelectorCreator : MonoBehaviour
{
    [SerializeField] private Material freeSquareMat;
    [SerializeField] private Material opponentSquareMat;
    [SerializeField] private GameObject prefab;
    private List <GameObject> instantiatedSelectors = new List <GameObject>();

    public void ShowSelection(Dictionary<Vector3,bool> squareData)
    {
        ClearSelection();

        foreach (var data in squareData)
        {
            GameObject selector = Instantiate(prefab, data.Key ,Quaternion.identity);
            instantiatedSelectors.Add(selector);
            foreach (var setter in selector.GetComponentsInChildren<MaterialSetter>())
            {
                setter.SetSingleMaterial(data.Value ? freeSquareMat : opponentSquareMat);
            }
        }
    }

    public void ClearSelection()
    {
        foreach (var selector in instantiatedSelectors)
        {
            Destroy(selector.gameObject);
        }
    }
}
