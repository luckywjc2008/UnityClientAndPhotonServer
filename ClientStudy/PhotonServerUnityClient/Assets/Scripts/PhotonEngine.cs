using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using System;
using Common;

public class PhotonEngine : MonoBehaviour,IPhotonPeerListener
{

    public static PhotonEngine Instance;
    private static PhotonPeer peer;

    public static PhotonPeer Peer
    {
        get { return peer; }
    }

    private Dictionary<OperationCode,Request> DictRequest = new Dictionary<OperationCode, Request>();

    public static string userName;

    public void DebugReturn(DebugLevel level, string message)
    {

    }

    public void OnEvent(EventData eventData)
    {
        switch (eventData.Code)
        {
            case 1:
                Debug.Log("收到服务器发送的事件");
                Debug.Log("server send event 1");
                Dictionary<byte, object> dicData = eventData.Parameters;
                object intValue;
                object stringValue;
                dicData.TryGetValue(1, out intValue);
                dicData.TryGetValue(2, out stringValue);

                Debug.Log("server send event 1 =" + intValue.ToString() + " 2 = " + stringValue.ToString());
                break;
            default:
                break;
        }
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        OperationCode opCode = (OperationCode)operationResponse.OperationCode;
        Request request = null;
        bool temp = DictRequest.TryGetValue(opCode, out request);
        if (temp)
        {
            request.OnOperationResponse(operationResponse);
        }
        else
        {
            Debug.Log("Find Not OperationCode");
        }
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
        Debug.Log(statusCode);
    }

    public void AddRequest(Request request)
    {
        DictRequest.Add(request.OpCode,request);
    }

    public void RemoveRequest(Request request)
    {
        DictRequest.Remove(request.OpCode);
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
    }

    // Use this for initialization
    void Start()
    {
        //通过Listener接收服务器的响应
        peer = new PhotonPeer(this,ConnectionProtocol.Udp);
        peer.Connect("127.0.0.1:5055", "MyGameServer");
    }

    // Update is called once per frame
    void Update () {
        peer.Service();
	}

    void OnDestroy()
    {
        if (peer != null && peer.PeerState == PeerStateValue.Connected)
        {
            peer.Disconnect();
        }
    }
}
