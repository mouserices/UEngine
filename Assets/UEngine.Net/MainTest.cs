using System;
using System.Collections;
using System.Collections.Generic;
using UEngine.Net;
using UnityEngine;

public class MainTest : MonoBehaviour
{
    private NetworkSystems NetworkSystems;
    // Start is called before the first frame update
    void Start()
    {
        Telepathy.Log.Info = Debug.Log;
        Telepathy.Log.Warning = Debug.LogWarning;
        Telepathy.Log.Error = Debug.LogError;

        
        NetworkSystems = new NetworkSystems();
        NetworkSystems.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        NetworkSystems.Execute();
    }

    private void OnDestroy()
    {
        NetworkSystems.TearDown();
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(100f,100f,100f,50f),"start client"))
        {
            NetworkHelper.StartClient("localhost",1337);
        }
        
        if (GUI.Button(new Rect(100f,200,100f,50f),"start server"))
        {
            NetworkHelper.StartServer(1337);
        }
        
        if (GUI.Button(new Rect(100f,300,100f,50f),"send"))
        {
            NetworkHelper.send(1001,new C2S_Login(){UserName = "zs",Password = "23456"});
        }
    }
}
