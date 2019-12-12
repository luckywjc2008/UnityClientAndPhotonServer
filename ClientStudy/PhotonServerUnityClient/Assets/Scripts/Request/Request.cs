using Common;
using ExitGames.Client.Photon;
using UnityEngine;
public abstract class Request : MonoBehaviour
{
    public OperationCode OpCode;
    public abstract void OnOperationResponse(OperationResponse operationResponse);
    public abstract void DefaultRequest();

    public virtual void Start()
    {
        PhotonEngine.Instance.AddRequest(this);
    }

    public void OnDestroy()
    {
        PhotonEngine.Instance.RemoveRequest(this);
    }
}
