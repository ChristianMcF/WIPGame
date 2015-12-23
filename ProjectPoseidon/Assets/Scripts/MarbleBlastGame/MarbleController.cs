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
    //Holds the variable which controls the players max spin speed
    public float maxAngularVelocity;
    //Holds a layermask to ignore certain layers
    public LayerMask collisionLayerMask;
    //Holds the players rigidbody for easer access
    private Rigidbody _playerRgdBdy;

    private Vector3 camForward;
    private Vector3 move;

    void Start()
    {
        //Assigns the rigidbody to the associated variable
        _playerRgdBdy = GetComponent<Rigidbody>();
        //Set the players max moving speed
        _playerRgdBdy.maxAngularVelocity = maxAngularVelocity;
    }

    void Update()
    {

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

                //Set the camera's forward to be the camera's current foward transform, multiplied by 1,0,1, removing the y vector, then normalising the result
                //This keeps the vectors between 1 and 0;
                camForward = Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
                //Set the players move direction.
                move = (moveVertical * camForward + moveHorizontal * cameraTransform.right).normalized;
                //
                // ... add torque around the axis defined by the move direction.
                _playerRgdBdy.AddTorque(new Vector3(move.z, 0, -move.x) * playerSpeed);

                //Get whether the space key is recieved by unity.
                if (Input.GetKeyDown("space"))
                {
                    RaycastHit groundHit;
                    if (Physics.Raycast(transform.position, Vector3.down, out groundHit, 1f, collisionLayerMask))
                    {
                        //Addforce to the player in the Y direction, by the player's jump force.
                        _playerRgdBdy.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
                    }
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
}