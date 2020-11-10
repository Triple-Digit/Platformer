using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    #region Variables
    

    [Header("UI components")]

    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject winScreen;

    bool b_paused;

    #endregion

    private void Awake()
    {
        instance = this;
    }
    private void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!b_paused)
            {
                Pause();
                b_paused = true;
            }
            else
            {
                Resume();
                b_paused = false;
            }
        }
    }
    private void Start()
    {
        Time.timeScale = 0f;
    }
    public void Begin()
    {
        startScreen.SetActive(false);
        Time.timeScale = 1f;
        b_paused = false;
    }

    public void Win()
    {
        winScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Lose()
    {
        if (!b_paused)
        {
            loseScreen.SetActive(true);
            b_paused = true;
        }
        else return;
            
    }

  


    #region Pause Menu Methods

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }

    #endregion
}
