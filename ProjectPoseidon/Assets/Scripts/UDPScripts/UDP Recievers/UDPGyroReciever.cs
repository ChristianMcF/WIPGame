using UnityEngine;
using System.Collections;

public class UDPGyroReciever : MonoBehaviour
{
    //CURRENTLY NOT FULLY YET IMPLEMENTED

    //Public Enum to change the Axis that the rotation for the accelerometer occurs on.
    public enum RotationAxis
    {
        XYZ,
        XZY,
        YXZ,
        YZX,
        ZXY,
        ZYX,
        CameraLoc
    }
    public RotationAxis rotationAxis;
    //END OF NOT YET FULLY IMPLEMENTED STUFF

    //The name of the control which will control if the data sent will send data to this object
    public string controlName;
    //Allows for control of the Lerp speed for the rotation between Quaternion steps
    public float lerpSpeed;
    //A boolean which controls whether the Y rotation needs to be reset.
    public bool resetYRotation;
    //A Vector3 which allows for user to set the offset to be calculated when resetting the Y position
    public Vector3 rotationOffset;
    //Quaternion variable which will hold the Gyrodata recieved from the script
    private Quaternion _gyroData;
    //Quaternion which holds the offset used for calculation of offsets
    private Quaternion defaultYRotation;

    //Called everyframe
    void Update()
    {
        //Check if the Y axis needs to be reset
        if (resetYRotation)
        {
            //Call the ResetYMethod to initialise the variables
            ResetYOrientation();
            //Set the bool to false, so it does not reset again
            resetYRotation = false;
        }
    }

    //Method which initialises variables so the Y axis resets in position
    void ResetYOrientation()
    {
        //Set the current rotation of the device to be the current rotation of the device
        defaultYRotation = _gyroData;
        //Set the rotation of the gameobject to be the rotation offset defined in the inspector
        transform.rotation = Quaternion.Euler(rotationOffset);
    }

    //Method which aligns the Axis of the Quaternion to the correct axis defined in the inspector
    Quaternion GyroDataToAxis(Quaternion baseGyroData)
    {
        //Declare new Quaternion to be returned
        Quaternion _fixedGyroData;
        /*HOW GYRO IS CURRENTLY SETUP
		 * By Default according to default camera position: 
		 * X rotation is mapped to Y
		 * Y rotation is mapped to Z
		 * Z rotation is mapped to X
		 */
        //Switch statement using the info from the inspector
        switch (rotationAxis)
        {
            case RotationAxis.XYZ:
                _fixedGyroData = new Quaternion(-baseGyroData.x, baseGyroData.z, -baseGyroData.x, -baseGyroData.w); // Realign the axis
                break;
            case RotationAxis.XZY:
                _fixedGyroData = new Quaternion(baseGyroData.y, baseGyroData.x, baseGyroData.z, -baseGyroData.w); // Realign the axis
                break;
            case RotationAxis.YXZ:
                _fixedGyroData = new Quaternion(-baseGyroData.z, -baseGyroData.y, baseGyroData.x, -baseGyroData.w); // Realign the axis
                break;
            case RotationAxis.YZX:
                _fixedGyroData = new Quaternion(baseGyroData.y, baseGyroData.z, -baseGyroData.x, -baseGyroData.w); // Realign the axis
                break;
            case RotationAxis.ZXY:
                _fixedGyroData = new Quaternion(baseGyroData.x, baseGyroData.z, baseGyroData.y, -baseGyroData.w); // Realign the axis
                break;
            case RotationAxis.ZYX:
                _fixedGyroData = new Quaternion(baseGyroData.x, baseGyroData.z, baseGyroData.y, -baseGyroData.w); // Realign the axis
                break;
            //case RotationAxis.CameraLoc:
            //  Debug.LogError("Not Yet Implemented, Will default to normal co-ordinate system");
            //  break;
            default:
                _fixedGyroData = baseGyroData; //Have the data be unchanged
                break;
        }
        //Return the gyro information to the variable calling the method
        return _fixedGyroData;
    }

    //Method which is called from the UDP manager Script, Recieves both the method the program needs to call, as well as the data needed
    public void DecipherData(string callMethod, string _data)
    {
        //Make a string array with the independent values, removing all unneeded characters which are created when unity sends a quaternion as a string
        string[] _dataString = _data.Split(new string[] { "(", ", ", ")" }, System.StringSplitOptions.RemoveEmptyEntries);
        //Set the Gyroscope data to be used by the script, to be the data aligned to the correct axis
        _gyroData = GyroDataToAxis(new Quaternion(float.Parse(_dataString[0]), float.Parse(_dataString[1]), (float.Parse(_dataString[2])), (float.Parse(_dataString[3]))));
        //Call the method sent by the script
        Invoke(callMethod, 0);
    }

    //Sets the Gyro to control the rotation of the gameobject the script is attached to
    void GyroAsRotation()
    {
        //Create a new Quaternion to hold the data, so the default data in unchanged, which solves a number of errors
        Quaternion _afterGyroData;
        //Set the variable to be the inverse of the default Y rotation * the Gyrodata. which resets the Y rotation to the correct number
        _afterGyroData = Quaternion.Inverse(defaultYRotation) * _gyroData;
        //Due to unstable nature of this line for some reason, it is disabled by using an unchanged rotation offset.
        if (rotationOffset != new Vector3(0, 0, 0))
        {
            //This line of code is supposed to fix errors with internal rotation, but may cause rotational error over time.
            _afterGyroData *= Quaternion.Euler(rotationOffset);
        }
        //Set the rotation of the gameobject to be a spherical lerp using various values to make it look smoother.
        transform.rotation = Quaternion.Slerp(transform.rotation, _afterGyroData, Time.deltaTime * lerpSpeed);
    }

    //Sets the Gyro to control the movement of the gameobject the script is attached to
    void GyroAsMovement()
    {

    }
}
