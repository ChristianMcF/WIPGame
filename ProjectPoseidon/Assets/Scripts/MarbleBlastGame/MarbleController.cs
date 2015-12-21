using UnityEngine;
using System.Collections;

public class MarbleController : MonoBehaviour
{
    //Declares a public enum, which will be used to determine whether the player is able to move or not
    public enum MoveState { CanMove, CannotMove }
    //Holds the currently selected enum which controls whether the player is able to move or not
    public MoveState selectedMoveState;
    //Gets the Transform component of the camera.
    public Transform cameraTransform;
    //Holds the variable which controls the players speed. Editable in inspector
    public float playerSpeed;
    //Holds the variable which controls the force from the ground the player experiences. Used for jumping
    public float jumpForce;
    //Holds the players rigidbody for easer access
    private Rigidbody _playerRgdBdy;

    void Start()
    {
        //Assigns the rigidbody to the associated variable
        _playerRgdBdy = GetComponent<Rigidbody>();
    }

    //Fixed update method, is used when dealing with rigidbodies
    void FixedUpdate()
    {
        //Begin a switch method which will allow for disabling of player movement when it shouldnt be able to.
        switch (selectedMoveState)
        {
            #region CanMoveRegion
            //If the player is currently able to move
            case MoveState.CanMove:
                //Declare and initialise two variables to be the Horizontal and vertical axis recieved by unity
                float moveHorizontal = Input.GetAxis("Horizontal");
                float moveVertical = Input.GetAxis("Vertical");

                //Add force to the player upon the proper axis.
                //Set the axis to be itself times the playerspeed variable. Then multiply this by the camera's forward and right direction
                //respectively, after it has been passed through the MinMaxVector Method.
                _playerRgdBdy.AddForce((moveVertical * playerSpeed) * MinMaxVector(cameraTransform.forward));
                _playerRgdBdy.AddForce((moveHorizontal * playerSpeed) * MinMaxVector(cameraTransform.right));

                //Get whether the space key is recieved by unity.
                if (Input.GetKeyUp("space"))
                {
                    //Addforce to the player in the Y direction, by the player's jump force.
                    _playerRgdBdy.AddForce(new Vector3(0, jumpForce, 0));
                }
                //break out of the switch so the next case does not run
                break;
            #endregion
            #region CannotMoveRegion
            //If the player is unable to move
            case MoveState.CannotMove:
                //Player cannot do anything
                break;
                #endregion
        }
    }

    #region HelpfulMethods
    //This method is called because without this method, the player would move faster while looking forward, and move slower while
    //looking down on the player from above.
    Vector3 MinMaxVector(Vector3 badVector)
    {
        //Create a new variable to hold the new vector, and assign it to be the return of the FixFloat Method, with the removal of the y axis.
        //Once again this aids in making the movement consistent, as without this, the player would move faster going foward than backwards
        Vector3 goodVector = new Vector3(FixFloat(badVector.x), 0, FixFloat(badVector.z));
        //Return the fixed vector to the main method
        return goodVector;
    }

    //This method sets the floats to an extreme of -1 or 1
    float FixFloat(float badFloat)
    {
        //Create a variable to hold the finalized badfloat
        float goodFloat;
        //If the float is more than zero, set it to its extreme of 1
        if (badFloat > 0)
        {
            goodFloat = 1;
        }
        //else if the float is less than zero set it to its extreme of -1
        else if (badFloat < 0)
        {
            goodFloat = -1;
        }
        else
        {
            //Otherwise if the float is 0, set goodfloat to be 0
            goodFloat = 0;
        }
        //Return the float to the MinMaxVector Method
        return goodFloat;
    }
    #endregion
}