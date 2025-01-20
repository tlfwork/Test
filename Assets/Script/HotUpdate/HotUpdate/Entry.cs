using LitJson;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

public class Entry
{
    // Start is called before the first frame update
    public static void Start()
    {
        Debug.Log("热更成功");

        ResourcePackage resourcePackage = YooAssets.GetPackage("FirstPackage");

        
    }

    //public object FGUIAssetLoader()
    //{

    //}
}
