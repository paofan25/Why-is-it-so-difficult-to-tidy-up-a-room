using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ����װ�ز�ͬ���������¼���Ϣ�ĸ���
/// </summary>
public abstract class EventInfoBase { }
/// <summary>
/// ���ڰ�����Ӧ�۲��� ί�к��� ����
/// </summary>
/// <typeparam name="T">�����������</typeparam>
public class EventInfo<T> : EventInfoBase
{
    public UnityAction<T> actions;
}
/// <summary>
/// ���ڰ����޲��޷���ֵ ί�к��� ����
/// </summary>
public class EventInfo : EventInfoBase
{
    public UnityAction actions;
}

public class EventCenter : BaseManager<EventCenter>
{
    //���ڼ�¼��Ӧ�¼�����Ӧ���¼�
    private Dictionary<E_EventType,EventInfoBase> eventDic = new Dictionary<E_EventType, EventInfoBase>();
    private EventCenter() { }


    /// <summary>
    /// �¼���������
    /// </summary>
    /// <param name="eventName">�¼���</param>
    /// <param name="obj">����Ĳ���</param>
    public void EventTrigger<T>(E_EventType eventName,T obj)
    {
        (eventDic[eventName] as EventInfo<T>).actions?.Invoke(obj);
    }

    /// <summary>
    /// �¼��������� �޲�
    /// </summary>
    /// <param name="eventName">�¼���</param>
    public void EventTrigger(E_EventType eventName)
    {
        (eventDic[eventName] as EventInfo).actions?.Invoke();
    }

    /// <summary>
    /// �¼���Ӽ����ߵķ���
    /// </summary>
    /// <param name="eventName">�¼���</param>
    /// <param name="func">����</param>
    public void AddEventListener<T>(E_EventType eventName,UnityAction<T> func)
    {
        //�������ڸ��ֵ� ����
        if (!eventDic.ContainsKey(eventName))
        {
            eventDic.Add(eventName, new EventInfo<T>());
        }
        //Ϊ�¼���Ӻ���
        (eventDic[eventName] as EventInfo<T>).actions += func;
    }

    /// <summary>
    /// �¼���Ӽ����ߵķ���
    /// </summary>
    /// <param name="eventName">�¼���</param>
    /// <param name="func">����</param>
    public void AddEventListener(E_EventType eventName, UnityAction func)
    {
        //�������ڸ��ֵ� ����
        if (!eventDic.ContainsKey(eventName))
        {
            eventDic.Add(eventName, new EventInfo());
        }
    //Ϊ�¼���Ӻ���
    (eventDic[eventName] as EventInfo).actions += func;
    }

    /// <summary>
    /// �¼�ɾ�������ߵķ���
    /// </summary>
    /// <param name="eventName">�¼���</param>
    /// <param name="func">����</param>
    public void RemoveEventListener<T>(E_EventType eventName,UnityAction<T> func)
    {
        if (eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo<T>).actions -= func;
        }
    }

    /// <summary>
    /// �¼�ɾ�������ߵķ���
    /// </summary>
    /// <param name="eventName">�¼���</param>
    /// <param name="func">����</param>
    public void RemoveEventListener(E_EventType eventName, UnityAction func)
    {
        if (eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo).actions -= func;
        }
    }

    /// <summary>
    /// ɾ�������¼������ߵ����з���
    /// </summary>
    public void Clear()
    {
        eventDic.Clear();
    }

    /// <summary>
    /// ɾ��ָ���¼������м���
    /// </summary>
    /// <param name="eventName">�¼���</param>
    public void Clear(E_EventType eventName)
    {
        if (eventDic.ContainsKey(eventName))
        {
            eventDic.Remove(eventName);
        }
    }
}
