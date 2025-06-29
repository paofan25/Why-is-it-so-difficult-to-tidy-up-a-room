using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCount : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI textContent;

    [SerializeField] private int maxCount=10;
    public int currentCount;

    private int previousCount;

    public Animator countAnimator;
    private Test test;

    public GameObject failPanel;

    public bool isPlayingAnim;
    // Start is called before the first frame update
    void Start()
    {
        test = FindObjectOfType<Test>();
        currentCount = test.currentRound;
        
        maxCount = FindObjectOfType<Test>().maxcount;
    }

    // Update is called once per frame
    void Update()
    {


        currentCount = test.currentRound;

        
        textContent.text = $"{maxCount-currentCount}步";
        if (maxCount - currentCount <= 0) {
            failPanel.SetActive(true);
            StartCoroutine(WaitAndRestart());
        }
    }

    
    IEnumerator WaitAndRestart(){
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void SetMaxCount(int count){
        maxCount = count;
    }

    public void SetCurrentCount(int count){
        currentCount = count;
    }
}
