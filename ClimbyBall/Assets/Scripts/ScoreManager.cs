using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    PlayerScoreManagerFile playerScoreManager = new PlayerScoreManagerFile();
    [SerializeField] private TextMeshProUGUI scoreTxt;
    private long playerScore = 0;
    private float topPlayerScore;
    private static bool instance = false;

    public TextMeshProUGUI ScoreTxt 
    {
        get
        {
            return scoreTxt;
        }
        set
        {
            scoreTxt.text = value.ToString();
        }
    }

    public long PlayerScore
    {
        get
        {
            return playerScore;
        }
        set
        {
            playerScore = value;
        }
    }

    private void Start()
    {
        if (!instance)
        {
            instance = true;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        playerScore = 0;
        //print(Application.persistentDataPath);
        if (SaveFiles.JsonFileExistsAtPersPath("PlayerScore"))//if there is a player score file already
        {
            playerScoreManager = SaveFiles.LoadObjectFromJSONFile<PlayerScoreManagerFile>("PlayerScore");
            topPlayerScore = playerScoreManager.TopPlayerScore;//load the top score from it
        }
        else
        {
            playerScoreManager.TopPlayerScore = 0;
            topPlayerScore = 0;
            SaveFiles.SaveObjectAsJSONAtPersDataPath(playerScoreManager, "PlayerScore");//make a new player score file
        }

        scoreTxt.text = playerScoreManager.TopPlayerScore.ToString();//show it on the screen
    }

    public void SavePlayerScore()
    {
        if (playerScore > playerScoreManager.TopPlayerScore)//if the new score is bigger the the top score
        {
            playerScoreManager.TopPlayerScore = playerScore;
            topPlayerScore = playerScoreManager.TopPlayerScore;//make it the top score
        }
        
        SaveFiles.SaveObjectAsJSONAtPersDataPath(playerScoreManager, "PlayerScore");//save new score
    }

    private void OnApplicationQuit()
    {
        SavePlayerScore();
    }

    public void ResetVars()
    {
        playerScore = 0;
        scoreTxt.text = topPlayerScore.ToString(); ;
    }

    public static float GetTopScore()
    {
        PlayerScoreManagerFile playerScoreManager = new PlayerScoreManagerFile();
        if (SaveFiles.JsonFileExistsAtPersPath("PlayerScore"))//if there is a player score file already
        {
            playerScoreManager = SaveFiles.LoadObjectFromJSONFile<PlayerScoreManagerFile>("PlayerScore");
            return playerScoreManager.TopPlayerScore;//load the top score from it

        }
        else
        {
            playerScoreManager.TopPlayerScore = 0;
            SaveFiles.SaveObjectAsJSONAtPersDataPath(playerScoreManager, "PlayerScore");//make a new player score file
            return 0;
        }
    }
}
