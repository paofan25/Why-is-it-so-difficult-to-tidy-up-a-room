using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("全局音源")]
    public AudioSource bgmSource;
    public AudioSource ambienceSource;
    public AudioSource sfxSource;
    public AudioSource uiSource;

    [Header("配置文件")]
    public SoundConfig soundConfig;

    [Header("音量控制")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    public Slider volumeSlider;

    private const string VolumeKey = "MasterVolume";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadVolumeSetting();
            ApplyVolume();

            if (volumeSlider != null)
            {
                volumeSlider.value = masterVolume;
                volumeSlider.onValueChanged.AddListener(SetMasterVolume);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region 通用播放接口

    public void PlaySoundByKey(string key){
        if (soundConfig == null) {
            Debug.LogError("未配置 SoundConfig！");
            return;
        }

        SoundEntry entry = soundConfig.GetEntryByKey(key);
        if (entry == null || entry.clips.Length == 0) {
            Debug.LogWarning($"没有找到可用音效：{key}");
            return;
        }

        AudioClip clip = entry.clips[Random.Range(0, entry.clips.Length)];

        if (entry.loop) {
            // 🗂️ 根据 key 前缀做简单路由
            if (key.StartsWith("BGM")) {
                bgmSource.clip = clip;
                bgmSource.loop = true;
                bgmSource.Play();
            }
            else if (key.StartsWith("环境") || key.StartsWith("Ambience") || key.Contains("局内")) {
                ambienceSource.clip = clip;
                ambienceSource.loop = true;
                ambienceSource.Play();
            }
            else {
                // 默认走 BGM Source
                bgmSource.clip = clip;
                bgmSource.loop = true;
                bgmSource.Play();
            }
        }
        else {
            // 非循环 => 当做 SFX 或 UI 音效播放
            if (key.StartsWith("UI-")) {
                uiSource.PlayOneShot(clip);
            }
            else {
                sfxSource.PlayOneShot(clip);
            }
        }
    }


    #endregion

    #region 音量

    public void SetMasterVolume(float value)
    {
        masterVolume = Mathf.Clamp01(value);
        ApplyVolume();
        SaveVolumeSetting();
    }

    private void ApplyVolume()
    {
        bgmSource.volume = masterVolume;
        ambienceSource.volume = masterVolume;
        sfxSource.volume = masterVolume;
        uiSource.volume = masterVolume;
    }

    private void SaveVolumeSetting()
    {
        PlayerPrefs.SetFloat(VolumeKey, masterVolume);
        PlayerPrefs.Save();
    }

    private void LoadVolumeSetting()
    {
        if (PlayerPrefs.HasKey(VolumeKey))
            masterVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);
    }

    #endregion
}
