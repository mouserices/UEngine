using System;
using System.Collections.Generic;
using UEngine.Net;
using UEngine.NP.Features.FsmState;
using UEngine.NP.FsmState;
using UnityEngine;
using UnityEngine.InputSystem;


public class GameStart : MonoBehaviour
{
    private ClientSystemsConfiguration _clientSystemsConfiguration;
    private GUIStyle labelStyle;
    public void Start()
    {
        _clientSystemsConfiguration = new ClientSystemsConfiguration();
        _clientSystemsConfiguration.InitializeSystems(Contexts.sharedInstance);
        _clientSystemsConfiguration.Initialize();


        MetaContext.Get<INetworkService>().StartClient("localhost", 1337);
        
        //定义一个GUIStyle的对象
        labelStyle = new GUIStyle();

//设置文本颜色
        labelStyle.normal.textColor = new Color(1, 0, 0);

//设置字体大小
        labelStyle.fontSize = 100;
    }

    public void Update()
    {
        _clientSystemsConfiguration.Execute();
        _clientSystemsConfiguration.Cleanup();
    }

    public void OnGUI()
    {
        // if (Contexts.sharedInstance.game.hasMainPlayerData)
        // {
        //     var roomID = Contexts.sharedInstance.game.mainPlayerData.RoomID;
        //     if (roomID > 0)
        //     {
        //         var roomEntity = Contexts.sharedInstance.room.GetEntityWithRoom(roomID);
        //         GUI.Label(new Rect(100f,0,500f,250f),$"ping: {Contexts.sharedInstance.game.ping.halfRTT}",labelStyle);
        //         GUI.Label(new Rect(100f,100f,500f,250f),$"tick: {roomEntity.tick.TickCount}",labelStyle);
        //         GUI.Label(new Rect(100f,400f,500f,250f),$"ServerTick: {roomEntity.tick.ServerTickCount}",labelStyle);
        //     }
        // }
    }

    public void OnDestroy()
    {
        _clientSystemsConfiguration.TearDown();
    }
}