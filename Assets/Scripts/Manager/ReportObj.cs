using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReportObj : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI id;
    [SerializeField] TextMeshProUGUI name;
    [SerializeField] TextMeshProUGUI description;
    private int idInt;

    private ReportsScript reportsScript;
    // Start is called before the first frame update
    void Awake()
    {
        id.text = "";
        name.text = "";
        description.text = "";
    }

    public void Ban()
    {
        reportsScript.Ban(idInt);
    }

    public void SetReportScript(ReportsScript a)
    {
        reportsScript= a;
    }
    public void SetId(int i)
    {
        idInt = i;
        id.text = i.ToString();
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
