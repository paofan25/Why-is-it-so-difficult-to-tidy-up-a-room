using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance{ get; private set; }

    void Awake(){
        if (Instance != null && Instance != this) {
            Destroy (gameObject);
            return;
        }
        Instance = this;
    }
    [SerializeField] private string FirstLevel;

    // [SerializeField] private GameObject startButton;
    // [SerializeField] private GameObject exitButton;
    // [SerializeField] private GameObject setButton;
    // [SerializeField] private GameObject setPanel;
    [SerializeField] private Button resume;
    [SerializeField] private Button exit;

    public GameObject mainMenu;
    
    // Start is called before the first frame update
    void Start()
    {
        // AudioManager.Instance.PlayRandomSFX(SFXClip.Jump);
        AudioManager.Instance.PlaySoundByKey("BGM-1");
        // startButton.GetComponent<Button>().onClick.AddListener(StartGame);
        // exitButton.GetComponent<Button>().onClick.AddListener(QuitGame);
        // setButton.GetComponent<Button>().onClick.AddListener(SetPanel);
        resume.onClick.AddListener(Resume);
        exit.onClick.AddListener(QuitGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Resume(){
        mainMenu.SetActive(false);
    }
    public void StartGame(){
        SceneManager.LoadScene(FirstLevel);
    }

    public void QuitGame(){
        Application.Quit();
        Debug.Log("Quit Game");
    }

    public void SetPanel(){
        // setPanel.SetActive(true);
    }
}
