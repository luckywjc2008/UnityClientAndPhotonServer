using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetMouseButtonDown(0))
	    {
	        SendRequest();
	    }
	}

    void SendRequest()
    {
        Dictionary<byte,object> dicData = new Dictionary<byte, object>();
        dicData.Add(1,100);
        dicData.Add(2,"还不错");

        PhotonEngine.Peer.OpCustom(1,dicData , true);
    }
}
