using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject hazardPrefab;
    public int maxHazardsToSpawn = 3;
    public TMPro.TextMeshProUGUI scoreText;
    public GameObject pauseMenu;
    public CinemachineVirtualCamera mainVCam;
    public CinemachineVirtualCamera zoomVCam;
    public GameObject gameOverMenu;
    public RectTransform scoreRectTransform;
    public AudioSource musicSource;
    public AudioSource loseSound;

    public static GameManager Instance => gameManagerInstance;
    private static GameManager gameManagerInstance;

    public int HighScore => highScore;

    private int score;
    private int highScore;
    private float timer;
    private bool gameOver = false;
    private const string HighScorePreferenceKey = "HighScore";
    private float hazardSpawnDelay;


    // Start is called before the first frame update
    void Start()
    {
        gameManagerInstance = this;

        hazardSpawnDelay = 1.2f;

        highScore = PlayerPrefs.GetInt(HighScorePreferenceKey);

        musicSource.Play();

        scoreRectTransform.anchoredPosition = new Vector2(scoreRectTransform.anchoredPosition.x, 0);

        scoreRectTransform.LeanMoveY(-72f, 0.75f).setEaseOutBounce();
        //Start spawning hazards
        StartCoroutine(SpawnHazards());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
            {
                //Unpause the game
                UnPauseGame();
            }
            if (Time.timeScale == 1)
            {
                //Pause the game
                PauseGame();
            }
        }
        if (gameOver == true)
            return;

            timer += Time.deltaTime;

            if (timer >= 1f)
            {
                score++;
                scoreText.text = score.ToString();

                DecreaseHazardSpawnDelay(0.01f);

                timer = 0;
            }
    }

    public void PauseGame()
    {
        
        if(pauseMenu.activeSelf == true)
        {
            LeanTween.value(0, 1, 0.3f).setOnUpdate(SetTimeScale).setIgnoreTimeScale(true);
            pauseMenu.SetActive(false);
        }
        else
        {
            LeanTween.value(1, 0, 0.3f).setOnUpdate(SetTimeScale).setIgnoreTimeScale(true);
            pauseMenu.SetActive(true);
        } 
    }

    public void UnPauseGame()
    {
        LeanTween.value(0, 1, 0.3f).setOnUpdate(SetTimeScale).setIgnoreTimeScale(true);
        pauseMenu.SetActive(false);
    }

    private void SetTimeScale(float value)
    {
        Time.timeScale = value;
        Time.fixedDeltaTime = 0.02f * value;
    }

    private IEnumerator SpawnHazards()
    {
        var hazardsToSpawn = Random.Range(1, maxHazardsToSpawn);

        for (int i = 0; i < hazardsToSpawn; i++)
        {
            var randPositionX = Random.Range(-9, 9);
            var randomDrag = Random.Range(0f, 2f);

            var hazardObject = Instantiate(hazardPrefab, new Vector3(randPositionX, 11, 0), Quaternion.identity);

            hazardObject.GetComponent<Rigidbody>().drag = randomDrag;
        }

        yield return new WaitForSeconds(hazardSpawnDelay);

        yield return SpawnHazards();
    }

    //IEnumerator ScaleTime(float start, float end, float duration)
    //{
    //    //Time since the game started
    //    float lastTime = Time.realtimeSinceStartup;
    //    //Timer
    //    float timer = 0.0f;

    //    //While the timer is less than the duration
    //    while (timer < duration)
    //    {
    //        //Interpolate start and end value based on the duration timer
    //        Time.timeScale = Mathf.Lerp(start, end, timer / duration);
    //        //Multiply fixed delta time by time scale
    //        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    //        timer += (Time.realtimeSinceStartup - lastTime);
    //        lastTime = Time.realtimeSinceStartup;
    //        yield return null;
    //    }
    //    Time.timeScale = end;
    //    Time.fixedDeltaTime = 0.02f * end;
    //}

    public void GameOver()
    {
        gameOver = true;

        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }

        loseSound.Play();

        //If game is paused
        if (Time.timeScale < 1)
        {
            LeanTween.value(Time.timeScale, 1, 0.3f).setOnUpdate(SetTimeScale).setIgnoreTimeScale(true);
            pauseMenu.SetActive(false);
        }

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(HighScorePreferenceKey, highScore);
        }

        mainVCam.gameObject.SetActive(false);
        zoomVCam.gameObject.SetActive(true);

        gameObject.SetActive(false);
        gameOverMenu.SetActive(true);
    }

    public int GetHighScore()
    {
        return highScore;
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void DecreaseHazardSpawnDelay(float spawnDelay)
    {
        if (hazardSpawnDelay <= 0.4f)
        {
            hazardSpawnDelay = 0.4f;
        }
        else
        {
            hazardSpawnDelay -= spawnDelay;
        }
    }
}
