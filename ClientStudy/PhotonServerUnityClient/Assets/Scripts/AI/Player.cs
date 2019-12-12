using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public bool isLocalPlayer = true;

    private SyncPositionRequest syncPositionRequest;

    private Vector3 lastPostion = Vector3.zero;
    private float moveOffset = 0.1f;

	// Use this for initialization
	void Start ()
	{
        if (isLocalPlayer)
	    {
            Renderer renderer = GetComponent<Renderer>();
	        renderer.material.color = Color.green;
            syncPositionRequest = GetComponent<SyncPositionRequest>();
            InvokeRepeating("SyncPosition", 3,0.1f);
	    }
	    else
	    {
	        
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
}
