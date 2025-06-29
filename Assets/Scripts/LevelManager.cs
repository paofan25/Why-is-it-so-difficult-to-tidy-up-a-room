using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private Test test;
    public string sceneToLoad;
    public List<ItemData> items;

    public GameObject successPanel;
    // Start is called before the first frame update
    void Start(){
        test = FindObjectOfType<Test>();
        RandomLoadMap();
    }

    // Update is called once per frame
    void Update()
    {
        if (test.CheckIsOver()) {
            StartCoroutine(Success());
        }
    }

    IEnumerator Success(){
        successPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneToLoad);
    }
    public void RandomLoadMap(){
        test.itemData = items[Random.Range(0, items.Count)];
        test.maxcount = test.itemData.maxCount;
    }
    
}
