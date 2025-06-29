using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Resources ��Դ����ģ�������
/// </summary>
public class ResourcesMgr : BaseManager<ResourcesMgr>
{
    private ResourcesMgr() { }

    /// <summary>
    /// ͬ��������Դ�ķ���
    /// </summary>
    /// <typeparam name="T">��Դ����</typeparam>
    /// <param name="path">��Դ·��</param>
    /// <returns></returns>
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    /// <summary>
    /// �첽������Դ�ķ���
    /// </summary>
    /// <typeparam name="T">��Դ����</typeparam>
    /// <param name="path">��Դ·��</param>
    /// <param name="callBack">���ؽ�����Ļص����� ���첽������Դ������Ż����</param>
    public void LoadAsync<T>(string path, UnityAction<T> callBack) where T : Object
    {
        //ͨ��Эͬ�����첽������Դ
        MonoMgr.Instance.StartCoroutine(ReallyLoadAsync<T>(path, callBack));
    }

    private IEnumerator ReallyLoadAsync<T>(string path, UnityAction<T> callBack) where T : Object
    {
        //�첽������Դ
        ResourceRequest rq = Resources.LoadAsync<T>(path);
        //�ȴ���Դ���ؽ����� �Ż����ִ��yield return����Ĵ���
        yield return rq;
        //��Դ���ؽ��� ��ί�д����ⲿ��ί�к���ȥ����ʹ��
        callBack(rq.asset as T);
    }

    /// <summary>
    /// ָ��ж��һ����Դ�ķ���
    /// </summary>
    /// <param name="assetToUnload"></param>
    public void UnLoadAsset(Object assetToUnload)
    {
        Resources.UnloadAsset(assetToUnload);
    }

    /// <summary>
    /// �첽ж�ض�Ӧû��ʹ�õ�Resources��ص���Դ
    /// </summary>
    /// <param name="callback">�ص�����</param>
    public void UnloadUnusedAssets(UnityAction callback)
    {
        MonoMgr.Instance.StartCoroutine(ReallyUnloadUnusedAssets(callback));
    }

    private IEnumerator ReallyUnloadUnusedAssets(UnityAction callback)
    {
        AsyncOperation ao = Resources.UnloadUnusedAssets();
        yield return ao;
        //ж����Ϻ�֪ͨ�ⲿ
        callback();
    }
}
