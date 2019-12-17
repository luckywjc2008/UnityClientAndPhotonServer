using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Common.Tools;
using ExitGames.Client.Photon;

class NewPlayerEvent : BaseEvent
{

    private Player player;

    public override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
    }

    public override void OnEvent(EventData eventData)
    {
        NewPlayer newPlayer = DictTool.GetProtoByDtoData<NewPlayer>(eventData.Parameters,ParameterCode.UserName);
        player.OnNewPlayerEvent(newPlayer.UserName);
    }
}

