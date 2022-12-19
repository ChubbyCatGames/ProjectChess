using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NamingList : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI name;
    [SerializeField] TextMeshProUGUI description;
    // Start is called before the first frame update
    void Awake()
    {
        name.text = "";
        description.text = "";
    }

    public void SetName(string n)
    {
        name.text = n;
    }

    public void SetDescription(string d)
    {
        description.text = d;
    }
}
