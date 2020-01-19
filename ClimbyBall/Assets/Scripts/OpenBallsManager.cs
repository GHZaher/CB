using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AT = Enums.BallsName.BallNamesEnum;

public class OpenBallsManager : MonoBehaviour
{
    [SerializeField] private Ball[] balls;
    private UnlockedBallsFile unlockedBalls = new UnlockedBallsFile();

    private void Start()
    {
        if (SaveFiles.JsonFileExistsAtPersPath(SaveFilesName.UnlockedBalls))//if there is a player score file already
        {
            unlockedBalls = SaveFiles.LoadObjectFromJSONFile<UnlockedBallsFile>(SaveFilesName.UnlockedBalls);
            UnLockBall(unlockedBalls);
        }
        else
        {
            SaveFiles.SaveObjectAsJSONAtPersDataPath(unlockedBalls, SaveFilesName.UnlockedBalls);//make a new player score file
        }
    }

    public void UnLockBall(UnlockedBallsFile unlockedBalls)
    {
        for (int i = 0; i < balls.Length; i++)
        {
            if (unlockedBalls.BallsLockStates.ContainsKey(balls[i].BallName))//if this ball name is exist in the dictionary
            {
                if (unlockedBalls.BallsLockStates[balls[i].BallName] == true)//if it was unlocked
                {
                    balls[i].IsAdNeeded = false;//no need to watch an ad to use it
                }
            }
        }
    }
}
