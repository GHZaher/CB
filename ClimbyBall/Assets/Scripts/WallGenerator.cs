using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGenerator : MonoBehaviour
{
    private const string rightWallTag = "RightWall";
    private const string leftWallTag = "LeftWall"; 
    [SerializeField] private GameObject lWallPrefab;
    [SerializeField] private GameObject rWallPrefab;
    //private float wallLength;//length of the wall
    [SerializeField] private PlayerBall PlayerBall;
    [SerializeField] private GameObject[] startingWalls;
    [SerializeField] private Transform leftGeneratingPoint;//the left wall generating position
    [SerializeField] private Transform rightGeneratingPoint;//the right wall generating position
    [SerializeField] private int numberOfWallsInQueue;//max number of walls that we want to use in this game
    [SerializeField] private float generatingTime;//the time needed to generate the next wall
    [SerializeField] private float wallStartingLength;
    [SerializeField] private float wallLengthDecreaser;
    [SerializeField] private float minWallLength;
    [SerializeField] private Transform wallsParent;
    private bool isWallsPaused;
    private int wallsCounter;//counts the number of generated walls
    private bool isGenerated;//true if we generated wall withen the generating Time
    private Queue<GameObject> lWallsQueue;//Queue of left Walls GameObjects
    private Queue<GameObject> rWallsQueue;//Queue of right Walls GameObjects

    //private Wall wall;
    //private float generateDecreaser;

    public bool IsWallsPaused
    {
        get
        {
            return isWallsPaused;
        }
    }

    public float GeneratingTime
    {
        get
        {
            return generatingTime;
        }
    }

    public float WallLengthDecreaser
    {
        get
        {
            return wallLengthDecreaser;
        }
        set
        {
            wallLengthDecreaser = value;
        }
    }

    private void Start()
    {
        isWallsPaused = false;
        isGenerated = false;
        lWallsQueue = new Queue<GameObject>();
        rWallsQueue = new Queue<GameObject>();
        //wallLength = wallStartingLength;
        wallsCounter = 0;//4 because we already have 4 walls in the sceane and they are the starting game walls
        GenerateTheStartingWalls();
    }

    void FixedUpdate()
    {
        //generateDecreaser = Mathf.Clamp((generatingTime - Wall.wallSpeed*Time.deltaTime), 2, 5);
        if (PlayingSceneManager.startFlag && !isGenerated)//if we did not generate wall after the generatingTime yet
        {
            StartCoroutine(GenerateWall());//generate wall
        }
    }

    public IEnumerator GenerateWall()//walls generating function
    {

        //print(generatingTime - Wall.wallSpeed + "  GwnrateTime - WallSpeed");
        isGenerated = true;//prevent the update function from calling this function until the generatingTime ends
        if (wallsCounter < numberOfWallsInQueue)
        {
            wallsCounter++;//increase the walls counter by 1

            GameObject rightWall = Instantiate(rWallPrefab, rightGeneratingPoint.position, rightGeneratingPoint.rotation);//generate right wall
            Vector3 wallScale = rightWall.transform.localScale;
            rightWall.transform.localScale = new Vector3(wallScale.x, Mathf.Clamp(wallScale.y - wallLengthDecreaser * Time.deltaTime, minWallLength, wallStartingLength), wallScale.z);//scale the right wall by the var wallLength
            rWallsQueue.Enqueue(rightWall);//add the rightWall to the queue
            rightWall.transform.parent = wallsParent;//put the wall inside the walls parent

            GameObject leftWall = Instantiate(lWallPrefab, leftGeneratingPoint.position, leftGeneratingPoint.rotation);//generate left wall
            wallScale = leftWall.transform.localScale;
            leftWall.transform.localScale = new Vector3(wallScale.x, Mathf.Clamp(wallScale.y - wallLengthDecreaser * Time.deltaTime, minWallLength, wallStartingLength), wallScale.z);//decrease the left wall length by time;
            lWallsQueue.Enqueue(leftWall);//add the leftWall to the queue
            leftWall.transform.parent = wallsParent;//put the wall inside the walls parent

            yield return new WaitForSeconds(generatingTime);//wait for seconds before allowing generating the next wall
            isGenerated = false;//allow generating another wall
            //wallLength = wallLength + wallLengthDecreaser;//decrease the wall length by time
        }
        else
        {
            GameObject rightWall = DeQueueWall(rightWallTag);//generate right wall
            rightWall.transform.position = rightGeneratingPoint.position;
            rightWall.SetActive(true);
            Vector3 wallScale = rightWall.transform.localScale;
            rightWall.transform.localScale = new Vector3(wallScale.x, Mathf.Clamp(wallScale.y - wallLengthDecreaser * Time.deltaTime, minWallLength, wallStartingLength), wallScale.z);//scale the right wall by the var wallLength
            rWallsQueue.Enqueue(rightWall);//add the rightWall to the queue

            GameObject leftWall = DeQueueWall(leftWallTag);//generate Left wall
            leftWall.transform.position = leftGeneratingPoint.position;
            leftWall.SetActive(true);
            wallScale = leftWall.transform.localScale;
            leftWall.transform.localScale = new Vector3(wallScale.x, Mathf.Clamp(wallScale.y - wallLengthDecreaser * Time.deltaTime, minWallLength, wallStartingLength), wallScale.z);//decrease the wall length by time;
            lWallsQueue.Enqueue(leftWall);//add the LeftWall to the queue

            yield return new WaitForSeconds(generatingTime);//wait for seconds before allowing generating the next wall
            isGenerated = false;//allow generating another wall
            //wallLength = wallLength + wallLengthDecreaser;//decrease the wall length by time
        }
    }

    public void EnQueueWall(GameObject wall)//add wall to the queue
    {
        //Transform wallPosition = wall.transform;//get the wall position
        
        if (wall.tag == rightWallTag)//if it was right wall
        {
            rWallsQueue.Enqueue(wall);
            //wallPosition.position = new Vector3(wallPosition.position.x, rWallPrefab.transform.position.y, wallPosition.position.z);//reset the rwall position
        }
        else if (wall.tag == leftWallTag)//if it was left wall
        {
            lWallsQueue.Enqueue(wall);
            //wallPosition.position = new Vector3(wallPosition.position.x, lWallPrefab.transform.position.y, wallPosition.position.z);//reset the lwall position
        }
        
    }

    public GameObject DeQueueWall(string wallPosition)//get wall from the queue according to it's position (right or left)
    {
        if (wallPosition == leftWallTag && lWallsQueue.Count > 0)
        {
            GameObject wall = lWallsQueue.Dequeue();
            //wall.SetActive(true);
            return wall;
        }
        else if (wallPosition == rightWallTag && rWallsQueue.Count > 0)
        {
            GameObject wall = rWallsQueue.Dequeue();
            //wall.SetActive(true);
            return wall;
        }
        return null;
    }

    public void HideAllWalls()//clear the walls queue
    {
        if (lWallsQueue.Count > 0)
        {
            foreach (GameObject wall in lWallsQueue)
            {
                wall.SetActive(false);
            }
        }
        if (rWallsQueue.Count > 0)
        {
            foreach (GameObject wall in rWallsQueue)
            {
                wall.SetActive(false);
            }
        }
    }

    public void PauseWallsMovement()//to pause the walls movement 
    {
        if (!isWallsPaused)
        {
            isWallsPaused = true;
            for (int i = 0; i < wallsParent.childCount; i++)
            {
                wallsParent.GetChild(i).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            }
        }
    }

    public void UnPauseWallsMovement()//to Unpause the walls movement
    {
        if (isWallsPaused)
        {
            isWallsPaused = false;
            for (int i = 0; i < wallsParent.childCount; i++)
            {
                wallsParent.GetChild(i).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
        }
    }

    public void ResetWalls()//reset the position and scale and velocity of the generated walls
    {
        foreach (GameObject wall in rWallsQueue)//for all right walls
        {
            wall.SetActive(false);
            Transform wallTransform = wall.transform;
            wall.transform.localScale = new Vector3(wallTransform.localScale.x, wallStartingLength, wallTransform.localScale.z);//reset the wall scale
            wall.transform.position = new Vector3(rightGeneratingPoint.position.x, rightGeneratingPoint.position.y, rightGeneratingPoint.position.z);//reset the lwall position
            wall.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        foreach (GameObject wall in lWallsQueue)//for all left walls
        {
            Transform wallTransform = wall.transform;
            wall.SetActive(false);
            wall.transform.localScale = new Vector3(wallTransform.localScale.x, wallStartingLength, wallTransform.localScale.z);//reset the wall scale
            wall.transform.position = new Vector3(leftGeneratingPoint.position.x, leftGeneratingPoint.position.y, leftGeneratingPoint.position.z);//reset the rwall position
            wall.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    public void GenerateTheStartingWalls()
    {
        for (int i = 0; i < startingWalls.Length; i++)//regenerate the starting walls
        {
            if (wallsCounter < numberOfWallsInQueue)
            {
                if (i % 2 != 0)
                {
                    GameObject rightWall = Instantiate(rWallPrefab, startingWalls[i].transform.position, startingWalls[i].transform.rotation);
                    EnQueueWall(rightWall);
                    rightWall.SetActive(true);
                    rightWall.transform.parent = wallsParent;//put the wall inside the walls parent
                    wallsCounter++;
                }
                else
                {
                    GameObject leftWall = Instantiate(lWallPrefab, startingWalls[i].transform.position, startingWalls[i].transform.rotation);
                    EnQueueWall(leftWall);
                    leftWall.SetActive(true);
                    leftWall.transform.parent = wallsParent;//put the wall inside the walls parent
                    wallsCounter++;
                }
            }
            else
            {
                if (i % 2 != 0)
                {
                    GameObject wall = DeQueueWall(rightWallTag);
                    if (wall != null)
                    {
                        wall.transform.position = startingWalls[i].transform.position;//reset the right starting walls position
                        EnQueueWall(wall);
                        wall.SetActive(true);
                        wall.name = "OldRWall";
                    }
                }
                else
                {
                    GameObject wall = DeQueueWall(leftWallTag);
                    if (wall != null)
                    {
                        wall.transform.position = startingWalls[i].transform.position;//reset the right starting walls position
                        EnQueueWall(wall);
                        wall.SetActive(true);
                        wall.name = "OldLWall";
                    }
                }
            }

        }
    }

    public void ResetVars()//reset all variables
    {
        //wallLength = wallStartingLength;//reset the WallLengthDecreaser value
        //HideAllWalls();
      
        ResetWalls();
        GenerateTheStartingWalls();
    }
}
