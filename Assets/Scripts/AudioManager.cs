using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class AudioManager : SingletonMonoBehaviour<AudioManager> {
    private const int SE_NUM = 14;

    private AudioSource bgmSource;
    private List<AudioSource> seSourceList = new List<AudioSource>();

    private Dictionary<string,AudioClip> bgmDic = new Dictionary<string,AudioClip>();
    private Dictionary<string,AudioClip> seDic = new Dictionary<string,AudioClip>();

    //=============================================================
    private void Init () {
        //SE、BGMの数分だけAudioSourceを追加
        for(int i = 0;i < SE_NUM + 1;i++) {
            gameObject.AddComponent<AudioSource>();
        }

        AudioSource[] audioSourceArray = GetComponents<AudioSource>();

        for(int i = 0;i < audioSourceArray.Length;i++) {
            audioSourceArray[i].playOnAwake = false;

            //BGM、SE設定
            if(i == 0) {
                audioSourceArray[i].loop = true;
                bgmSource = audioSourceArray[i];
            } else {
                seSourceList.Add(audioSourceArray[i]);
            }
        }

        object[] bgmData = Resources.LoadAll("Audio/BGM");
        object[] seData = Resources.LoadAll("Audio/SE");

        foreach(AudioClip bgm in bgmData) {
            bgmDic[bgm.name] = bgm;
        }

        foreach(AudioClip se in seData) {
            seDic[se.name] = se;
        }
    }

    private void Awake () {
        //Instance化をすでにしてるなら
        if(this != Instance) {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
        Init();
    }

    //=============================================================
    /// <summary>
    /// seをならす
    /// </summary>
    public void PlaySE (string name) {
        if(!seDic.ContainsKey(name)) {
            ErrorManager.ErrorLog(ErrorManager.ERROR_CODE.AUDIO_SE_NONE);
            return;
        }

        foreach(AudioSource se in seSourceList) {
            se.PlayOneShot(seDic[name] as AudioClip);
            return;
        }
    }

    //=============================================================
    /// <summary>
    /// bgmをならす
    /// </summary>
    public void PlayBGM (string name) {
        if(!bgmDic.ContainsKey(name)) {
            ErrorManager.ErrorLog(ErrorManager.ERROR_CODE.AUDIO_SE_NONE);
            return;
        }

        bgmSource.clip = bgmDic[name] as AudioClip;
        bgmSource.Play();
    }

    //=============================================================
    /// <summary>
    /// bgmを止める
    /// </summary>
    public void StopBGM () {
        bgmSource.Stop();
    }
}