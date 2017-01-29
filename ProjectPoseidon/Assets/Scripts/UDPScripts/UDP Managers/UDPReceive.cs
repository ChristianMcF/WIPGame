using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading;
//using System.Net.NetworkInformation;

public class UDPReceive : MonoBehaviour
{
	Thread receiveThread; 
	UdpClient client; 
	public int port = 26000;  
	string strReceiveUDP = "";
	public string LocalIP = String.Empty;
	string hostname;

	public TestButtonReciever buttonReciever;
	public bool interpretData = false;

	public void Start ()
	{
		Application.runInBackground = true;
		init ();   
	}

	// init
	private void init ()
	{
		receiveThread = new Thread (new ThreadStart (ReceiveData));
		receiveThread.IsBackground = true;
		receiveThread.Start ();
		hostname = Dns.GetHostName ();
		IPAddress[] ips = Dns.GetHostAddresses (hostname);
		if (ips.Length > 0) {
			LocalIP = ips [0].ToString ();
		}
	}
	
	void OnGUI ()
	{
		Rect rectObj = new Rect (10, 10, 400, 200);
		GUIStyle style = new GUIStyle ();
		style.alignment = TextAnchor.UpperLeft;
		GUI.Box (rectObj, hostname + " MY IP : " + LocalIP + " : " + strReceiveUDP, style);
	}
	
	private  void ReceiveData ()
	{
		client = new UdpClient (port);
		while (true) {
			try {
				IPEndPoint anyIP = new IPEndPoint (IPAddress.Broadcast, port);
				//IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
				byte[] data = client.Receive (ref anyIP);
				string text = Encoding.UTF8.GetString (data);
				strReceiveUDP = text;
				interpretData = true;
				//Debug.Log(strReceiveUDP);
			} catch (Exception err) {
				print (err.ToString ());
			}
		}
	}

	private void InterpretData (string _recievedString)
	{
		string[] _controlName = _recievedString.Split (new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
		if (buttonReciever.controlName == _controlName [0]) {
			buttonReciever.Invoke (_controlName [1], 0);
		}
	}
	
	public string UDPGetPacket ()
	{
		return strReceiveUDP;
	}
	
	void OnDisable ()
	{
		if (receiveThread != null)
			receiveThread.Abort ();
		client.Close ();
	}

	void Update ()
	{
		if (interpretData) {
			interpretData = false;
			InterpretData (strReceiveUDP);
		}
	}

	void Awake ()
	{
		buttonReciever = (TestButtonReciever)FindObjectOfType (typeof(TestButtonReciever));
	}
}