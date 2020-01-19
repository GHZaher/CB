using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AT = Enums.BallsName.BallNamesEnum;

[System.Serializable]
public class UnlockedBallsFile//these strings will containes "true" if it unlocked by the player
{
    public Dictionary<AT, bool> BallsLockStates = new Dictionary<AT, bool>
    {
        { AT.Default, false }, { AT.Gear, false }, { AT.Only, false }, { AT.Pattern, false },{ AT.Tire, false },
        { AT.Vortex, false }
    };
    //public string[] BallsLockStates;

    //public string DefaultBall = "locked";
    //public string GearBall = "locked";
    //public string OnlyBall = "locked";
    //public string PatternBall = "locked";
    //public string tireBall = "locked";
    //public string VortexBall = "locked";
}
