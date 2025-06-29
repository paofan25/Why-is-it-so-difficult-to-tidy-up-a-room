using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class BasePanel : MonoBehaviour
{
    /// <summary>
    /// ���ڴ洢����Ҫ�õ���UI�ؼ� �������滻ԭ�� UIBehaviourװ��
    /// </summary>
    protected Dictionary<string,UIBehaviour> controlDic = new Dictionary<string,UIBehaviour>();

    /// <summary>
    /// �ؼ�Ĭ������ ��������Ҳ��������ʹ��
    /// </summary>
    private static List<string> defaultNameList = new List<string>() { "Image",
                                                                   "Text (TMP)",
                                                                   "RawImage",
                                                                   "Background",
                                                                   "Checkmark",
                                                                   "Label",
                                                                   "Text (Legacy)",
                                                                   "Arrow",
                                                                   "Placeholder",
                                                                   "Fill",
                                                                   "Handle",
                                                                   "Viewport",
                                                                   "Scrollbar Horizontal",
                                                                   "Scrollbar Vertical"};

    protected virtual void Awake()
    {
        //Ϊ�˱��� ĳһ�������ϴ������ֿؼ������
        //����Ӧ�����Ȳ�����Ҫ�����
        FindChildrenControl<Button>();
        FindChildrenControl<Toggle>();
        FindChildrenControl<Slider>();
        FindChildrenControl<InputField>();
        FindChildrenControl<ScrollRect>();
        FindChildrenControl<Dropdown>();
        //��ʹ�����Ϲ����˶����� ֻҪ�����ҵ�����Ҫ���
        //֮��Ҳ����ͨ����Ҫ����õ������������ص�����
        FindChildrenControl<Text>();
        FindChildrenControl<TextMeshPro>();
        FindChildrenControl<Image>();
    }

    /// <summary>
    /// �����ʾʱ���õĺ���
    /// </summary>
    public abstract void ShowMe();

    /// <summary>
    /// �������ʱ���õĺ���
    /// </summary>
    public abstract void HideMe();

    /// <summary>
    /// ��ȡָ�������Լ�ָ�����͵����
    /// </summary>
    /// <typeparam name="T">�������</typeparam>
    /// <param name="name">�������</param>
    /// <returns></returns>
    public T GetControl<T>(string name)where T : UIBehaviour
    {
        if (controlDic.ContainsKey(name))
        {
            T control = controlDic[name] as T;
            if(control == null)
            {
                Debug.LogError($"�����ڶ�Ӧ����{name},����Ϊ{typeof(T)}�����");
            }
            return controlDic[name] as T;
        }
        else{
            Debug.LogError($"�����ڶ�Ӧ����{name}�����");
            return null;
        }
    }

    protected virtual void ClickBtn(string btnName) { }

    protected virtual void SliderValueChange(string SliderName, float value) { }

    protected virtual void ToggleValueChange(string SliderName, bool value) { }

    private void FindChildrenControl<T>() where T : UIBehaviour
    {
        T[] controls = GetComponentsInChildren<T>();
        for (int i = 0; i < controls.Length; i++)
        {
            //��ȡ��ǰ�ؼ���
            //���ڽ�� Ϊ��ť������¼����� ������
            string controlName = controls[i].gameObject.name;

            //����ť��¼���ֵ���
            //����ֵ��в�����ͬ����key
            if (!controlDic.ContainsKey(controlName))
            {
                //�������Ĭ�����ӵ��ֵ���
                if (!defaultNameList.Contains(controlName))
                {
                    controlDic.Add(controlName, controls[i]);

                    //�жϿؼ������� �����Ƿ���¼�����
                    if (controls[i] is Button)
                    {
                        (controls[i] as Button).onClick.AddListener(() =>
                        {
                            ClickBtn(controlName);
                        });
                    }
                    else if (controls[i] is Slider)
                    {
                        (controls[i] as Slider).onValueChanged.AddListener((value) =>
                        {
                            SliderValueChange(controlName, value);
                        });
                    }
                    else if (controls[i] is Toggle)
                    {
                        (controls[i] as Toggle).onValueChanged.AddListener((value) =>
                        {
                            ToggleValueChange(controlName, value);
                        });
                    }
                }
                    
            }
        }
    }
}
