using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveSmoothly : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public RectTransform rectTransform;

    private Vector3 startPosition;

    public Vector3 offSet;
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

    public void OnPointerEnter(PointerEventData eventData){
        rectTransform.DOAnchorPos(startPosition+offSet, duration).SetEase(Ease.OutQuad);
    }

    public void OnPointerExit(PointerEventData eventData){
        rectTransform.DOAnchorPos(startPosition, duration).SetEase(Ease.OutQuad);
    }
}
