using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllSceneMgr : BaseManager<AllSceneMgr>
{
    private AllSceneMgr() { }
    public int curScene = 0;
    public int maxScene = 5;
    public void Reset()
    {
        MonoMgr.Instance.StartCoroutine(GoNextIE(true));
    }
    public void GoNext()
    {
        curScene++;
        if(curScene > maxScene)
        {
            SceneManager.LoadScene("BeginScene");
        }
        MonoMgr.Instance.StartCoroutine(GoNextIE(false));
    }
    IEnumerator GoNextIE(bool b)
    {
        if (b)
        {
            yield return null;
        }
        else
        {
            yield return new WaitForSeconds(2f);
        }
        SceneManager.LoadSceneAsync("EmptyScene " + curScene.ToString());
        //SceneMgr.Instance.LoadSceneAsyn("EmptyScene " + curScene.ToString());
    }
    public void StartGame()
    {
        curScene++;
        SceneManager.LoadSceneAsync("EmptyScene 1");
    }
}
