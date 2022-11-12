using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject music;

    private void Awake()
    {
        //-----------Play Music control----------
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");

        if (objs.Length > 1)
        {
            Destroy(objs[1].gameObject);
        }

        if (objs.Length != 0) music = objs[0];

        if (music != null)
        {
            DontDestroyOnLoad(music);
            if (!music.GetComponent<AudioSource>().isPlaying) music.GetComponent<AudioSource>().Play();
        }
        //---------------------------------------------
    }

    public void GameScene(string nameScene)
    {
        //------------Music things-------------
        if (nameScene == "FirstTry") //Change this value according to the name of the Ingame scene
            if(music != null)
                music.GetComponent<AudioSource>().Stop();
        //--------------------------------------

        SceneManager.LoadScene(nameScene);
    }

    public void QuitGame()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
