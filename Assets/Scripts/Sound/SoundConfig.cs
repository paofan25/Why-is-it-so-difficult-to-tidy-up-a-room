using UnityEngine;

[CreateAssetMenu(fileName = "SoundConfig", menuName = "Audio/SoundConfig")]
public class SoundConfig : ScriptableObject
{
    public SoundEntry[] soundEntries;

    public SoundEntry GetEntryByKey(string key){
        foreach (var entry in soundEntries) {
            if (entry.key == key)
                return entry;
        }
        Debug.LogWarning($"SoundConfig: 没有找到 Key [{key}] 对应的音效");
        return null;
    }
}