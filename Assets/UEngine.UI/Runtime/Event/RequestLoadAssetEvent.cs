using System;

public struct RequestLoadAssetEvent
{
    public string AssetName;
    public Type AssetType;
    public Action<UnityEngine.Object> LoadCallBack;
}