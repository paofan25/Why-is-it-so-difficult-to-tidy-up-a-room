using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class SceneButtonPair
{
#if UNITY_EDITOR
    public SceneAsset sceneAsset;   // 可拖入 Scene 资源
#endif
    [HideInInspector] public string sceneName;  // 实际加载用的名称
    public Button button;           // 关联按钮
}

public class ChangeLevelUI : MonoBehaviour
{
    [Header("UI 引用")]
    public GameObject changePanel;
    public Button changeLevelButton;
    public Button exitButton;

    [Header("场景按钮设置")]
    public SceneButtonPair[] sceneButtons; // 每个按钮 + 对应场景

    void Start()
    {
        changePanel.SetActive(false);

        changeLevelButton.onClick.AddListener(() => changePanel.SetActive(true));
        exitButton.onClick.AddListener(() => changePanel.SetActive(false));

        // 初始化每个按钮的点击事件
        foreach (var pair in sceneButtons)
        {
#if UNITY_EDITOR
            if (pair.sceneAsset != null)
            {
                pair.sceneName = AssetDatabase.GetAssetPath(pair.sceneAsset)
                    .Replace("Assets/", "")
                    .Replace(".unity", "");
            }
#endif
            if (pair.button != null && !string.IsNullOrEmpty(pair.sceneName))
            {
                string sceneToLoad = pair.sceneName;
                pair.button.onClick.AddListener(() =>
                {
                    Debug.Log("加载场景：" + sceneToLoad);
                    SceneManager.LoadScene(sceneToLoad);
                });
            }
        }
    }
}
