using System.Collections;
using System.Collections.Generic;
using Common.Tools;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string userName;

    public GameObject PlayerPrefab;

    public GameObject player;

    private SyncPositionRequest syncPositionRequest;
    private SyncPlayerRequest syncPlayerRequest;

    private Vector3 lastPostion = Vector3.zero;
    private float moveOffset = 0.1f;

    public Dictionary<string,GameObject> otherPlayerDict = new Dictionary<string, GameObject>();

	// Use this for initialization
	void Start ()
	{

        Renderer renderer = player.GetComponent<Renderer>();
        renderer.material.color = Color.green;

        syncPositionRequest = GetComponent<SyncPositionRequest>();
        syncPlayerRequest = FindObjectOfType<SyncPlayerRequest>();
        syncPlayerRequest.DefaultRequest();

        InvokeRepeating("SyncPosition", 3, 0.1f);

    }

    void SyncPosition()
    {
        if ( Vector3.Distance(player.transform.position,lastPostion) >= moveOffset )
        {
            lastPostion = player.transform.position;
            syncPositionRequest.pos = player.transform.position;
            syncPositionRequest.DefaultRequest();
        }
    }

    // Update is called once per frame
	void Update () {
	    if (player != null)
	    {
	        float h = Input.GetAxis("Horizontal");
	        float v = Input.GetAxis("Vertical");

            player.transform.Translate(new Vector3(-h,0,-v)*Time.deltaTime*4);
	    }
	}

    public void OnSyncPlayResponse(UserNameList userNameList)
    {
        Debug.Log("Start Instantiate Other Player");
        foreach (var username in userNameList.NameList)
        {
            OnNewPlayerEvent(username);
        }
    }

    public void OnNewPlayerEvent(string userName)
    {
        GameObject otherPlayer = GameObject.Instantiate(PlayerPrefab);
        otherPlayerDict.Add(userName, otherPlayer);
    }

    public void OnSyncPositionEvent(PlayerList playerList)
    {
        foreach (PlayerData playerData in playerList.PlayerList_)
        {
            //Debug.Log("playerData.UsrName = " + playerData.UsrName + "Pos = " + playerData.Pos.ToString());
            GameObject go = DictTool.GetValue<string, GameObject>(otherPlayerDict, playerData.UsrName);

            if (go != null)
            {
                go.transform.position = new Vector3()
                {
                    x = playerData.Pos.X,
                    y = playerData.Pos.Y,
                    z = playerData.Pos.Z,
                };
            }
        }
    }
}
