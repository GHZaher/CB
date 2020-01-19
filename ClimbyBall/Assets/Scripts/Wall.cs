using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private float wallStartingSpeed;
    public static float wallSpeed = 2;//wall movement speed
    [SerializeField] private WallGenerator wallGenerator;
    private PlayerBall playerBall;
    private Rigidbody rig;
    [SerializeField] private GameObject wallParticle;

    private void Start()
    {
        playerBall = FindObjectOfType<PlayerBall>();
        rig = GetComponent<Rigidbody>();
        wallSpeed = wallStartingSpeed;//reset the wallStartingSpeed value
    }

    private void FixedUpdate()
    {
        
        //wallSpeed = Mathf.Clamp((wallSpeed + speedIncreaser * Time.deltaTime) , wallStartingSpeed, wallGenerator.GeneratingTime - 1);
        //print(wallSpeed + " Wall speed");
        if (PlayingSceneManager.startFlag)
        {
            //wallSpeed = wallSpeed + wallSpeedIncreaser * Time.deltaTime;
            rig.velocity = new Vector3(0, -wallSpeed, 0);//move the wall down
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == 10)//if it was the player
        {
            wallParticle.SetActive(true);//active wall particles
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.layer == 10)//if it was the player
        {
            wallParticle.SetActive(false);//disactive wall particles
        }
    }

    public void ResetVars()//reset all variables
    {
        wallSpeed = wallStartingSpeed;//reset wall speed value
    }
}
