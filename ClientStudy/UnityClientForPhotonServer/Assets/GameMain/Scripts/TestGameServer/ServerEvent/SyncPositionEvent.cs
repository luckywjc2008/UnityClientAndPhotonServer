using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Common.Tools;
using ExitGames.Client.Photon;
using UnityEngine;

public class SyncPositionEvent : BaseEvent
{
    private Player player;

    public override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
    }

    public override void OnEvent(EventData eventData)
    {
        PlayerList playerList = DictTool.GetProtoByDtoData<PlayerList>(eventData.Parameters,ParameterCode.PlayerDataList);

        player.OnSyncPositionEvent(playerList);
    }
}
