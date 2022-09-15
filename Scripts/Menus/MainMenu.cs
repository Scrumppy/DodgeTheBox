using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private AudioSource musicSource; 

    private void Awake()
    {
#if UNITY_EDITOR
        Application.targetFrameRate = 45;
#endif
    }

    private void Start()
    {
        musicSource.Play();

        GetComponentInChildren<TMPro.TextMeshProUGUI>().gameObject.LeanScale(new Vector3(1.2f, 1.2f), 0.5f).setLoopPingPong();

        if (Time.timeScale < 1 && Time.fixedDeltaTime < 0.02f)
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f;
        }
    }

    public void Play()
    {
        GetComponent<CanvasGroup>().LeanAlpha(0, 0.35f).setOnComplete(OnComplete);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnComplete()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
