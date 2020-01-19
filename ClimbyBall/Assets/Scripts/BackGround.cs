using UnityEngine;
using UnityEngine.SceneManagement;

public class BackGround : MonoBehaviour
{
    [SerializeField] private float gearSpeed;
    [SerializeField] private float bgGearSpeed;
    [SerializeField] private Transform rGear;
    [SerializeField] private Transform lGear;
    [SerializeField] private Transform rBGGear;
    [SerializeField] private Transform lBGGear;
    [SerializeField] private Transform pauseBtnGear;
    [SerializeField] private Transform RestartBtnGear;

    void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name == "PlayingScene")//if we are in the playing scene
        {
            if (PlayingSceneManager.startFlag)//play the background when we start the game
            {
                rGear.Rotate(0, 0, -gearSpeed);
                lGear.Rotate(0, 0, gearSpeed);
                rBGGear.Rotate(0, 0, -bgGearSpeed);
                lBGGear.Rotate(0, 0, bgGearSpeed);
            }
            pauseBtnGear.Rotate(0, 0, bgGearSpeed);
            RestartBtnGear.Rotate(0, 0, bgGearSpeed);
        }
        else//if we are in the start scene no need to check the startFlag
        {
            rGear.Rotate(0, 0, -gearSpeed);
            lGear.Rotate(0, 0, gearSpeed);
            rBGGear.Rotate(0, 0, -bgGearSpeed);
            lBGGear.Rotate(0, 0, bgGearSpeed);
        }
    }

    public void ResetVars()
    {
        pauseBtnGear.transform.rotation = Quaternion.identity;
        //RestartBtnGear.transform.rotation = Quaternion.identity;
    }
}
