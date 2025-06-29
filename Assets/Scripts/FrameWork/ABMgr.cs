using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ABMgr : SingletonAutoMono<ABMgr>
{
    //����
    private AssetBundle mainAB = null;
    //��������������Ϣ
    private AssetBundleManifest manifest = null;
    //���ع���AB��
    private Dictionary<string,AssetBundle> abDic = new Dictionary<string,AssetBundle>();
    /// <summary>
    /// AB�����·��
    /// </summary>
    private string abPath
    {
        get
        {
            return Application.streamingAssetsPath + "/";
        }
    }
    /// <summary>
    /// ������
    /// </summary>
    private string MainABName
    {
        get
        {   
#if UNITY_IOS
            return "IOS";
#elif UNITY_ANDROID
            return "Android"
#elif UNITY_STANDALONE_WIN
            return "PC";
#else
            return "PC";
#endif
        }
    }
    #region ͬ������

    /// <summary>
    /// ���������������ļ��Լ�����
    /// </summary>
    /// <param name="abName"></param>
    private void LoadAB(string abName)
    {
        //���������������ļ�
        if (mainAB == null)
        {
            mainAB = AssetBundle.LoadFromFile(abPath + MainABName);

        }
        if (manifest == null)
        {
            manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        string[] strs = manifest.GetAllDependencies(abName);
        for (int i = 0; i < strs.Length; i++)
        {
            if (!abDic.ContainsKey(strs[i]))
            {
                abDic.Add(strs[i], AssetBundle.LoadFromFile(abPath + strs[i]));
            }
        }
        //���ذ�
        if (!abDic.ContainsKey(abName))
        {
            AssetBundle ab = AssetBundle.LoadFromFile(abPath + abName);
            abDic.Add(abName, ab);
        }
    }
    /// <summary>
    /// ͬ������ ��ָ������
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <returns>Object����</returns>
    public Object LoadRes(string abName, string resName)
    {
        LoadAB(abName);
        //������Դ
        Object obj = abDic[abName].LoadAsset(resName);
        //�����ж�
        if (obj is GameObject)
        {
            return Instantiate(obj) as GameObject;
        }
        else
        {
            return obj;
        }
    }
    /// <summary>
    /// ͬ������ ָ������
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="type"></param>
    /// <returns>Object����</returns>
    public Object LoadRes(string abName, string resName, System.Type type)
    {
        LoadAB(abName);
        //������Դ
        Object obj = abDic[abName].LoadAsset(resName, type);
        //�����ж�
        if (obj is GameObject)
        {
            return Instantiate(obj) as GameObject;
        }
        else
        {
            return obj;
        }
    }

    /// <summary>
    /// ͬ������ ����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="type"></param>
    /// <returns>T����</returns>
    public T LoadRes<T>(string abName, string resName, System.Type type) where T: Object
    {
        LoadAB(abName);
        //������Դ
        T obj = abDic[abName].LoadAsset<T>(resName);
        //�����ж�
        if (obj is GameObject)
        {
            return Instantiate(obj);
        }
        else
        {
            return obj;
        }
    }

    #endregion

    #region �첽����

    /// <summary>
    /// �첽���� ��ָ������
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="callback"></param>
    public void LoadResAsync(string abName,string resName,UnityAction<Object> callback)
    {
        StartCoroutine(ReallyLoadResAsync(abName, resName, callback));
    }

    private IEnumerator ReallyLoadResAsync(string abName, string resName, UnityAction<Object> callback)
    {
        LoadAB(abName);
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName);
        yield return abr;
        if (abr.asset is GameObject)
        {
            callback(Instantiate(abr.asset) as GameObject);
        }
        else
        {
            callback(abr.asset);
        }
    }

    /// <summary>
    /// �첽���� ָ������
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="type"></param>
    /// <param name="callback"></param>
    public void LoadResAsync(string abName, string resName, System.Type type, UnityAction<Object> callback)
    {
        StartCoroutine(ReallyLoadResAsync(abName, resName, type, callback));
    }

    private IEnumerator ReallyLoadResAsync(string abName, string resName, System.Type type, UnityAction<Object> callback)
    {
        LoadAB(abName);
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName,type);
        yield return abr;
        if (abr.asset is GameObject)
        {
            callback(Instantiate(abr.asset) as GameObject);
        }
        else
        {
            callback(abr.asset);
        }
    }

    /// <summary>
    /// �첽���� ����
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="type"></param>
    /// <param name="callback"></param>
    public void LoadResAsync<T>(string abName, string resName, UnityAction<T> callback) where T : Object
    {   
        StartCoroutine(ReallyLoadResAsync<T>(abName, resName, callback));
    }

    private IEnumerator ReallyLoadResAsync<T>(string abName, string resName, UnityAction<T> callback) where T : Object
    {
        LoadAB(abName);
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync<T>(resName);
        yield return abr;
        if (abr.asset is GameObject)
        {   
            callback(Instantiate(abr.asset) as T);
        }   
        else
        {
            callback(abr.asset as T);
        }
    }

    #endregion

    #region ����ж��

    /// <summary>
    /// ж�ص�����
    /// </summary>
    /// <param name="abName"></param>
    public void UnLoadRes(string abName)
    {
        if (abDic.ContainsKey(abName))
        {
            abDic[abName].Unload(false);
            abDic.Remove(abName);
        }
    }

    /// <summary>
    /// ж�����а���
    /// </summary>
    public void ClearAB()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        abDic.Clear();
        mainAB = null;
        manifest = null;
    }

    #endregion
}
