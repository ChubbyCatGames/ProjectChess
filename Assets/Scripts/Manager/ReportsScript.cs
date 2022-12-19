using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportsScript : MonoBehaviour
{
    [SerializeField] string filePath;

    [SerializeField] GameObject prefabReport;
    [SerializeField] Transform initialPos;
    private List<GameObject> gameObjects = new List<GameObject>();

    void Start()
    {
        if (!System.IO.File.Exists(filePath + "/reports.json"))
        {
            filePath += "/reports.json";
            // Create a list of admins
            List<Report> reports = new List<Report>()
            {
                new Report { id = Random.Range(0,9999), Username = "JuanLoko32", Description = "Me robó una pieza de la nada y consiguió acabar la partida" },
                new Report { id = Random.Range(0,9999),Username = "BangUpBenito", Description = "Sabe jugar mejor que yo" },
                new Report { id = Random.Range(0,9999),Username = "AbominableAlbert", Description = "Me cae mal" },
                new Report { id = Random.Range(0,9999),Username = "Carly_The_Fancy", Description = "sakkozni szent háborút" },
                new Report { id = Random.Range(0,9999),Username = "Gerbob", Description = "Bloqueó mi juego con un script y terminó mi partida" },
                new Report { id = Random.Range(0,9999),Username = "C.J", Description = "yo how does this work?" },
                new Report { id = Random.Range(0,9999),Username = "Dunnomeister", Description = "lass ihn nicht mehr spielen" }
            };

            // Serialize the list of admins to JSON
            string json = JsonConvert.SerializeObject(reports, Formatting.Indented);
            System.IO.File.WriteAllText(filePath, json);
        }
        else
        {
            filePath += "/reports.json";
        }
    }
    public void ListReports()
    {
        string json = System.IO.File.ReadAllText(filePath);
        List<Report> reports = JsonConvert.DeserializeObject<List<Report>>(json);
        int i = 0;
        foreach (var report in reports)
        {
            var newReport = Instantiate(prefabReport, initialPos.position, Quaternion.identity) as GameObject;
            newReport.transform.SetParent(GameObject.FindGameObjectWithTag("Reports").transform, false);
            newReport.transform.position = new Vector3(initialPos.position.x, initialPos.position.y - (i*2f), initialPos.position.z);
            gameObjects.Add(newReport);
            ReportObj ro = newReport.GetComponent<ReportObj>();
            ro.SetName(report.Username);
            ro.SetDescription(report.Description);
            ro.SetId(report.id);
            ro.SetReportScript(this);
            i++;
        }

    }

    public void DeleteReports()
    {
        foreach(var report in gameObjects)
        {
            Destroy(report.gameObject);
        }
        gameObjects.Clear();
    }
    internal void Ban(int id)
    {
        string json = System.IO.File.ReadAllText(filePath);
        List<Report> reports = JsonConvert.DeserializeObject<List<Report>>(json);
        for (int i = 0; i < reports.Count; i++)
        {
            if (reports[i].id == id)
            {
                reports.Remove(reports[i]);
            }
        }
        // Serialize the list of admins to JSON
        json = JsonConvert.SerializeObject(reports, Formatting.Indented);
        System.IO.File.WriteAllText(filePath, json);
        DeleteReports();
        ListReports();
    }
}
// Report class to store the reported username and description
class Report
{
    public int id { get; set; }
    public string Username { get; set; }
    public string Description { get; set; }
}
