using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGMClip
{
    SceneBegin,

    levelPaet1,
    levelPaet2,
}

public enum WhiteNoiseClip
{
    SceneBegin,

    levelPaet1,

    levelPaet2,
}

public enum SFXClip
{
    Jump,
    Move,
}

public class AudioManager : MonoBehaviour
{

    [Header("Audio Sources")] public AudioSource whiteNoiseSource;

    public AudioSource bgmSource;

    public AudioSource[] sfxSources;

    private int sfxIndex = 0;

    [Header("Audio Clips")] public List<BGMEntry> bgmClips;

    public List<WhiteNoiseEntry> whiteNoiseClips;

    public List<SFXEntry> sfxClips;

    private Dictionary<BGMClip, BGMEntry> bgmDict;

    private Dictionary<WhiteNoiseClip, WhiteNoiseEntry> whiteNoiseDict;

    private Dictionary<SFXClip, SFXEntry> sfxDict;

    [System.Serializable]
    public class BGMEntry
    {
        public BGMClip key;

        public AudioClip clip;

        [Range(0, 1)] public float volume;
    }

    [System.Serializable]
    public class WhiteNoiseEntry
    {
        public WhiteNoiseClip key;

        public AudioClip clip;

        [Range(0, 1)] public float volume;
    }

    [System.Serializable]
    public class SFXEntry
    {
        public SFXClip key;

        public List<AudioClip> clips;

        [Range(0, 1)] public float volume;

        public bool loop;
    }

    private Dictionary<BGMClip, AudioSource> bgmSourceDict = new();

    private Dictionary<BGMClip, AudioSource> bgmGallaryDict = new();

    private Dictionary<WhiteNoiseClip, AudioSource> whiteNoiseSourceDict = new();

    //private Dictionary<SFXClip, AudioSource> sfxSourceDict = new();
    private Dictionary<SFXClip, Dictionary<string, AudioSource>> sfxSourceDict = new();

    public static AudioManager Instance { get; private set; }

    private float masterVolume;

    private float musicVolume;

    private float sfxVolume;

    private const string MASTER_KEY = "MasterVolume";

    private const string MUSIC_KEY = "MusicVolume";

    private const string SFX_KEY = "SFXVolume";
    private void Awake(){
        if (Instance != null && Instance != this) {
            Destroy(gameObject); // 防止重复
            return;
        }

        Instance = this;
        masterVolume = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
        musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);
    }

    public float GetMasterVolume() => masterVolume;
    public float GetMusicVolume() => musicVolume;

    public float GetSFXVolume() => sfxVolume;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetMasterVolume(float value){
        masterVolume = value;
        PlayerPrefs.SetFloat(MASTER_KEY, value);
        PlayerPrefs.Save();
        ApplyVolumes();
    }

    public void SetMusicVolume(float value){
        musicVolume = value;
        PlayerPrefs.SetFloat(MUSIC_KEY, value);
        PlayerPrefs.Save();
        ApplyVolumes();
    }

    public void SetSFXVolume(float value){
        sfxVolume = value;
        PlayerPrefs.SetFloat(SFX_KEY, value);
        PlayerPrefs.Save();
        ApplyVolumes();
    }

    public void PlayRandomSFX(SFXClip clipKey){
        if (!sfxDict.TryGetValue(clipKey, out var entry)) {
            Debug.LogWarning($"SFXClip {clipKey} not found!");
            return;
        }

        if (entry.clips == null || entry.clips.Count == 0) {
            Debug.LogWarning($"SFXClip {clipKey} has no clips!");
            return;
        }

        // 随机选一个
        AudioClip randomClip = entry.clips[Random.Range(0, entry.clips.Count)];

        // 找可用 AudioSource
        var source = sfxSources[sfxIndex];
        source.clip = randomClip;
        source.volume = entry.volume * sfxVolume * masterVolume;
        source.loop = entry.loop;
        source.Play();

        sfxIndex = (sfxIndex + 1) % sfxSources.Length;
    }

    public void SetBGMVolume(float masterVolume, float bgmVolume){
        foreach (var pair in bgmDict) {
            if (bgmSourceDict.TryGetValue(pair.Key, out var source)) {
                float baseVolume = pair.Value.volume;
                source.volume = baseVolume * bgmVolume * masterVolume;
            }
        }

        foreach (var pair in whiteNoiseDict) {
            if (whiteNoiseSourceDict.TryGetValue(pair.Key, out var source)) {
                float baseVolume = pair.Value.volume;
                source.volume = baseVolume * bgmVolume * masterVolume;
            }
        }
    }

    public void SetSFXVolume(float masterVolume, float sfxVolume){
        foreach (var clipPair in sfxSourceDict) {
            var clipKey = clipPair.Key;

            // 获取每个 SFXClip 的基础 volume（配置）
            if (!sfxDict.TryGetValue(clipKey, out var entry)) continue;
            float baseVolume = entry.volume;

            foreach (var instancePair in clipPair.Value) {
                var source = instancePair.Value;
                if (source == null) continue;

                source.volume = baseVolume * sfxVolume * masterVolume;
            }
        }
    }
    private void ApplyVolumes(){
        if (AudioManager.Instance != null) {
            AudioManager.Instance.SetBGMVolume(masterVolume, musicVolume);
            AudioManager.Instance.SetSFXVolume(masterVolume, sfxVolume);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
