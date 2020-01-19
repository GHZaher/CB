using UnityEngine;

public class Cursor : MonoBehaviour
{
    
    private Vector3 currentAngle;//eural angel after taking the calculating the negative angle
    private Vector3 rotVec;//eural angel
    private bool isItMovingRight;//true if the Cursor is moving right
    //private Rigidbody rig;
    [SerializeField] private float cursorSpeed;//Cursor rotating speed
    [SerializeField] private float maxAngleOnRWall = 180; //Cursor max angel on the right wall
    [SerializeField] private float minAngleOnRWall = 0; //Cursor min angel on the right wall
    [SerializeField] private float maxAngleOnLWall = 0; //Cursor max angel on the left wall
    [SerializeField] private float minAngleOnLWall = -180; //Cursor min angel on the left wall
    [SerializeField] private float maxAngleOnStart = 90; //Cursor max angel on the Start position wall
    [SerializeField] private float minAngleOnStart = -90; //Cursor min angel on the Start position wall

    public bool isTheRightWall;
    public bool isTheLeftWall;
    public bool isForward;
    

    private void Start()
    {
        isTheRightWall = false;
        isTheLeftWall = false;
        isForward = true;
        //rig = GetComponent<Rigidbody>();
        isItMovingRight = true;
    }

    void Update()
    {
        #region sinRot
        //print(transform.rotation.z * 180);


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
            if (transform.localRotation.z * 180 >= maxAngleOnRWall-2 || transform.localRotation.z * 180 <= minAngleOnRWall+2)//if Asin(angel) =>89 || <= 0 the rotate the opposit side
            {
                isItMovingRight = !isItMovingRight;
            }
        }
        else if (isTheLeftWall)
        {
            if (transform.localRotation.z * 180 >= maxAngleOnLWall - 2 || transform.localRotation.z * 180 <= minAngleOnLWall + 2)//if Asin(angel) =>89 || <= 0 the rotate the opposit side
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

        #endregion

        //#region eurlaRot

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
        //#endregion
    }
}
