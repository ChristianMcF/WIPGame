using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UDPMultiControllerClient : MonoBehaviour
{
	//Declaration of Enum which holds what types of controls this script supports
	public enum ControllerType
	{
		DPad,
		Joystick,
		Button,
		Gyro
	}
	//Holds the currently selected controller in the editor
	public ControllerType selectedControllerType;

	//General Variables which will be sent to the server to aid in communication
	//Holds the UDPManaged component so that messages can be sent to server.
	public UDPManager udpManager;
	//Holds the name of the control which will be accessed
	public string controlName;
	//Holds the name of the custom method to call on the object
	public string methodToCall;

	#region Button Type Variables
	//Variables which are used in the button controller enum option
	//Image to be displayed on button prior to click
	public Sprite DefaultButtonImage;
	//Image to be displayed on the button as it is being clicked.
	public Sprite ClickedButtonImage;
    #endregion

    #region Gyro Type Variables
    private Quaternion _lastLocation;
	#endregion


	void Start ()
	{
		//Check for which control is being used.
		switch (selectedControllerType) {
		case ControllerType.Gyro: //If the controller is to send Gyroscope info
			//Enable the Gyro.
			Input.gyro.enabled = true;
			break;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		switch (selectedControllerType) {
		case ControllerType.Gyro: //If the controller is to send Gyroscope info
                //If the current gyroattitute is not equal to last location, send new location. Saves number of packets sent.
                if (Input.gyro.attitude != _lastLocation)
                {
                    //Call the send mesage function, with all required components needed from gyroscope. and send it directly to the UDPManager
                    udpManager.sendString(FormatMessage(selectedControllerType.ToString(), controlName, methodToCall, (Input.gyro.attitude * GetRotFix()).ToString()));
                    _lastLocation = Input.gyro.attitude;
                }
			break;
		}
	}
	//Formats the message for it to be sent to the server
	string FormatMessage (string _ControlType, string _ControlName, string _MethodCall, params string[] _ControlData)
	{
		//Depending on how many parameters have been passed into the method, send to server the data and all variables
		switch (_ControlData.Length) {
		case 0:
			return (_ControlType + "/!/" + _ControlName + "/!/" + _MethodCall);
		case 1:
			return (_ControlType + "/!/" + _ControlName + "/!/" + _MethodCall + "/!/" + _ControlData [0]);
		case 2:
			return (_ControlType + "/!/" + _ControlName + "/!/" + _MethodCall + "/!/" + _ControlData [0] + "/!/" + _ControlData [1]);
		case 3:
			return (_ControlType + "/!/" + _ControlName + "/!/" + _MethodCall + "/!/" + _ControlData [0] + "/!/" + _ControlData [1] + "/!/" + _ControlData [2]);
		default:
			return (_ControlType + "/!/" + _ControlName + "/!/" + _MethodCall);
		}
	}

    //Gets a rotation fix for the Gyro
    private Quaternion GetRotFix()
    {
        if (Screen.orientation == ScreenOrientation.Portrait)
            return Quaternion.identity;
        if (Screen.orientation == ScreenOrientation.LandscapeLeft
        || Screen.orientation == ScreenOrientation.Landscape)
            return Quaternion.Euler(0, 0, 90);
        if (Screen.orientation == ScreenOrientation.LandscapeRight)
            return Quaternion.Euler(0, 0, -90);
        if (Screen.orientation == ScreenOrientation.PortraitUpsideDown)
            return Quaternion.Euler(0, 0, -180);
        return Quaternion.identity;
    }
}
