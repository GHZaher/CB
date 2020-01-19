using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class RewardedAds : MonoBehaviour, IUnityAdsListener
{

    private string gameId = "1234567";//Game ID for the wanted store and you get it from your Unity Dashboard
    private string myPlacementId = "rewardedVideo";//type of the ad
    private bool testMode = false;

    /*[SerializeField]*/ private PlayingSceneCanves playingSceneCanves;
    /*[SerializeField]*/ private PlayingSceneManager playingSceneManager;
    /*[SerializeField]*/ private StartSceneCanves startSceneCanves;
    /*[SerializeField]*/ private PlayerBall playerBall;
    /*[SerializeField]*/ private OpenBallsManager openBallsManager;
    private bool isBallAd;//true if this ad wanted to open a ball
    private static bool instance = false;

    public bool IsBallAd
    {
        set
        {
            isBallAd = value;
        }
        get
        {
            return isBallAd;
        }
    }


    // Initialize the Ads listener and service:
    void Start()
    {
        if (!instance && SceneManager.GetActiveScene().name == "StartScene")
        {
            instance = true;

            isBallAd = false;
            Advertisement.AddListener(this);
            Advertisement.Initialize(gameId, testMode);
            DontDestroyOnLoad(gameObject);
        }
        else if (SceneManager.GetActiveScene().name == "StartScene")
        {
            Destroy(gameObject);
        }
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        playingSceneCanves = FindObjectOfType<PlayingSceneCanves>();
        playingSceneManager = FindObjectOfType<PlayingSceneManager>();
        playerBall = FindObjectOfType<PlayerBall>();
        startSceneCanves = FindObjectOfType<StartSceneCanves>();
        openBallsManager = FindObjectOfType<OpenBallsManager>();

        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
            if (!isBallAd)
            {
                print("Player Finished the ad");
                playerBall.RevivePlayer();
                playingSceneCanves.HideAdMenu();
            }
            else
            {
                startSceneCanves.ChoosedBall.IsAdNeeded = false;
                startSceneCanves.SetBallSprite(startSceneCanves.ChoosedBall);//open the choosed ball if he watced the ad succesfuly
                startSceneCanves.HideLockedPanel();
            }
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
            if (!isBallAd)
            {
                print("Player Skipped the ad");
                playingSceneManager.LoseTheGame();
                playingSceneCanves.HideAdMenu();
            }
            else
            {
                startSceneCanves.HideLockedPanel();
            }
        }
        else if (showResult == ShowResult.Failed)
        {
            if (!isBallAd)
            {
                Debug.LogWarning("The ad did not finish due to an error.");
                playingSceneManager.LoseTheGame();
                playingSceneCanves.HideAdMenu();
            }
            else
            {
                startSceneCanves.HideLockedPanel();
            }
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, show the ad:
        //if (placementId == myPlacementId)
        //{
        //    Advertisement.Show(myPlacementId);
        //}
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }
}
