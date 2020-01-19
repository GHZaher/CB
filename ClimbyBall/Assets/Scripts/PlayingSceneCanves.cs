using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayingSceneCanves : MonoBehaviour
{
    [SerializeField] private PlayerBall playerBall;
    [SerializeField] private WallGenerator wallGenerator;
    [SerializeField] private Wall wallPrefab;
    [SerializeField] private BackGround backGround;
    [SerializeField] private PlayingSceneManager playingSceneManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject adMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject pauseBtn;
    [SerializeField] private AudioManager audioManager;

    public void Restart()//reset all vars in the playingScene scripts
    {
        PlayPressedBtnSound();
        gameOverMenu.SetActive(false);
        pauseBtn.SetActive(true);
        PlayingSceneManager.playerDied = false;//the ball is inside the screen again
        playerBall.ResetVars();
        wallPrefab.ResetVars();
        wallGenerator.ResetVars();
        playingSceneManager.ResetVars();
        scoreManager.ResetVars();
        backGround.ResetVars();
    }

    public void ShowAdMenu()
    {
        adMenu.SetActive(true);
    }

    public void HideAdMenu()
    {
        adMenu.SetActive(false);
    }

    public void ShowOptionsMenu()
    {
        PlayPressedBtnSound();
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void BackFromOptionsMenu()
    {
        PlayPressedBtnSound();
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void ShowPauseMenu()
    {
        PlayPressedBtnSound();
        //Time.timeScale = 0;
        playingSceneManager.PauseTheGame();
        pauseMenu.SetActive(true);
    }

    public void BackFromPauseMenu()
    {
        PlayPressedBtnSound();
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void ShowGameOverMenu()
    {
        gameOverMenu.SetActive(true);
        pauseBtn.SetActive(false);
    }

    public void BackToStartScreen(int sceneNumber)
    {
        PlayPressedBtnSound();
        scoreManager.SavePlayerScore();
        Restart();//restart the game vars if we went back to the start scene
        SceneManager.LoadScene(sceneNumber);
    }

    public void PlayPressedBtnSound()
    {
        audioManager = FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
        audioManager.PlayPressedBtnAudio();
    }
}
