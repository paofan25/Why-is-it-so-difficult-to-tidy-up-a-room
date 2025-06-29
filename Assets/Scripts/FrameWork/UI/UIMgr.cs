using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// �㼶ö��
/// </summary>
public enum E_UILayer
{
    /// <summary>
    /// �ײ�
    /// </summary>
    Bottom,
    /// <summary>
    /// �в�
    /// </summary>
    Middle,
    /// <summary>
    /// �߲�
    /// </summary>
    top,
    /// <summary>
    /// ϵͳ��
    /// </summary>
    system,
}

/// <summary>
/// UI������
/// ע��:���Ԥ������Ҫ���������һ��
/// </summary>
public class UIMgr : BaseManager<UIMgr>
{
    private Camera uiCamera;
    private Canvas uiCanvas;
    private EventSystem eventSystem;

    //�㼶������
    private Transform bottomLayer;
    private Transform middleLayer;
    private Transform topLayer;
    private Transform systemLayer;

    /// <summary>
    /// ���ڴ洢���е�������
    /// </summary>
    private Dictionary<string,BasePanel> panelDic = new Dictionary<string,BasePanel>();

    private UIMgr() 
    {
        //��̬����Ψһ��Canvas��EventSystem��UI�����

        //ͬ����ȡ����� ʵ���� ��ȡ�����Ԥ�����µ���������
        uiCamera = GameObject.Instantiate(ResourcesMgr.Instance.Load<Camera>("UI/Prefabs/UICamera")).GetComponent<Camera>();
        //UI��������������Ƴ� ר��������ȾUI���
        GameObject.DontDestroyOnLoad(uiCamera.gameObject);

        //��̬����Canvas
        uiCanvas = GameObject.Instantiate(ResourcesMgr.Instance.Load<Canvas>("UI/Prefabs/Canvas")).GetComponent <Canvas>();
        //����ʹ�õ�UI�����
        uiCanvas.worldCamera = uiCamera;
        //ͬ�����������Ƴ�
        GameObject.DontDestroyOnLoad (uiCanvas.gameObject);

        //��̬����EventSystem
        eventSystem = GameObject.Instantiate(ResourcesMgr.Instance.Load<EventSystem>("UI/Prefabs/EventSystem")).GetComponent<EventSystem>();
        //ͬ�����������Ƴ�
        GameObject.DontDestroyOnLoad (eventSystem.gameObject);

        //�ҵ��㼶������
        bottomLayer = uiCanvas.transform.Find("Bottom");
        middleLayer = uiCanvas.transform.Find("Middle");
        topLayer = uiCanvas.transform.Find("Top");
        systemLayer = uiCanvas.transform.Find("System");
    }

    /// <summary>
    /// ��ȡ�㼶�������Transform
    /// </summary>
    /// <param name="layer">�㼶ö��</param>
    /// <returns></returns>
    public Transform GetLayerFather(E_UILayer layer)
    {
        switch (layer)
        {
            case E_UILayer.Bottom:
                return bottomLayer;
            case E_UILayer.Middle:
                return middleLayer;
            case E_UILayer.top:
                return topLayer;
            case E_UILayer.system:
                return systemLayer;
            default:
                return null;
        }
    }

    /// <summary>
    /// ��ʾ���
    /// </summary>
    /// <typeparam name="T">�������</typeparam>
    /// <param name="layer">�����ʾ�㼶 Ĭ��middle</param>
    /// <param name="callback">�������첽���� �ص�����</param>
    /// <param name="isSyns">�Ƿ���ͬ������</param>
    public void ShowPanel<T>(E_UILayer layer = E_UILayer.Middle,UnityAction<T> callback = null,bool isSyns = false) where T : BasePanel
    {
        //��ȡ����� 
        string panelName = typeof(T).Name;
        //�������
        if (panelDic.ContainsKey(panelName))
        {
            //ִ��������ʾ�߼�
            panelDic[panelName].ShowMe();
            //���ڻص����� ���ؼ���
            callback?.Invoke(panelDic[panelName] as T);
            return;
        }
        //���������
        else
        {
            //ͬ������
            if (isSyns)
            {
                GameObject res = ResourcesMgr.Instance.Load<GameObject>("UI/Prefabs/" + panelName);
                //�㼶����
                Transform father = GetLayerFather(layer);
                //����û�а�ָ�����������
                if (father != null)
                {
                    father = middleLayer;
                }
                //�����Ԥ���崴������Ӧ�������� ���ұ���ԭ�������Ŵ�С
                GameObject panelObj = GameObject.Instantiate(res, father, false);
                //��ȡ��ӦUI������س�ȥ
                T panel = panelObj.GetComponent<T>();
                //��ʾ���ʱִ�е�Ĭ�Ϸ���
                panel.ShowMe();
                //����ȥʹ��
                callback?.Invoke(panel);
                //�浽�ֵ���
                panelDic.Add(panelName, panel);
            }
            //�첽����
            else
            {
                ResourcesMgr.Instance.LoadAsync<GameObject>("UI/Prefabs/"+ panelName, (res) =>
                {
                    //�㼶����
                    Transform father = GetLayerFather(layer);
                    //����û�а�ָ�����������
                    if (father != null)
                    {
                        father = middleLayer;
                    }
                    //�����Ԥ���崴������Ӧ�������� ���ұ���ԭ�������Ŵ�С
                    GameObject panelObj = GameObject.Instantiate(res, father,false);
                    //��ȡ��ӦUI������س�ȥ
                    T panel = panelObj.GetComponent<T>();
                    //��ʾ���ʱִ�е�Ĭ�Ϸ���
                    panel.ShowMe();
                    //����ȥʹ��
                    callback?.Invoke(panel);
                    //�浽�ֵ���
                    panelDic.Add(panelName, panel);
                });
            }
        }
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <typeparam name="T">�������</typeparam>
    public void HidePanel<T>() where T : BasePanel
    {
        //��ȡ����� 
        string panelName = typeof(T).Name;
        if (panelDic.ContainsKey(panelName))
        {
            //��������߼�
            panelDic[panelName].HideMe();
            //�������
            GameObject.Destroy(panelDic[panelName].gameObject);
            //���������Ƴ�
            panelDic.Remove(panelName);
        }
    }

    /// <summary>
    /// ��ȡ���
    /// </summary>
    /// <typeparam name="T">�������</typeparam>
    public T GetPanel<T>() where T : BasePanel
    {
        //��ȡ����� 
        string panelName = typeof(T).Name;
        if (panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName] as T;
        }
        return null;
    }
}
