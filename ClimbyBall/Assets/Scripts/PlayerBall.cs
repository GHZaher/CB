using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerBall : MonoBehaviour
{
    [SerializeField] private SpriteRenderer playerBallSprite;
    [SerializeField] private TrailRenderer playerBallTrail;
    [SerializeField] private Ball[] ballsSprites;//array which contains all balls sprites
    [SerializeField] private float startingCursorSpeed;//Cursor rotating speed
    private float cursorSpeed;//Cursor rotating speed
    [SerializeField] private float cursorSpeedIncreaser;//Cursor rotating speed
    [SerializeField] private float maxAngleOnRWall = 180; //Cursor max angel on the right wall
    [SerializeField] private float minAngleOnRWall = 0; //Cursor min angel on the right wall
    [SerializeField] private float maxAngleOnLWall = 0; //Cursor max angel on the left wall
    [SerializeField] private float minAngleOnLWall = -180; //Cursor min angel on the left wall
    [SerializeField] private float maxAngleOnStart = 90; //Cursor max angel on the Start position wall
    [SerializeField] private float minAngleOnStart = -90; //Cursor min angel on the Start position wall
    [SerializeField] private float ballSpeed;//speed of ball throwing
    [SerializeField] private float lWallRotationFixingAngel = 350;//to make sure thet the cursor starts from up (better for the player)
    [SerializeField] private float rWallRotationFixingAngel = 20;//to make sure thet the cursor starts from up (better for the player)
    [SerializeField] private float trailHideTime;//time to wait before hiding the ball trail
    [SerializeField] private GameObject cursor;
    //[SerializeField] private GameObject ballTrail;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private WallGenerator wallGenerator;
    private AudioManager audioManager;
    [SerializeField] private PlayingSceneManager playingSceneManager;


    private bool isTheRightWall;
    private bool isTheLeftWall;
    private bool isForward;
    private bool isThrew;
    private bool isItMovingRight;//true if the Cursor is moving right
    private Rigidbody rig;
    private Vector3 playerBallStartingPosition;
    private Vector3 initialBallScale;

    public float CursorSpeedIncreaser
    {
        get
        {
            return cursorSpeedIncreaser;
        }

        set
        {
            cursorSpeedIncreaser = value;
        }
    }

    private void Start()
    {
        initialBallScale = transform.lossyScale;
        //if (PlayerPrefs.HasKey("BallName"))//if the player choosed a ball
        //{
        //    string ballName = SaveFiles.GetStringPlayerPref("BallName");//get the choosen bal name
            
        //    for (int i = 0; i < ballsSprites.Length; i++)//find the choosen ball sprite
        //    {
        //        if (ballsSprites[i].BallName.ToString() == ballName)
        //        {
        //            //playerBallSprite.sprite = ballsSprites[i].BallSprite;//set the player sprite to the choosen sprite
        //            //playerBallTrail.material = ballsSprites[i].BallMaterial;//change the trail material
        //            break;
        //        }
        //    }
        //}

        isTheRightWall = false;
        isTheLeftWall = false;
        isForward = true;
        isItMovingRight = true;
        isThrew = false;
        rig = GetComponent<Rigidbody>();
        playerBallStartingPosition = transform.position;
        cursorSpeed = startingCursorSpeed;
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isThrew && !PlayingSceneManager.gamePaused/*&& PlayingSceneManager.startFlag*/)//if we toched the screen and not threw it before and game has been started
        {
            if (!playingSceneManager.IsTouchingOnUI())//if we are not cliking on an ui element 
            {
                //PlayingSceneManager.startFlag = true;
                wallGenerator.UnPauseWallsMovement();//to unPause generated walls movement if we pause it because of showing an ad
                ThrowBall();//throw the ball
            }
        }
    }

    private void FixedUpdate()
    {
        RotateCursor();//keep rotating the cursor(ball becuase the cursor is the son of the ball)
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "LeftWall" && isThrew && !isTheLeftWall)//if the ball hitted the left wall for one time
        {
            PlayWallHitSound();
            isThrew = false;
            isTheRightWall = false;//it is not on the right wall
            isTheLeftWall = true;//put left wall = true to know what angels we have to check in the rotation code
            isForward = false;//it's not on the starting place

            rig.velocity = Vector3.zero;
            rig.velocity = collision.GetComponent<Rigidbody>().velocity;//make the velocity of the ball == the velocity of the hitted wall
            transform.rotation = Quaternion.identity;//reset the ball rotation
            transform.Rotate(0, 0, lWallRotationFixingAngel);//rotate the ball by lWallRotationFixingAngel angels to put the cursor in the right oriantation
            cursor.SetActive(true);//active the cursor again because we reached the wall
            StartCoroutine(HideBallTrail());//disactive ball trail on walls

            scoreManager.PlayerScore++;//add the score by one
            scoreManager.ScoreTxt.text = scoreManager.PlayerScore.ToString();//show the score on the screen

        }
        else if (collision.gameObject.tag == "RightWall" && isThrew && !isTheRightWall)//if the ball hitted the right wall for one time
        {
            PlayWallHitSound();
            isThrew = false;
            isTheRightWall = true;//put right wall = true to know what angels we have to check in the rotation code
            isTheLeftWall = false;//it is not on the left wall
            isForward = false;//it's not on the starting place

            rig.velocity = Vector3.zero;
            rig.velocity = collision.GetComponent<Rigidbody>().velocity;//make the velocity of the ball == the velocity of the hitted wall
            transform.rotation = Quaternion.identity;//reset the ball rotation
            transform.Rotate(0, 0, rWallRotationFixingAngel);//rotate the ball by rWallRotationFixingAngel angels to put the cursor in the right oriantation
            cursor.SetActive(true);//active the cursor again because we reached the wall
            StartCoroutine(HideBallTrail());//disactive ball trail on walls

            scoreManager.PlayerScore++;//add the score by one
            scoreManager.ScoreTxt.text = scoreManager.PlayerScore.ToString();//show the score on the screen
        }
    }

    public void RotateCursor()
    {
        #region Rotation
        //print(transform.rotation.z * 180);
        cursorSpeed = cursorSpeed + cursorSpeedIncreaser*Time.deltaTime;//increase the cursor rotation speed by the time

        if (!isThrew)
        {
            if (isItMovingRight)//if the Cursor is rotating right
            {
                transform.Rotate(new Vector3(0, 0, -cursorSpeed));//rotate right
            }
            else//if the Cursor is rotating left
            {
                transform.Rotate(new Vector3(0, 0, cursorSpeed));//rotate left
            }



            if (isTheRightWall)//if the ball is on the left wall or on the right wall
            {
                if (transform.localRotation.z * 180 >= maxAngleOnRWall - 2 || transform.localRotation.z * 180 <= minAngleOnRWall + 2)//if Asin(angel) =>89 || <= 0 the rotate the opposit side
                {
                    isItMovingRight = !isItMovingRight;
                }
            }
            else if (isTheLeftWall)
            {
                if (transform.localRotation.z * 180 >= maxAngleOnLWall || transform.localRotation.z * 180 <= minAngleOnLWall)//if Asin(angel) =>89 || <= 0 the rotate the opposit side
                {
                    isItMovingRight = !isItMovingRight;
                }
            }
            else if (isForward)//if the ball is not on the wall
            {
                if (transform.localRotation.z * 180 >= maxAngleOnStart - 2 || transform.localRotation.z * 180 <= minAngleOnStart + 2)//if Asin(angel) =>45 || <= -45 the rotate the opposit side
                {
                    isItMovingRight = !isItMovingRight;
                }
            }
        }
        #endregion

        #region eurlaRot

        //if (isItMovingRight)//if the Cursor is rotating right
        //{
        //    transform.Rotate(new Vector3(0, 0, -speed));//rotate right
        //}
        //else//if the Cursor is rotating left
        //{
        //    transform.Rotate(new Vector3(0, 0, speed));//rotate left
        //}

        //rotVec = transform.eulerAngles;//get euler angels

        //if (rotVec.z > 180)//if the euler angels > 180 then it's negative angel
        //{
        //    currentAngle.z = rotVec.z - 360;//calc the negative angel cause the euler angels does not have a negative value 0<angel<399 so we need to get the negative by this equasion
        //}
        //else
        //{
        //    currentAngle.z = rotVec.z;//if we still in the positive angel then take it itself
        //}

        //if (isTheRightWall || isTheLeftWall)//if the ball is on the left wall or on the right wall
        //{
        //    if (Mathf.Floor(currentAngle.z) <= maxAngleOnWall && Mathf.Floor(currentAngle.z) >= maxAngleOnWall - 2 || Mathf.Floor(currentAngle.z) >= minAngleOnWall && Mathf.Floor(currentAngle.z) <= minAngleOnWall + 2)//if the cursor between 0 && 2 || between 180 && 178 then rotate to the opposite side(we make it a range to make sure that we will not miss the right angel)
        //    {
        //        isItMovingRight = !isItMovingRight;
        //    }
        //}

        //else if (isForward)//if the ball is not on the wall
        //{
        //    if (Mathf.Floor(currentAngle.z) <= minAngleOnStart + 1 && Mathf.Floor(currentAngle.z) >= minAngleOnStart - 1 || Mathf.Floor(currentAngle.z) >= maxAngleOnStart - 1 && Mathf.Floor(currentAngle.z) <= maxAngleOnStart + 1)//if the cursor between -89 && -91 || between 89 && 91 then rotate to the opposite side(we make it a range to make sure that we will not miss the right angel)
        //    {
        //        isItMovingRight = !isItMovingRight;
        //    }
        //}
        #endregion
    }

    public void ThrowBall()
    {
        PlayBallClickSound();
        isThrew = true;
        transform.parent = null;
        transform.localScale = initialBallScale;
        rig.AddRelativeForce(new Vector3(0, ballSpeed, 0));//throw the ball
        cursor.SetActive(false);//disable the cursor unitill we reach the wall
        playerBallTrail.gameObject.SetActive(true);//active ball shade unitill we reach the wall
    }

    public IEnumerator HideBallTrail()//hide ball trail
    {
        yield return new WaitForSeconds(trailHideTime);//wait for some time
        if (!isThrew)//if the ball still on the wall after that time
        {
            playerBallTrail.gameObject.SetActive(false);//disactive the trail
        }
    }

    public void RevivePlayer()//Revive the player after finishing the ad
    {
        rig.velocity = Vector3.zero;
        transform.eulerAngles = Vector3.zero;//make the player rotation (0 0 0 0)
        transform.position = playerBallStartingPosition;//bring back the ball to it's starting position
        gameObject.SetActive(true);//reactive the playerBall
        cursor.SetActive(true);
        isTheRightWall = false;
        isTheLeftWall = false;
        isForward = true;
        isItMovingRight = true;
        isThrew = false;
        playerBallTrail.gameObject.SetActive(false);//hide the ball shade
        playingSceneManager.UnPauseTheGame();//continue the game
        //PlayingSceneManager.startFlag = true;
        PlayingSceneManager.playerDied = false;
        //wallGenerator.UnPauseWallsMovement();
        PlayingSceneManager.gamePaused = false;
        playingSceneManager.ResetVars();
    }

    public void ResetVars()//reset all variables
    {
        rig.velocity = Vector3.zero;
        transform.rotation = Quaternion.identity;//make the player rotation (0 0 0 0)
        transform.position = playerBallStartingPosition;//bring back the ball to it's starting position
        //gameObject.SetActive(true);//reactive the playerBall
        cursor.SetActive(true);
        cursorSpeedIncreaser = 0;
        isTheRightWall = false;
        isTheLeftWall = false;
        isForward = true;
        isItMovingRight = true;
        isThrew = false;
        cursorSpeed = startingCursorSpeed;
        playerBallTrail.gameObject.SetActive(false);//hide the ball shade
        playingSceneManager.UnPauseTheGame();//unpause the scean
    }

    public void PlayWallHitSound()//play wall hit sound
    {
        audioManager = FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
        audioManager.PlayWallHitAudio();
    }

    public void PlayBallClickSound()//play wall hit sound
    {
        audioManager = FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
        audioManager.PlayBallClickAudio();
    }

    
}
