using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Զ�����ʽ�ļ̳�Mono�ĵ���ģʽ����(�Ƽ�)
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonAutoMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                //��̬���� ��̬����
                //�ڳ����ϴ���������
                GameObject obj = new GameObject();
                //�õ�T�ű������� Ϊ�������
                obj.name = typeof(T).ToString();
                //��̬���ض�Ӧ�ĵ���ģʽ�ű�
                instance = obj.AddComponent<T>();
                //��֤�ö���һֱ����
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }
}
