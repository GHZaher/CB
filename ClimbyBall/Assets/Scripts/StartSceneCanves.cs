using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AT = Enums.BallsName.BallNamesEnum;
public class StartSceneCanves : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private GameObject optionMenu;
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject ballsMenu;
    [SerializeField] private GameObject[] usedBallPanels;
    [SerializeField] private GameObject lockedPanel;
    [SerializeField] private TextMeshProUGUI lockedBallTxt;
    [Header("Sprites")]
    [SerializeField] private Sprite muteMusicSprite;
    [SerializeField] private Sprite playMusicSprite;
    [SerializeField] private Sprite muteSoundSprite;
    [SerializeField] private Sprite playSoundSprite;
    [Header("Buttons")]
    [SerializeField]
    private Transform buttonStart;
    [SerializeField]
    private Transform buttonOptions;
    [SerializeField]
    private GameObject buttonMusicControl;
    [SerializeField]
    private GameObject buttonSoundsControl;
    [SerializeField]
    private GameObject buttonBackFromOptions;
    [SerializeField]
    private Transform buttonExit;
    [Header("Text")]
    [SerializeField]
    private TextMeshProUGUI highScoreText;
    [SerializeField]
    private float gearsRotateSpeed;
    [SerializeField] private GameObject ballAdBtn;
    private Ball choosedBall;
    private float topScore;
    private UnlockedBallsFile unlockedBallsFile = new UnlockedBallsFile();
    private AdManager adManager;

    public Ball ChoosedBall
    {
        get
        {
            return choosedBall;
        }
    }

    private void Start()
    {
        print(Application.persistentDataPath);

        if (SceneManager.GetActiveScene().name == "StartScene")//to get the high score
        {
            topScore = ScoreManager.GetTopScore();
            highScoreText.text = "High Score:" + topScore;//show top score on the screen
        }

        if (audioManager.IsMusicsMuted)
        {
            buttonMusicControl.GetComponent<Image>().sprite = muteMusicSprite;
        }
        if (audioManager.IsSoundsMuted)
        {
            buttonSoundsControl.GetComponent<Image>().sprite = muteSoundSprite;
        }

        if (PlayerPrefs.HasKey("BallName"))//if the player choosed a ball
        {
            string ballName = SaveFiles.GetStringPlayerPref("BallName");//get the choosen bal name

            RemoveUsedBallPanel();
            for (int i = 0; i < usedBallPanels.Length; i++)
            {
                if (usedBallPanels[i].transform.parent.GetComponent<Ball>().BallName.ToString() == ballName)
                {
                    usedBallPanels[i].SetActive(true);
                }
            }
        }
    }

    private void FixedUpdate()//rotate the gears
    {
        if (SceneManager.GetActiveScene().name == "StartScene")//to put spriets in option menu for music and sound buttons in corrent sprite even after moving to outher scene
        {
            buttonStart.Rotate(0, 0, gearsRotateSpeed);
            buttonOptions.Rotate(0, 0, -gearsRotateSpeed);
            buttonExit.Rotate(0, 0, gearsRotateSpeed);
        }
        buttonMusicControl.transform.Rotate(0, 0, gearsRotateSpeed);
        buttonSoundsControl.transform.Rotate(0, 0, -gearsRotateSpeed);
        buttonBackFromOptions.transform.Rotate(0, 0, gearsRotateSpeed);
    }

    public void PlayMainScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void ShowOptionsMenu()
    {
        PlayPressedBtnSound();
        startMenu.SetActive(false);
        optionMenu.SetActive(true);
    }

    public void BackFromOptionMenu()
    {
        PlayPressedBtnSound();
        startMenu.SetActive(true);
        optionMenu.SetActive(false);
    }

    public void PlayORMuteMusic()
    {
        audioManager = FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
        Sprite buttonSprite = buttonMusicControl.GetComponent<Image>().sprite;

        if (buttonSprite == playMusicSprite)
        {
            PlayPressedBtnSound();
            buttonMusicControl.GetComponent<Image>().sprite = muteMusicSprite;
            audioManager.MuteMainMusic();
        }
        else if (buttonSprite == muteMusicSprite)
        {
            buttonMusicControl.GetComponent<Image>().sprite = playMusicSprite;
            audioManager.UnMuteMainMusic();
        }
    }

    public void PlayORMuteSounds()
    {
        audioManager = FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
        Sprite buttonSprite = buttonSoundsControl.GetComponent<Image>().sprite;

        if (buttonSprite == playSoundSprite)
        {
            buttonSoundsControl.GetComponent<Image>().sprite = muteSoundSprite;
            audioManager.MuteSounds();
        }
        else if (buttonSprite == muteSoundSprite)
        {
            buttonSoundsControl.GetComponent<Image>().sprite = playSoundSprite;
            audioManager.UnMuteSounds();
            PlayPressedBtnSound();
        }
    }

    public void PlayPressedBtnSound()
    {
        audioManager = FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
        audioManager.PlayPressedBtnAudio();
    }

    public void ShowBallsMenu()
    {
        startMenu.SetActive(false);
        ballsMenu.SetActive(true);
    }

    public void BackFromBallsMenu()
    {
        ballsMenu.SetActive(false);
        startMenu.SetActive(true);
    }

    public void SetBallSprite(Ball ball)
    {
        choosedBall = ball;//set the coosed ball which the player choosed it

        if (ball.IsAdNeeded)
        {
            ShowLockedPanel(ball);//show this ball is locked because of not enough score
            print("AdNeeded");
        }
        else if (!ball.IsAdNeeded && ball.BallScoreOpen == 0)
        {
            unlockedBallsFile.BallsLockStates[ball.BallName] = true;//unlock the ball
            SaveFiles.SaveObjectAsJSONAtPersDataPath(unlockedBallsFile, SaveFilesName.UnlockedBalls);//save the new ball lock state
            RemoveUsedBallPanel();//remove the UsedBallPanel from the other balls
            ball.UsedBallPanel.SetActive(true);//active the UsedBallPanel of this ball
            SaveFiles.SetStringPlayerPref("BallName", ball.BallName.ToString());
            print("BallChoosed");
        }
        else
        {
            if (topScore >= ball.BallScoreOpen)//if he has enough score then use this ball
            {
                RemoveUsedBallPanel();//remove the UsedBallPanel from the other balls
                ball.UsedBallPanel.SetActive(true);//active the UsedBallPanel of this ball
                SaveFiles.SetStringPlayerPref("BallName", ball.BallName.ToString());
                print("BallChoosed");
            }
            else
            {
                ShowLockedPanel(ball);//show this ball is locked because of not enough score
                print("BallLocked");
            }
        }
    }

    public void ShowLockedPanel(Ball ball)//to show the locked panel of the balls
    {
        if (ball.IsAdNeeded)
        {
            lockedBallTxt.text = "Unlock Ball ?";
            lockedPanel.SetActive(true);
            ballAdBtn.SetActive(true);
        }
        else
        {
            lockedBallTxt.text = "This ball will be opened when you reach the Score:" + ball.BallScoreOpen;
            lockedPanel.SetActive(true);
        }
    }

    public void HideLockedPanel()//to show the locked panel of the balls
    {
        lockedPanel.SetActive(false);
    }

    public void RemoveUsedBallPanel()//used to remove the UsedBallPanel from the other balls
    {
        for (int i = 0; i < usedBallPanels.Length; i++)
        {
            usedBallPanels[i].SetActive(false);
        }
    }

    public void AskedForAd()
    {
        adManager = FindObjectOfType<AdManager>();
        adManager.ShowRewardedVideo(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}