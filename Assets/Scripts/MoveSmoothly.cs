using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveSmoothly : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    public RectTransform rectTransform;

    private Vector3 startPosition;

    private Vector3 offSet;
    public float duration=0.5f;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData){
        rectTransform.DOAnchorPos(startPosition+offSet, duration).SetEase(Ease.OutQuad);
    }

    public void OnPointerUp(PointerEventData eventData){
        rectTransform.DOAnchorPos(startPosition, duration).SetEase(Ease.OutQuad);
    }
}
