using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    [SerializeField] private Texture2D mouseText;
    // Start is called before the first frame update
    void Start()
    {
        //Cursor.SetCursor(mouseText, Vector2.zero, CursorMode.ForceSoftware);
    }

}
