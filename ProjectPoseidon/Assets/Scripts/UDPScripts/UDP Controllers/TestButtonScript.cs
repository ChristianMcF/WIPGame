using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TestButtonScript : MonoBehaviour
{
	public UDPManager udpManagerScript;
	public string controlName;
	public string methodToCall;


	public void ButtonPressed ()
	{
		if (methodToCall != "" && controlName != "") {
			udpManagerScript.sendString (controlName + " " + methodToCall);
		}
	}
}
