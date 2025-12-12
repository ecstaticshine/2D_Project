using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance = null;

    private AudioSource bgmSource;
    private AudioSource sfxSource;

    [System.Serializable]
    public class BGMClip
    {
        public string name;
        public AudioClip clip;
    }

    [System.Serializable]
    public class SoundEffect
    {
        public string name;
        public AudioClip clip;
    }

    public List<BGMClip> bgmClips = new List<BGMClip>();
    private Dictionary<string, AudioClip> bgmDict;

    public List<SoundEffect> sfxClips = new List<SoundEffect>();
    private Dictionary<string, AudioClip> sfxDict;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);// 로드되는 동안은 파괴시키지 마세요.

            if (bgmSource == null)
                bgmSource = gameObject.AddComponent<AudioSource>();
            if (sfxSource == null)
                sfxSource = gameObject.AddComponent<AudioSource>();

            Init();

            bgmSource.volume = PlayerPrefs.GetFloat("BGMVolume", 1f);
            sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        }
        else
        {

            Destroy(gameObject);

        }
    }

    private void Init()
    {
        bgmDict = new Dictionary<string, AudioClip>();
        foreach (var item in bgmClips)
        {
            if (!bgmDict.ContainsKey(item.name) && item.clip != null)
            {
                bgmDict[item.name] = item.clip;
            }

        }
        sfxDict = new Dictionary<string, AudioClip>();
        foreach (var sfx in sfxClips)
        {
            if (!sfxDict.ContainsKey(sfx.name) && sfx.clip != null)
            {
                sfxDict[sfx.name] = sfx.clip;
            }

        }

        // 씬 로드 시마다 자동으로 BGM 변경
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void PlayBGM(string name)
    {
        if (bgmDict.ContainsKey(name))
        {
            bgmSource.clip = bgmDict[name];
            bgmSource.loop = true;
            bgmSource.Play();
        }
        else
        {
            Debug.Log($"[AudioManager] BGM '{name}' not found!");
        }
    }

    public void PlaySFX(string name)
    {
        if (sfxDict.ContainsKey(name))
        {
            sfxSource.PlayOneShot(sfxDict[name]);
        }
        else
        {
            Debug.Log($"[AudioManager] SFX '{name}' not found!");
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬 이름으로 BGM 전환
        switch (scene.name)
        {
            case "Title":
                PlayBGM("Title");
                break;
            case "PrepareStage":
                PlayBGM("PrepareStage");
                break;
            case "Stage":
                PlayBGM("Stage");
                break;
            case "Clear":
                PlayBGM("Clear");
                break;
            default:
                StopBGM();
                break;
        }
    }

    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
        PlayerPrefs.SetFloat("BGMVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }
}
