using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelCount : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI textContent;

    [SerializeField] private int maxCount;
    public int currentCount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textContent.text = $"{currentCount}/{maxCount}";
    }

    public void SetMaxCount(int count){
        maxCount = count;
    }

    public void SetCurrentCount(int count){
        currentCount = count;
    }
}
