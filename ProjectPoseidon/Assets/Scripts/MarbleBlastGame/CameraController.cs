
using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    //Declares a public enum, which will be used to determine whether the player is able to move or not
    public enum MoveState { ViewMode1, ViewMode2, UnInteractablePan1, UnInteractablePan2 }
    //Holds the currently selected enum which controls whether the player is able to move or not
    public MoveState selectedMoveState;
    //Variable to hold the transform component of the player
    public Transform targTransform;
    //Creates a variable which displays a LayerMask, which allows for physics detections to be ignored when interacting with certain objects
    public LayerMask ignoreCollisionLayers;
    //Public floats to help with the control of the camera. Very useful
    public float startDistance = 10.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;
    public float zoomSensitivity = 5;

    public float yMinLimit = 20f;
    public float yMaxLimit = 80f;

    public float distanceMin = 1f;
    public float distanceMax = 15f;
    public float startCamHeight;

    float x = 0.0f;
    float y = 0.0f;

    private float lastUserDistance;
    private bool playerTooClose;

    private bool initialiseCam = true;
    private float _cameraDistance;

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
            case MoveState.ViewMode1:
                MainPlayerCameraMethod(targTransform, ignoreCollisionLayers, startDistance, xSpeed, ySpeed, zoomSensitivity, yMinLimit, yMaxLimit, distanceMin, distanceMax, true);
                break;
            case MoveState.ViewMode2:
                //Player controls the camera normally, but the camera does not snap to look at player when view is obstructed by an obstacle
                MainPlayerCameraMethod(targTransform, ignoreCollisionLayers, startDistance, xSpeed, ySpeed, zoomSensitivity, yMinLimit, yMaxLimit, distanceMin, distanceMax, false);
                break;
            case MoveState.UnInteractablePan1:
                //Camera will automatically pan around player in this view. But does not recieve any player interaction
                MainAICameraMethod(targTransform, ignoreCollisionLayers, startDistance, distanceMin, xSpeed, startCamHeight, true);
                break;
            case MoveState.UnInteractablePan2:
                //Camera will automatically pan around player in this view, and does not move to look at player when obstructed
                MainAICameraMethod(targTransform, ignoreCollisionLayers, startDistance, distanceMin, xSpeed, startCamHeight, false);
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

    void MainAICameraMethod(Transform _targetTransform, LayerMask _layerMask, float _cameraStartDistance, float _cameraDistanceMin, float _rotateSpeed, float _camHeight, bool _alwaysViewTarg)
    {
        if (initialiseCam)
        {
            _cameraDistance = _cameraStartDistance;
            lastUserDistance = _cameraDistance;
            initialiseCam = false;
        }
        //If the player object is initialized and not null
        if (_targetTransform)
        {
            //Get the x axis and multiply it by various values. then times by 0.02f to smooth movement
            x += 1 * ((_rotateSpeed / 2) / (_cameraDistance / _cameraDistance)) * 0.02f;
            y = _camHeight;


            //Call the Clamp angle method and assign it back to the y variable
            y = ClampAngle(y, -90, 90);

            //Declare and initialise a quaternion, to be equal to a reorganied Vector3, converted into a Quaternion using .Euler()
            Quaternion rotation = Quaternion.Euler(y, x, 0);

            if (_cameraDistance > _cameraDistanceMin)
            {
                playerTooClose = false;
            }

            if (!playerTooClose)
            {
                lastUserDistance = Mathf.Clamp(lastUserDistance, _cameraDistanceMin, _cameraStartDistance);
                //Set the distance to be the resultant float, after the mousewheelAxis is subtracted from the distance. and then
                //multiplied by a zoom sensitivity. After is has been clamped between two designated values
                _cameraDistance = Mathf.Clamp(_cameraDistance, _cameraDistanceMin, lastUserDistance);
            }

            if (_alwaysViewTarg)
            {
                //Calls the method which handles the player going behind walls, and having the camera snap to the player.
                SnapCameraToPlayer(_targetTransform, _layerMask);
            }
            //Create a new vector3 which hold the negative distance on the z axis
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -_cameraDistance);
            //Create a new vector3 which will be used to set the cameras position
            Vector3 position = rotation * negDistance + _targetTransform.position;

            //Set the rotation and the position to their associated parts
            transform.rotation = rotation;
            transform.position = position;
        }
    }
    void MainPlayerCameraMethod(Transform _targetTransform, LayerMask _layerMask, float _cameraStartDistance, float _xSpeed, float _ySpeed, float _zoomSensitivity, float _yMinLimit, float _yMaxLimit, float _cameraDistanceMin, float _cameraDistanceMax, bool _alwaysViewTarg)
    {
        if (initialiseCam)
        {
            _cameraDistance = _cameraStartDistance;
            lastUserDistance = _cameraDistance;
            initialiseCam = false;
        }
        //If the player object is initialized and not null
        if (_targetTransform)
        {
            //Get the x axis and multiply it by various values. then times by 0.02f to smooth movement
            x += Input.GetAxis("Mouse X") * (_xSpeed / (_cameraDistance / _cameraDistance)) * 0.02f;
            y -= Input.GetAxis("Mouse Y") * _ySpeed * 0.02f;

            //Call the Clamp angle method and assign it back to the y variable
            y = ClampAngle(y, _yMinLimit, _yMaxLimit);

            //Declare and initialise a quaternion, to be equal to a reorganied Vector3, converted into a Quaternion using .Euler()
            Quaternion rotation = Quaternion.Euler(y, x, 0);

            if (_cameraDistance > _cameraDistanceMin)
            {
                playerTooClose = false;
            }

            if (!playerTooClose)
            {
                lastUserDistance = Mathf.Clamp(lastUserDistance - Input.GetAxis("Mouse ScrollWheel") * _zoomSensitivity, _cameraDistanceMin, _cameraDistanceMax);
                //Set the distance to be the resultant float, after the mousewheelAxis is subtracted from the distance. and then
                //multiplied by a zoom sensitivity. After is has been clamped between two designated values
                _cameraDistance = Mathf.Clamp(_cameraDistance - Input.GetAxis("Mouse ScrollWheel") * _zoomSensitivity, _cameraDistanceMin, lastUserDistance);
            }

            if (_alwaysViewTarg)
            {
                //Calls the method which handles the player going behind walls, and having the camera snap to the player.
                SnapCameraToPlayer(_targetTransform, _layerMask);
            }
            //Create a new vector3 which hold the negative distance on the z axis
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -_cameraDistance);
            //Create a new vector3 which will be used to set the cameras position
            Vector3 position = rotation * negDistance + _targetTransform.position;

            //Set the rotation and the position to their associated parts
            transform.rotation = rotation;
            transform.position = position;
        }
    }

    void SnapCameraToPlayer(Transform _targetTransform, LayerMask _layerMask)
    {
        //Create a variable which holds all the information outed from any raycast
        RaycastHit hit;
        //Create a linecast, using the player position and the transform position and out all info to the hit variable
        if (Physics.Linecast(transform.position, _targetTransform.position, out hit, _layerMask))
        {
            //decrement the distance by the distance from the player to the object hit
            _cameraDistance -= hit.distance + 0.1f;
            //PlayerTooClose
            playerTooClose = true;
        }
        else
        {
            if (!(Physics.Raycast((transform.position + (transform.forward * 1f)), -transform.forward, out hit, 1f)))
            {
                if (_cameraDistance < lastUserDistance)
                {
                    _cameraDistance += 10f * Time.deltaTime;
                }
            }
        }
    }
}