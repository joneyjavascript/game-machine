using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static AudioSource musicAudioSource;
    public static GameManager instance;
    public string playingMusic = "";

    void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayMusic("ZigZag");
    }

    // Update is called once per frame
    void Update() { }

    public void GoToThanks()
    {
        GoToScene("Thanks");
    }

    public void GoToPlayGame()
    {
        GoToScene("Gameplay");
    }

    public void GoToCredits()
    {
        GoToScene("Credits");
    }

    public void GoToScene(string sceneName)
    {
        PlayMusic("ZigZag");
        SceneManager.LoadScene(sceneName);
    }

    public void GoToScene(SceneName sceneName)
    {
        string name = GetSceneName(sceneName);
        GoToScene(name);
    }

    public string GetSceneName(SceneName sceneName)
    {
        Dictionary<SceneName, string> dictionary = new Dictionary<SceneName, string>();
        dictionary.Add(SceneName.Menu, "Intro");
        dictionary.Add(SceneName.Credits, "Credits");
        dictionary.Add(SceneName.Gameplay, "Gameplay");
        dictionary.Add(SceneName.Thanks, "Thanks");

        string result = "";
        dictionary.TryGetValue(sceneName, out result);

        return result;
    }

    public void PlayMusic(string musicName)
    {
        if (playingMusic == musicName)
        {
            return;
        }

        if (musicAudioSource == null)
        {
            musicAudioSource = gameObject.AddComponent<AudioSource>();
        }

        playingMusic = musicName;

        AudioClip music = Resources.Load<AudioClip>(musicName);
        musicAudioSource.clip = music;
        musicAudioSource.Play();
    }

    public void PlaySoundEffect(string soundEffectName)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        AudioClip sfx = Resources.Load<AudioClip>(soundEffectName);
        source.PlayOneShot(sfx);

        StartCoroutine(
            WaitAndDo(
                () =>
                {
                    DestroyImmediate(source);
                },
                10
            )
        );
    }

    IEnumerator WaitAndDo(Action callback, float seconds = 1)
    {
        yield return new WaitForSeconds(seconds);
        callback();
        yield return null;
    }
}
