using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TestCanvasScript : MonoBehaviour
{

	public Text ipPortText;
	public Text counterText;
	public Image uiBackground;
	public UDPSend udpSendScript;

	private bool _startCount;

	private int _counter;

	public void Update ()
	{
		ipPortText.text = udpSendScript.GetIPAndPort ();
		if (_startCount) {
			counterText.text = _counter.ToString ();
			udpSendScript.sendString (_counter.ToString ());
			_counter++;
		}
	}

	public void StartCounter ()
	{
		_startCount = !_startCount;
		if (_startCount)
			uiBackground.color = Color.green;
		else
			uiBackground.color = Color.red;
	}
}
