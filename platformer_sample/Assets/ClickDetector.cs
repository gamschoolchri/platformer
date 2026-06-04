using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickDetector : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private string myObject;
    void Start()
    {
        myObject = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {

        if (myObject == "Start_Botton")
        {
            Debug.Log("Scene Changed");
            SceneManager.LoadScene("Scene_onPlay");
        }
        if (myObject == "Quit_Botton")
        {
            Debug.Log("Quited");
            Application.Quit();
        }
    }
}
