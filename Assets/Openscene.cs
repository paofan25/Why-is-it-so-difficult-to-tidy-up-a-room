using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Openscene : MonoBehaviour
{
    public Button button1;
    public Button button2;
    // public Button button3;
    // Start is called before the first frame update
    void Start()
    {
        button1.onClick.AddListener(StartGame);
        button2.onClick.AddListener(ExitGame);
    }

    public void StartGame(){
        SceneManager.LoadScene("EmptyScene 1");
    }

    public void ExitGame(){
        Application.Quit();
    }
}
