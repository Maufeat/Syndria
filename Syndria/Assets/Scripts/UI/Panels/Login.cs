using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Login : UIPanel
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayMusic(Resources.Load<AudioClip>("Sounds/BGM/bgm_sakura"));
        AudioManager.Instance.SetMusicVolume(0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
