using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    //Declares a public enum, which will be used to determine whether the player is able to move or not
    public enum MoveState { CanMove, CannotMove }
    //Holds the currently selected enum which controls whether the player is able to move or not
    public MoveState selectedMoveState;
    //Variable to hold the transform component of the player
    public Transform player;
    //Public floats to help with the control of the camera. Very useful
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;
    public float zoomSensitivity;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    float x = 0.0f;
    float y = 0.0f;

    public float lastUserDistance;
    bool resetCam;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    void LateUpdate()
    {
        //Begin a switch method which will allow for disabling of player movement when it shouldnt be able to.
        switch (selectedMoveState)
        {
            //If the player is currently able to move
            case MoveState.CanMove:
                //If the player object is initialized and not null
                if (player)
                {
                    //Get the x axis and multiply it by various values. then times by 0.02f to smooth movement
                    x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
                    y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                    //Call the Clamp angle method and assign it back to the y variable
                    y = ClampAngle(y, yMinLimit, yMaxLimit);

                    //Declare and initialise a quaternion, to be equal to a reorganied Vector3, converted into a Quaternion using .Euler()
                    Quaternion rotation = Quaternion.Euler(y, x, 0);

                    //Set the distance to be the resultant float, after the mousewheelAxis is subtracted from the distance. and then
                    //multiplied by a zoom sensitivity. After is has been clamped between two designated values
                    distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity, distanceMin, distanceMax);
                    //
                    //THIS RESETTING OF CAMERA NEEDS TO BE FIXED SO IT WORKS BETTER
                    //
                    //Check whether the camera needs to be reset after zooming in.
                    if (!resetCam)
                    {
                        //if the camera does not need to be reset, set the last distance set by the user to be the current distance
                        lastUserDistance = distance;
                    }
                    //Create a variable which holds all the information outed from any raycast
                    RaycastHit hit;
                    //Create a linecast, using the player position and the transform position and out all info to the hit variable
                    if (Physics.Linecast(player.position, transform.position, out hit))
                    {
                        //decrement the distance by the distance from the player to the object hit
                        distance -= hit.distance;
                        //set the boolean to false, stopping the lastuserdistance from updating while the camera is zooming in
                        resetCam = true;
                    }
                    else
                    {
                        //Set the distance to the last known user distance
                        distance = lastUserDistance;
                        //have it so the user distance can be updated again
                        resetCam = false;
                    }
                    //Create a new vector3 which hold the negative distance on the z axis
                    Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
                    //Create a new vector3 which will be used to set the cameras position
                    Vector3 position = rotation * negDistance + player.position;

                    //Set the rotation and the position to their associated parts
                    transform.rotation = rotation;
                    transform.position = position;
                }
                break;
            //If the player is unable to move
            case MoveState.CannotMove:
                //Player cannot do anything
                break;
        }
    }

    //A public method used to clamp the angle of a float between two values
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}