using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    public Button button;
    private void Awake()
    {
        button.onClick.AddListener(() =>
        {
            AllSceneMgr.Instance.StartGame();
        });
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
