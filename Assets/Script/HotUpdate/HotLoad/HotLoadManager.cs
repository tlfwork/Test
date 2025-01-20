using HybridCLR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using YooAsset;

public class HotLoadManager 
{
    public enum DLLMode
    {
        local,

        hotupdate,
    }

    public static async void Start(DLLMode dLLMode)
    {
        Assembly hotUpdateAss = null;

        if (dLLMode == DLLMode.hotupdate)
        {
            await LoadMetdata(new List<string>() 
            
            { 
                "mscorlib.dll", "UnityEngine.dll", "System.Core.dll","System.dll"
            });

            await LoadThirdPartyDLL(new List<string>() { "LitJSON.dll" });


            ResourcePackage package = YooAssets.GetPackage("FirstPackage");

            AssetHandle assetHandle = package.LoadAssetAsync("HotUpdate.dll");

            await assetHandle.Task;

            TextAsset textAsset = assetHandle.AssetObject as TextAsset;

            hotUpdateAss = Assembly.Load(textAsset.bytes);
        }
        else
        {
            hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "HotUpdate");
        }

        Type type = hotUpdateAss.GetType("Entry");

        MethodInfo method = type.GetMethod("Start");

        method.Invoke(null, null);
    }

    public static async Task LoadMetdata(List<string> strings)
    {
        foreach (string s in strings)
        {
            ResourcePackage package = YooAssets.GetPackage("FirstPackage");

            AssetHandle assetHandle = package.LoadAssetAsync(s);

            await assetHandle.Task;

            TextAsset textAsset = assetHandle.AssetObject as TextAsset;

            HybridCLR.RuntimeApi.LoadMetadataForAOTAssembly(textAsset.bytes, HomologousImageMode.SuperSet);
        }
    }

    public static async Task LoadThirdPartyDLL(List<string> strings)
    {
        foreach (string s in strings)
        {
            ResourcePackage package = YooAssets.GetPackage("FirstPackage");

            AssetHandle assetHandle = package.LoadAssetAsync(s);

            await assetHandle.Task;

            TextAsset textAsset = assetHandle.AssetObject as TextAsset;

            Assembly.Load(textAsset.bytes);
        }
    }
}
