using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MusicMgr.Instance.PlayerBKMusic("Musics/MUS_A");
        MusicMgr.Instance.PlayerBKMusic2("Musics/MUS_B");
        MusicMgr.Instance.PlayerBKMusic3("Musics/MUS_C");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnGUI()
    {
        if(GUI.Button(new Rect(0, 0, 100, 50), "±≥æ∞“Ù2"))
        {
            MusicMgr.Instance.ChangeBKMusicValue2(1);
        }
        if (GUI.Button(new Rect(50, 0, 100, 50), "±≥æ∞“Ù3"))
        {
            MusicMgr.Instance.ChangeBKMusicValue3(1);
        }
    }
}
