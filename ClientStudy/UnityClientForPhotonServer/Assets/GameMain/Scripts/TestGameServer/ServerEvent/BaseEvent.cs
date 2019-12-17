using System.Collections;
using System.Collections.Generic;
using Common;
using ExitGames.Client.Photon;
using UnityEngine;

public abstract class BaseEvent : MonoBehaviour {

    public EventCode eventCode;
    public abstract void OnEvent(EventData eventData);

    public virtual void Start()
    {
        PhotonEngine.Instance.AddEvent(this);
    }

    public void OnDestroy()
    {
        PhotonEngine.Instance.RemoveEvent(this);
    }
}
