using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ������Ч������
/// </summary>
public class MusicMgr : BaseManager<MusicMgr>
{
    //�������ֲ������
    private AudioSource bkMusic = null;
    //�������ִ�С
    private float bkMusicValue = 1f;
    //�������ֲ������
    private AudioSource bkMusic2 = null;
    //�������ִ�С
    private float bkMusicValue2 = 0f;
    //�������ֲ������
    private AudioSource bkMusic3 = null;
    //�������ִ�С
    private float bkMusicValue3 = 0f;

    //�������ڲ��ŵ���Ч
    private List<AudioSource> soundList = new List<AudioSource>();
    //��Ч������С
    private float soundValue = 1f;
    //�ж���Ч�����Ƿ���ͣ
    private bool soundIsPlay = true;

    private MusicMgr() 
    {
        //��Update������MonoMgr���� ʹ��Updateÿִ֡��
        MonoMgr.Instance.AddUpateListener(Update);
    }
    private void Update()
    {
        if (!soundIsPlay) return;

        //��ͣ�ر������� ����Ƿ�����Ч������� �����������
        //Ϊ�˱���߱������Ƴ��������� ���ǲ����������
        for(int i = soundList.Count -1;i>=0;i--)
        {
            if (!soundList[i].isPlaying)
            {
                //��Ч������� ����Ƭ�ƿ�
                soundList[i].clip = null;
                //������뻺�����
                PoolMgr.Instance.PushObj(soundList[i].gameObject);
                //�����List���Ƴ�
                soundList.RemoveAt(i);
            }
        }
    }

    //���ű�������
    public void PlayerBKMusic(string path)
    {
        //���û�й���AudioSource����� ��̬����
        if(bkMusic == null)
        {
            GameObject obj = new GameObject();
            obj.name = "BKMusic";
            GameObject.DontDestroyOnLoad(obj);
            bkMusic = obj.AddComponent<AudioSource>();
        }
        //���ݴ���ı����������� �����ű�������
        ResourcesMgr.Instance.LoadAsync<AudioClip>(path, (AudioClip) =>
        {
            bkMusic.clip = AudioClip;
            bkMusic.loop = true;
            bkMusic.volume = bkMusicValue;
            bkMusic.Play();
        });
    }

    //���ñ������ִ�С
    public void ChangeBKMusicValue(float value)
    {
        bkMusicValue = value;
        if (bkMusic == null) return;
        bkMusic.volume = bkMusicValue;
    }

    //ֹͣ��������
    public void StopBKMusic()
    {
        if (bkMusic == null) return;
        bkMusic.Stop();
    }

    //��ͣ��������
    public void PauseBKMusic()
    {
        if (bkMusic == null) return;
        bkMusic.Pause();
    }

    //���ű�������
    public void PlayerBKMusic2(string path)
    {
        //���û�й���AudioSource����� ��̬����
        if (bkMusic2 == null)
        {
            GameObject obj = new GameObject();
            obj.name = "BKMusic2";
            GameObject.DontDestroyOnLoad(obj);
            bkMusic2 = obj.AddComponent<AudioSource>();
        }
        //���ݴ���ı����������� �����ű�������
        ResourcesMgr.Instance.LoadAsync<AudioClip>(path, (AudioClip) =>
        {
            bkMusic2.clip = AudioClip;
            bkMusic2.loop = true;
            bkMusic2.volume = bkMusicValue2;
            bkMusic2.Play();
        });
    }

    //���ñ������ִ�С
    public void ChangeBKMusicValue2(float value)
    {
        bkMusicValue2 = value;
        if (bkMusic2 == null) return;
        bkMusic2.volume = bkMusicValue2;
    }

    //���ű�������
    public void PlayerBKMusic3(string path)
    {
        //���û�й���AudioSource����� ��̬����
        if (bkMusic3 == null)
        {
            GameObject obj = new GameObject();
            obj.name = "BKMusic3";
            GameObject.DontDestroyOnLoad(obj);
            bkMusic3 = obj.AddComponent<AudioSource>();
        }
        //���ݴ���ı����������� �����ű�������
        ResourcesMgr.Instance.LoadAsync<AudioClip>(path, (AudioClip) =>
        {
            bkMusic3.clip = AudioClip;
            bkMusic3.loop = true;
            bkMusic3.volume = bkMusicValue3;
            bkMusic3.Play();
        });
    }

    //���ñ������ִ�С
    public void ChangeBKMusicValue3(float value)
    {
        bkMusicValue3 = value;
        if (bkMusic3 == null) return;
        bkMusic3.volume = bkMusicValue3;
    }

    /// <summary>
    /// ������Ч
    /// </summary>
    /// <param name="path">��Ч·��</param>
    /// <param name="isLoop">�Ƿ�ѭ��</param>
    /// <param name="isSync">�Ƿ�ͬ������</param>
    /// <param name="callback">���ؽ�����Ļص�</param>
    public void PlaySound(string path,bool isLoop = false,bool isSync = false,UnityAction<AudioSource> callback = null)
    {
        //�ӻ������ȡ����Ч����õ���Ӧ���
        AudioSource source = PoolMgr.Instance.GetObj("Sounds/Prefabs/soundObj").GetComponent<AudioSource>();

        //���ݴ������Ч���� ��������Ч
        if (isSync)
        {
            //ͬ��������Ч��Դ
            AudioClip audioClip = ResourcesMgr.Instance.Load<AudioClip>(path);

            //��Ϊ���ڻ�������� ȡ�����Ŀ����������õ���� ��������ȡ������stop
            source.Stop();

            source.clip = audioClip;
            source.loop = isLoop;
            source.volume = soundValue;
            source.Play();
            //����������¼ ����֮���ж��Ƿ�ֹͣ
            //���ڴӻ������ȡ������ �п���ȡ������ʹ�õĶ��󣨳������ˣ�
            //����Ҫ�ظ�ȥ��Ӽ���
            if(!soundList.Contains(source))
                    soundList.Add(source);
        }
        else
        {
            //�첽������Ч��Դ
            ResourcesMgr.Instance.LoadAsync<AudioClip>(path, (AudioClip) =>
            {
                source.clip = AudioClip;
                source.loop = isLoop;
                source.volume = soundValue;
                source.Play();
                //����������¼ ����֮���ж��Ƿ�ֹͣ
                soundList.Add(source);
                //���ݸ��ⲿʹ��
                callback?.Invoke(source);
            });
        }
    }

    /// <summary>
    /// ֹͣ����ָ����Ч
    /// </summary>
    /// <param name="source">��Ч�������</param>
    public void StopSound(AudioSource source)
    {
        if (soundList.Contains(source))
        {
            //ֹͣ����
            source.Stop();
            //��List���Ƴ�
            soundList.Remove(source);

            //��Ч������� ����Ƭ�ƿ�
            source.clip = null;
            //������뻺�����
            PoolMgr.Instance.PushObj(source.gameObject);
        }
    }

    /// <summary>
    /// �ı���Ч��С
    /// </summary>
    /// <param name="value"></param>
    public void ChangeSoudnValue(float value)
    {
        soundValue = value;
        foreach (AudioSource source in soundList)
        {
            source.volume = value;
        }
    }

    /// <summary>
    /// ���Ż�����ͣ������Ч
    /// </summary>
    /// <param name="isPlay">�Ƿ��Ǽ������� trueΪ���� falseΪ��ͣ</param>
    public void PlayOrPauseSound(bool isPlay)
    {
        if(isPlay)
        {
            foreach (AudioSource source in soundList)
            {
                source.Stop();
            }
        }
        else
        {
            foreach (AudioSource source in soundList)
            {
                source.Pause();
            }

        }
    }

    /// <summary>
    /// �����Ч��ؼ�¼ ������ʱ !!!����ջ����֮ǰȥ����!!!
    /// </summary>
    public void ClearSound()
    {
        for(int i = 0;i<soundList.Count; i++)
        {
            soundList[i].Stop();
            soundList[i].clip = null;
            PoolMgr.Instance.PushObj(soundList[i].gameObject);
        }
        //�����Ч�б�
        soundList.Clear();
    }
}
