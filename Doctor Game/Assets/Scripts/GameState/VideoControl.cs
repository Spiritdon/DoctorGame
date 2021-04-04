using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoControl : MonoBehaviour
{
    public VideoPlayer video;
    public GameObject panel;

    float timeBuffer = 0.5f;

    // Update is called once per frame
    void Update()
    {
        timeBuffer -= Time.deltaTime;
        if (!video.isPlaying && timeBuffer <= 0)
        {
            SceneManager.LoadScene("EndGameInfo");
        }
        else
        {
            panel.SetActive(false);
        }
    }
}
