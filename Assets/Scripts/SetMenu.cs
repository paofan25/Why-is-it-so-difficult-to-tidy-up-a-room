using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetMenu : MonoBehaviour
{
    public static SetMenu Instance { get; private set; }

    // public Slider masterSlider;

    // public Slider musicSlider;
    //
    // public Slider sfxSlider;
    

    

    // [SerializeField] private GameObject setMenu;

    // [SerializeField] private GameObject quitBtn;
    private void Awake(){
        if (Instance != null && Instance != this) {
            Destroy(gameObject); // 防止重复
            return;
        }

        Instance = this;
    }

    void Start(){
        // 初始化 slider 值，直接从 AudioManager 获取
        // masterSlider.value = AudioManager.Instance.GetMasterVolume();
        // // musicSlider.value = AudioManager.Instance.GetMusicVolume();
        // // sfxSlider.value = AudioManager.Instance.GetSFXVolume();
        //
        // masterSlider.onValueChanged.AddListener(AudioManager.Instance.SetMasterVolume);
        // musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
        // sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);
    
    
        // quitBtn.GetComponent<Button>().onClick.AddListener(Quit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Quit(){
        // setMenu.SetActive(false);
    }
}
