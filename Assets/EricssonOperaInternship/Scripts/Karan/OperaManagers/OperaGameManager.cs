using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Discover.Colocation.Test;

[Serializable]
public enum OperaTypesRPC
{
    PlayAudio,
    StopAudio,
    SpawnCube,
    DestroyCube
}


public class OperaGameManager : MonoBehaviour
{
    //this line makes it Singleton
    public static OperaGameManager instance;


    public string networkSessionName = "OperaXR";
    public OperaNetworkManager mainNetworkManager;
    public AudioRPCManager audioRpcController;

    private bool isHostInFusionServer = false;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }


    public void StartNetworkOpera(bool isHost = false)
    {
        // Connect as host or client
        mainNetworkManager.StartConnection(isHost);
    }

    public void SetHostInFusionServer(bool isHost)
    {
        isHostInFusionServer = isHost;

        if (isHostInFusionServer)
        {
            audioRpcController.RequestStateAuthority();
        }
    }

    public void SendRPC(OperaTypesRPC rpcType)
    {
        switch (rpcType)
        {
            case OperaTypesRPC.PlayAudio:
                if (isHostInFusionServer)
                {
                    audioRpcController.RPC_PlayAudioClip();
                    DebugLogMessage($"Host triggered RPC {rpcType}");
                }
                break;
            case OperaTypesRPC.StopAudio:
                if(isHostInFusionServer)
                {
                    audioRpcController.RPC_StopAudioClip();
                    DebugLogMessage($"Host triggered RPC {rpcType}");
                }
                break;
            case OperaTypesRPC.SpawnCube:
                break;
            case OperaTypesRPC.DestroyCube:
                break;
            default:
                break;
        }

        
    }

    public void DebugLogMessage(string text)
    {
        OperaEventsManager.Send_OnLogMessage(text);
        Debug.Log(text);
    }


}
