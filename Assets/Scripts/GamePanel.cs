using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    [SerializeField] private Button Restart;
    [SerializeField] private Button Exit;
    [SerializeField] private GameObject mainMenu;
    // Start is called before the first frame update
    void Start()
    {
        Restart.onClick.AddListener(RestartGame);
        Exit.onClick.AddListener(ExitGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame(){
        mainMenu.SetActive(true);
    }
}
