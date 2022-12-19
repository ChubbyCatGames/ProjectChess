using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class ManagerScript : MonoBehaviour
{
    [SerializeField] string filePath;
    [SerializeField] GameObject prefabText;
    [SerializeField] Transform initialPos;

    [Header("InputFields")]
    [SerializeField] TMP_InputField inputName;
    [SerializeField] TMP_InputField inputPass;
    [SerializeField] TMP_InputField inputDeleteName;

    // Start is called before the first frame update
    void Start()
    {
        if (!System.IO.File.Exists(filePath+"/admins.json"))
        {
            filePath += "/admins.json";
            // Create a list of admins
            List<Admin> admins = new List<Admin>()
            {
                new Admin {Username = "admin", Password = "admin"}
            };
            // Serialize the list of admins to JSON
            string json = JsonConvert.SerializeObject(admins, Formatting.Indented);
            System.IO.File.WriteAllText(filePath, json);
        }
        else
        {
            filePath += "/admins.json";
        }
    }


    public void CreateAnAdmin()
    {
        // Create a list of admins

        Admin newAdmin = new Admin
        {
            Username = inputName.text,
            Password = inputPass.text
        };
        string json = System.IO.File.ReadAllText(filePath);
        List<Admin> admins = JsonConvert.DeserializeObject<List<Admin>>(json);
        admins.Add(newAdmin);
        // Serialize the list of admins to JSON
        json = JsonConvert.SerializeObject(admins, Formatting.Indented);

        // Write the JSON string to a file
        System.IO.File.WriteAllText(filePath, json);

        Debug.Log("JSON file created successfully.");
    }

    public void ListAdmins()
    {
        string json = System.IO.File.ReadAllText(filePath);
        List<Admin> admins = JsonConvert.DeserializeObject<List<Admin>>(json);
        int i = 0;
        foreach(var admin in admins)
        {
            var newAdmin = Instantiate(prefabText, initialPos.position, Quaternion.identity) as GameObject;
            newAdmin.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
            newAdmin.transform.position = new Vector3(initialPos.position.x, initialPos.position.y - (i), initialPos.position.z);
            NamingList nl = newAdmin.GetComponent<NamingList>();
            Debug.Log(admin.Username);
            nl.SetName(admin.Username);
            i++;
        }

    }

    public void DeleteAdmins()
    {
        string json = System.IO.File.ReadAllText(filePath);
        List<Admin> admins = JsonConvert.DeserializeObject<List<Admin>>(json);

        for (int i = 0; i < admins.Count; i++)
        {
            if (admins[i].Username == inputDeleteName.text)
            {
                admins.RemoveAt(i);
            }
        }

        // Serialize the list of admins to JSON
        json = JsonConvert.SerializeObject(admins, Formatting.Indented);

        // Write the JSON string to a file
        System.IO.File.WriteAllText(filePath, json);
    }
}


    // Admin class to store username and password
    class Admin
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

