using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public bool isLocalPlayer = true;
    public string userName;

    public GameObject PlayerPrefab;

    private SyncPositionRequest syncPositionRequest;
    private SyncPlayerRequest syncPlayerRequest;

    private Vector3 lastPostion = Vector3.zero;
    private float moveOffset = 0.1f;

    public Dictionary<string,GameObject> otherPlayerDict = new Dictionary<string, GameObject>();

	// Use this for initialization
	void Start ()
	{
        if (isLocalPlayer)
	    {
            Renderer renderer = GetComponent<Renderer>();
            renderer.material.color = Color.green;

            syncPositionRequest = GetComponent<SyncPositionRequest>();
	        syncPlayerRequest = FindObjectOfType<SyncPlayerRequest>();
            syncPlayerRequest.DefaultRequest();

            InvokeRepeating("SyncPosition", 3,0.1f);
	    }
	    else
	    {
            Renderer renderer = GetComponent<Renderer>();
            renderer.material.color = Color.white;
        }
	}

    void SyncPosition()
    {
        if ( Vector3.Distance(transform.position,lastPostion) >= moveOffset )
        {
            lastPostion = transform.position;
            syncPositionRequest.pos = transform.position;
            syncPositionRequest.DefaultRequest();
        }
    }

    // Update is called once per frame
	void Update () {
	    if (isLocalPlayer)
	    {
	        float h = Input.GetAxis("Horizontal");
	        float v = Input.GetAxis("Vertical");

            transform.Translate(new Vector3(-h,0,-v)*Time.deltaTime*4);

	    }
	}

    public void OnSyncPlayResponse(UserNameList userNameList)
    {
        Debug.Log("Start Instantiate Other Player");
        foreach (var username in userNameList.NameList)
        {
            GameObject otherPlayer =  GameObject.Instantiate(PlayerPrefab);
            Destroy(otherPlayer.GetComponent<SyncPlayerRequest>());
            Destroy(otherPlayer.GetComponent<SyncPositionRequest>());
            otherPlayer.GetComponent<Player>().isLocalPlayer = false;
            otherPlayer.GetComponent<Player>().userName = username;

            otherPlayerDict.Add(username,otherPlayer);
        }
    }
}
