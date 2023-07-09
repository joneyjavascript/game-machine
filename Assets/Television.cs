using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;

public class Television : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private GameObject pauseScreen;

    public UnityEvent OnVideoEnd = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        pauseScreen = transform.Find("PauseScreen").gameObject;
        videoPlayer.loopPointReached += DispatchVideoEnd;
    }

    void DispatchVideoEnd(VideoPlayer player)
    {
        OnVideoEnd.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        pauseScreen.SetActive(videoPlayer.isPaused);
    }

    public bool isPaused()
    {
        return !videoPlayer.isPlaying;
    }

    public void PlayVideo()
    {
        videoPlayer.Play();
    }

    public void PauseVideo()
    {
        videoPlayer.Pause();
    }

    public void SetPlaybackSpeed(float playbackSpeed)
    {
        videoPlayer.playbackSpeed = playbackSpeed;
    }

    public void SetPlaybackFrameRate(float frameRate = 30)
    {
        float playbackSpeed = frameRate / 30f;
        SetPlaybackSpeed(playbackSpeed);
    }

    public void ResetVideo()
    {
        videoPlayer.time = 0;
    }
}
