using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OpenSilder : MonoBehaviour,IPointerDownHandler
{
    public GameObject silder;

    public bool isOpen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData){
        silder.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData){
        silder.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData){
        if(!isOpen){
            silder.SetActive(true);
            isOpen = true;
        }
        else {
            silder.SetActive(false);
            isOpen = false;
        }
        
    }
}
