using UnityEngine;

[System.Serializable]
public class SoundEntry
{
    [Tooltip("需求描述，例如 局内-脚步")] public string key;

    [Tooltip("是否循环")] public bool loop;

    [Tooltip("用于随机的 AudioClip 集合")] public AudioClip[] clips;
}