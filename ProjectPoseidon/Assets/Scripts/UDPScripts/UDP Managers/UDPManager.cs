using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using System.Threading;

public class UDPManager : MonoBehaviour
{
    #region Variable Declarations

    #region General Variables
    //Holds info which is useful to both server and client
    //Holds the ports which the server and client connect to
    public int serverSocketPort;
    public int clientSocketPort;
    //Enumerator which will allow for selection between server and client.
    public enum peerType
    {
        Server,
        Client
    }
    ;
    //Holds the info for the currently selected type
    public peerType peertype = new peerType();

    //Will Determine if the server and client can both send and recieve data
    public bool sendRecieveData;
    //CURRENTLY NOT IMPLEMENTED!!
    //Holds whether the client should automatically connect to the broadcasting server
    public bool clientConnectAutomatically;
    //Have it so the controller layout defualts to landscape left
    public bool makeControllerLandscapeLeft;
    //Have it log to console for each command sent
    public bool logVerbose;
    //Set the name of the server application running
    public string applicationName;
    #endregion

    #region Control Holders
    //These variables will hold each instance of the control reciever scripts in the current scene.
    //Holds all the gameobjects which recieve Gyroscope data
    private UDPGyroReciever[] _udpGyroReciever;
    #endregion

    #region Server Variables
    //Recieving Variables
    //The thread which will handle the server
    Thread receiveThread;
    //Variable which holds info for server client
    UdpClient recieveClient;
    //Will hold the data which will be recieved from the client
    string strReceiveUDP = "";
    //Holds the Local IP of the server computer
    string LocalIP = String.Empty;
    //Gets the name of the computer hosting the server
    string hostname;
    //Bool which checks whether the data should be interpreted on recieving from client.
    private bool interpretData = false;
    #endregion

    #region Client Variables
    //Sending Variables
    //The IP of the Server to connect to.
    public string serverIP = "myIP"; // default local
                                     //Varaible which contains the host and local or remote port information needed by an application to connect to a service on a host
    IPEndPoint remoteEndPoint;
    //Variable which holds data which provides simple methods for sending and receiving connectionless UDP datagrams
    UdpClient sendClient;
    #endregion

    #endregion

    #region General Methods
    //Start method
    public void Start()
    {
        //If the PeerType is that of a server
        if (peertype == peerType.Server)
        {
            //Have the application continue running when in the background
            Application.runInBackground = true;
            //Call the method to start the server
            ServerInit();
        }
        else
        {
            //Call the method to start the client
            ClientInit();
        }
    }

    //On disable of this manager
    void OnDisable()
    {
        //IF the peertype is that of a server
        if (peertype == peerType.Server)
        {
            //If the thread is not null
            if (receiveThread != null)
                //Abort the currently running server thread
                receiveThread.Abort();
            //Then close the running server client
            recieveClient.Close();
        }
        else
        {
            //If the client is not null
            if (sendClient != null)
                //Close te running server client
                sendClient.Close();
        }
    }

    //General Update Method
    void Update()
    {
        //If the server is running as server
        if (peertype == peerType.Server)
        {
            //If data needs to be interpreted
            if (interpretData)
            {
                //Set the need to interpret false
                interpretData = false;
                //Call the interpret method and pass in the recieved string.
                InterpretData(strReceiveUDP);
            }
            //Otherwise if client
        }
        else
        {
        }
    }

    void Awake()
    {
        //If the manager is running as a server
        if (peertype == peerType.Server)
        {
            //Find all gameobjects in the scene that require gyro info
            _udpGyroReciever = FindObjectsOfType(typeof(UDPGyroReciever)) as UDPGyroReciever[];
        }
        else
        {
        }
    }

    #endregion

    #region Server Methods
    //Server initialisation
    private void ServerInit()
    {
        //Create an instance of a new thread
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        //Sets the thread to run in the background of the application
        receiveThread.IsBackground = true;
        //Start the thread
        receiveThread.Start();
        //Get the hostname of the local computer
        hostname = Dns.GetHostName();
        //Initialise an array of the ips for the current host
        IPAddress[] ips = Dns.GetHostAddresses(hostname);
        //If there are more than one host adressess.
        if (ips.Length > 0)
        {
            //Set the local ip string to the ip
            LocalIP = ips[0].ToString();
            //Debug the IP to stop warning message
            Debug.Log(LocalIP);
        }
    }

    //RecieveData script
    private void ReceiveData()
    {
        //Create an instance of the UDPclient using the port for server communication
        recieveClient = new UdpClient(serverSocketPort);
        //Have a while loop that always runs
        while (true)
        {
            //Attempt to
            try
            {
                //Create the endpoint using the ip adress and the port for server communication
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Broadcast, serverSocketPort);
                //IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                //Recieve all data from client, as a byte array.
                byte[] data = recieveClient.Receive(ref anyIP);
                //Get a string from the data byte
                string text = Encoding.UTF8.GetString(data);
                strReceiveUDP = text;
                //Make it so the data should be interpreted
                interpretData = true;
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }

    //Interpret data function
    private void InterpretData(string _recievedString)
    {
        //Get the string, and remove the delimiters and put each word into one part of a string array.
        string[] _controlInfo = _recievedString.Split(new string[] { "/!/" }, StringSplitOptions.RemoveEmptyEntries);
        //HOW THE STRINGS ARE FORMATTED
        //String[0] = ControlType
        //String[1] = Control Name
        //String[2] = Method To Call
        //String[3] = Data To Read

        //Get what type of control the information is coming from
        switch (_controlInfo[0])
        {
            //If it is being recieved from a gyro script
            case "Gyro":
                //For each Reciever in the scene,
                foreach (var _gyroRecievers in _udpGyroReciever)
                {
                    //Makes it so that disabled gameobjects are not contacted
                    if (_gyroRecievers.enabled)
                    {
                        //See if the control is recieving of this data
                        if (_gyroRecievers.controlName == _controlInfo[1])
                        {
                            //Call the Decipher method in the Reciever. Passing in firstly the method to call, then the associated Gyro Data
                            _gyroRecievers.DecipherData(_controlInfo[2], _controlInfo[3]);
                        }
                    }
                }
                break;
        }
    }

    //Get the UDP Packet
    public string UDPGetPacket()
    {
        return strReceiveUDP;
    }
    #endregion

    #region Client Methods
    //Initialise the client
    public void ClientInit()
    {
        //Set the socket port to be something
        clientSocketPort = 26000; // quake port ;)
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(serverIP), clientSocketPort);
        //remoteEndPoint = new IPEndPoint(IPAddress.Broadcast, port); // toute machine
        sendClient = new UdpClient();
    }

    //Get the IP port
    public string GetIPAndPort()
    {
        //Get the info about the IP and port
        String ipPortString;
        ipPortString = ("IP : " + serverIP + " Port : " + clientSocketPort);
        return ipPortString;
    }

    //Method which is called from other scripts, such as Multicontroller scripts
    public void sendString(string message)
    {
        //Attempt to send the data across the server
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            sendClient.Send(data, data.Length, remoteEndPoint);
        }
        catch (Exception err)
        {
            print(err.ToString());
        }
    }
    #endregion
}
