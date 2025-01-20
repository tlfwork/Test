using FairyGUI;
using MyBag;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using YooAsset;
using HybridCLR;
using System.Threading.Tasks;
using System.IO;

public class StartRun : MonoBehaviour
{
    public enum DLLMode
    {
        local,

        hotupdate,
    }

    public DLLMode dllMode;

    Dictionary<string, object> context = new Dictionary<string, object>();

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        YooAssets.Initialize();

        ResourcePackage package = YooAssets.CreatePackage("FirstPackage");

        YooAssets.SetDefaultPackage(package);

        StartCoroutine(InitPackage());
    }

    private IEnumerator InitPackage()
    {
        IRemoteServices remoteServices = new RemoteServices();

        ResourcePackage package = YooAssets.GetPackage("FirstPackage");

        FileSystemParameters cacheFileSystemParameters = FileSystemParameters.CreateDefaultCacheFileSystemParameters(remoteServices);

        //FileSystemParameters buildinFileSystem = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();

        HostPlayModeParameters initParameters = new HostPlayModeParameters();

        //initParameters.BuildinFileSystemParameters = buildinFileSystem;

        initParameters.CacheFileSystemParameters = cacheFileSystemParameters;

        InitializationOperation initOperation = package.InitializeAsync(initParameters);

        yield return initOperation;

        if (initOperation.Status == EOperationStatus.Succeed)

            Debug.Log("资源包初始化成功！");
        else
            Debug.LogError($"资源包初始化失败：{initOperation.Error}");

        StartCoroutine(RequestPackageVersion());
    }

    private IEnumerator RequestPackageVersion()
    {
        string packageVersion = "";

        ResourcePackage package = YooAssets.GetPackage("FirstPackage");

        RequestPackageVersionOperation operation = package.RequestPackageVersionAsync(false);

        yield return operation;

        if (operation.Status == EOperationStatus.Succeed)
        {
            //更新成功
            packageVersion = operation.PackageVersion;

            Debug.Log($"Request package Version : {packageVersion}");

            context.Add("PackageVersion", packageVersion);
        }
        else
        {
            //更新失败
            Debug.LogError(operation.Error);
        }

        StartCoroutine(UpdatePackageManifest());
    }

    private IEnumerator UpdatePackageManifest()
    {
        string packageVersion = (string)context["PackageVersion"];

        ResourcePackage package = YooAssets.GetPackage("FirstPackage");

        UpdatePackageManifestOperation operation = package.UpdatePackageManifestAsync(packageVersion);

        yield return operation;

        if (operation.Status == EOperationStatus.Succeed)
        {
            //更新成功
        }
        else
        {
            //更新失败
            Debug.LogError(operation.Error);
        }

        StartCoroutine(ClearPackageUnusedCacheFiles());
    }

    private IEnumerator ClearPackageUnusedCacheFiles()
    {
        ResourcePackage package = YooAssets.GetPackage("FirstPackage");

        ClearUnusedBundleFilesOperation operation = package.ClearUnusedBundleFilesAsync();

        yield return operation;

        HotUpdate();
    }

    public class RemoteServices : IRemoteServices
    {
        public string GetRemoteFallbackURL(string fileName)
        {
            return $"http://mypanda/Res/v1/{fileName}";
        }

        public string GetRemoteMainURL(string fileName)
        {
            return $"http://mypanda/Res/v1/{fileName}";
        }
    }

    public async void HotUpdate()
    {
        System.Collections.Specialized.IOrderedDictionary orderedDictionary = null;

        Assembly hotUpdateAss = null;

        if (dllMode == DLLMode.hotupdate)
        {
            ResourcePackage package = YooAssets.GetPackage("FirstPackage");

            AssetHandle assetHandle = package.LoadAssetAsync("HotLoad.dll");

            await assetHandle.Task;

            TextAsset textAsset = assetHandle.AssetObject as TextAsset;

            hotUpdateAss = Assembly.Load(textAsset.bytes);
        }
        else
        {
            hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "HotLoad");
        }

        Type type = hotUpdateAss.GetType("HotLoadManager");

        MethodInfo method = type.GetMethod("Start");

        method.Invoke(null,new object[] { dllMode.GetHashCode()});
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InitOK();
        }
    }

    void InitOK()
    {
        MyBagBinder.BindAll();

        UIPackage uIPackage = UIPackage.AddPackage("MyBag", LoadPackageXML);

        UI_Component2 uI_Component1 = UIPackage.CreateObject("MyBag", "Component2").asCom as UI_Component2;

        GRoot.inst.AddChild(uI_Component1);

        GLoader gLoader = new GLoader();

        gLoader.SetSize(100, 100);

        uI_Component1.AddChild(gLoader);

        gLoader.url = "ui://MyBag/SimpleMovie";

        gLoader.fill = FillType.Scale;
    }

    public object LoadPackageXML(string name, string extension, Type type, out DestroyMethod destroyMethod)
    {
        destroyMethod = DestroyMethod.None;

        ResourcePackage package = YooAssets.GetPackage("FirstPackage");

        AssetHandle assetHandle = package.LoadAssetSync(name);

        return assetHandle.AssetObject;
    }


}

