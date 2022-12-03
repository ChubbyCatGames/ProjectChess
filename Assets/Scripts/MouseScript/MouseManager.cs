using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    [SerializeField] public Texture2D mouseText;
    [SerializeField] public Texture2D mouseObject;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(mouseText, new Vector2(1,2), CursorMode.ForceSoftware);

    }

}
