using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetMenu : MonoBehaviour
{
    [SerializeField]private GameObject setMenu;
    [SerializeField]private GameObject quitBtn;
    // Start is called before the first frame update
    void Start()
    {
        quitBtn.GetComponent<Button>().onClick.AddListener(Quit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Quit(){
        setMenu.SetActive(false);
    }
}
