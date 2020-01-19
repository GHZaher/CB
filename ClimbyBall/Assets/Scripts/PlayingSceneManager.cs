using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayingSceneManager : MonoBehaviour
{
    public static bool startFlag = false;//true when we click the first time on screen
    public static bool playerDied = false;//true if ball went out of the screen
    public static bool gamePaused = false;//pause game when we have ad
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private PlayingSceneCanves playingSceneCanves;
    [SerializeField] private GameObject loseFlash;
    private float flashAlphaDecreaser;
    [SerializeField] private float startingFlashAlpha;
    //[SerializeField] private GameObject pressToStartTxt;
    [SerializeField] private GraphicRaycaster canves;//the canves which we do not want to move the ball if we are clicking on one of it's UI elements(this needed for androied devices only because the other methods work only on the unity editor)
    [SerializeField] private AdManager adsManager;
    [SerializeField] private WallGenerator wallGenerator;
    private string isItAuthorized;//if it was "true" then the game will work fine and if it was "false" the game will close(Anti Cheat)

    public GameObject LoseFlash
    {
        get 
        { 
            return loseFlash; 
        }
        set
        { 
            loseFlash = value;
        }
    }


    private void Awake()
    {
        StartCoroutine(checkInternetConnection());//check if it is authorized or not
    }

    private void Start()
    {
        
        flashAlphaDecreaser = startingFlashAlpha;
        //pressToStartTxt.SetActive(true);
    }

    private void Update()
    {
        print(startFlag);
        if (Input.GetMouseButtonDown(0) && !playerDied && !IsTouchingOnUI() && !gamePaused)//if we pressed and the player was not died and we are not pressing on UI element
        {
            startFlag = true;//start the game
        }
    }

    private void FixedUpdate()
    {
        if (playerDied)
        {
            flashAlphaDecreaser = flashAlphaDecreaser - 1 * Time.deltaTime;
            ShowLoseFlash();
        }
        else
        {
            loseFlash.SetActive(false);
        }
    }

    public void LoseTheGame()//called when lose
    {
        wallGenerator.HideAllWalls();
        playerDied = true;//ball went out of the screen
        startFlag = false;
        UnPauseTheGame();

        scoreManager.SavePlayerScore();
        playingSceneCanves.ShowGameOverMenu();
        
        playingSceneCanves.HideAdMenu();
        //playingSceneCanves.ShowGameOverMenu();
    }

    //public void EndTheGame()//to end the game if the player did not watch an ad
    //{
    //    playerDied = true;//ball went out of the screen
    //    startFlag = false;

    //    scoreManager.SavePlayerScore();
    //    playingSceneCanves.ShowGameOverMenu();
    //    //StartCoroutine(ShowLoseFlash());//show flash light
    //    ShowLoseFlash();//show flash light
    //}

    public void AskedForAdd()//called when the player wants to watch an add an revive
    {
        wallGenerator.PauseWallsMovement();//to pause the generated walls movement after reviving the player untill he start playing again(after watching ad)
        adsManager = FindObjectOfType<AdManager>();
        adsManager.ShowRewardedVideo();
        startFlag = false;
    }

    public void PauseTheGame(float waitSecs = 0)//to pause the game
    {
        StartCoroutine(WaitForSec(waitSecs));
    }

    public IEnumerator WaitForSec(float waitSecs = 0)
    {
        yield return new WaitForSeconds(waitSecs);
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
    }

    public void UnPauseTheGame()//to unpause the game
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

    public void ResetVars()
    {
        startFlag = false;
        flashAlphaDecreaser = startingFlashAlpha;//reset flash alpha decreaser
        loseFlash.GetComponent<Image>().color = new Color(1, 1, 1, 1);//reset flash alpha
    }

    public void ShowLoseFlash()
    {
        print("EnteredFlash");
        loseFlash.SetActive(true);
        if (loseFlash.GetComponent<Image>().color.a > 0)
        {
            print("Flash");
            loseFlash.GetComponent<Image>().color = new Color(1, 1, 1, flashAlphaDecreaser);
        }
        else
        {
            loseFlash.SetActive(false);
        }
    }

    public bool IsTouchingOnUI()//to know if we are touching on canves Ui elemnt
    {
        PointerEventData ped = new PointerEventData(null);
        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        canves.Raycast(ped, results);
        if (results.Count == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    #region Authorization
    public IEnumerator GetAuthorization()
    {
        UnityWebRequest myHttpWebRequest = UnityWebRequest.Get("http://ph-scince.000webhostapp.com/zaherGame/getPermission.php");
        yield return myHttpWebRequest.Send();
        isItAuthorized = myHttpWebRequest.downloadHandler.text;

        if (isItAuthorized == "false")//if it was not authorized
        {
            SaveFiles.SetStringPlayerPref("Authorization", "false");
            print("Not Authorized  " + isItAuthorized);
            Application.Quit();//close the game
        }
        else if (isItAuthorized == "true")
        {
            SaveFiles.SetStringPlayerPref("Authorization", "true");
            print("Authorized  " + isItAuthorized);
        }
    }

    public IEnumerator checkInternetConnection()
    {
        WWW www = new WWW("http://ph-scince.000webhostapp.com/zaherGame/getPermission.php");
        yield return www;
        if (www.error != null)
        {
            print("Disconnected");

            isItAuthorized = SaveFiles.GetStringPlayerPref("Authorization");
            if (isItAuthorized == "false")
            {
                Application.Quit();//close the game
                print("NotAuthorized  " + isItAuthorized);
            }
            else if (isItAuthorized == "true")
            {
                print("Authorized  " + isItAuthorized);
            }
        }
        else//if there was an internet connection the git datetime from internet
        {
            print("connected");
            StartCoroutine(GetAuthorization());
        }
        yield return null;
    }
    #endregion
}
