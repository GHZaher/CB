using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour
{
    private string myPlacementId = "rewardedVideo";

    public void ShowRewardedVideo(bool isBallAd = false)//true if it was for Ball opening and it comes from the ad pressed button in the scene
    {
        if (Advertisement.IsReady())
        {
            GetComponent<RewardedAds>().IsBallAd = isBallAd;
            Advertisement.Show(myPlacementId);
        }
        else
        {
            print("Ad is not Ready yet !");
        }
    }
}
