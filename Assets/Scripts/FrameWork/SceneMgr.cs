using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneMgr : BaseManager<SceneMgr>
{
    private SceneMgr() { }

    //ͬ���л���������
    public void LoadScene(string name,UnityAction callback = null)
    {
        //�л�����
        SceneManager.LoadScene(name);
        //���ûص�
        callback?.Invoke();
    }

    //�첽�л���������
    public void LoadSceneAsyn(string name,UnityAction callback = null)
    {
        MonoMgr.Instance.StartCoroutine(ReallyLoadSceneAsyn(name, callback));
    }

    private IEnumerator ReallyLoadSceneAsyn(string name, UnityAction callback = null)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);
        //��ͣ����Эͬ������ÿ֡����Ƿ���ؽ��� ������ؽ����Ͳ�������ѭ��ÿ֡������
        while(!ao.isDone)
        {
            //�����������¼����� ÿһ֡�����ȷ��͸���Ҫ�õ��ĵط�
            EventCenter.Instance.EventTrigger<float>(E_EventType.E_SceneLoadChange, ao.progress);
            yield return 0;
        }
        //�������һֱ֡�ӽ����� û��ͬ��1��ȥ
        EventCenter.Instance.EventTrigger<float>(E_EventType.E_SceneLoadChange, 1);

        //���ؽ���
        callback?.Invoke();
    }
}
