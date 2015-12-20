using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPSend : MonoBehaviour
{
	public string IP = "myIP"; // default local
	public int port = 26000;  
	IPEndPoint remoteEndPoint;
	UdpClient client;
	//string strMessage = "";
	
	public void Start ()
	{
		init ();   
	}
	
//	void OnGUI ()
//	{
//		Rect rectObj = new Rect (40, 40, 400, 800);
//		GUIStyle style = new GUIStyle ();
//		style.alignment = TextAnchor.UpperLeft;
//		GUI.Box (rectObj, "UDPSendData\n IP : " + IP + " Port : " + port, style);
//		//
//		strMessage = GUI.TextField (new Rect (40, 80, 280, 40), strMessage);
//		if (GUI.Button (new Rect (320, 80, 80, 40), "Send")) {
//			sendString (strMessage + "\n");
//		}      
//	}
	
	// init
	public void init ()
	{
		//IP = "myIP";
		port = 26000; // quake port ;)
		remoteEndPoint = new IPEndPoint (IPAddress.Parse (IP), port);
		//remoteEndPoint = new IPEndPoint(IPAddress.Broadcast, port); // toute machine
		client = new UdpClient ();
	}

	public string GetIPAndPort ()
	{
		String ipPortString;
		ipPortString = ("IP : " + IP + " Port : " + port);
		return ipPortString;
	}
	
	// sendData
	public void sendString (string message)
	{
		try {
			byte[] data = Encoding.UTF8.GetBytes (message);
			client.Send (data, data.Length, remoteEndPoint);
		} catch (Exception err) {
			print (err.ToString ());
		}
	}
	
	void OnDisable ()
	{
		if (client != null)
			client.Close ();
	}
}