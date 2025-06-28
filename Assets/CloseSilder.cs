using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CloseSilder : MonoBehaviour,IPointerExitHandler
{
    public GameObject silder;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerExit(PointerEventData eventData){
        silder.SetActive(false);
    }
}
