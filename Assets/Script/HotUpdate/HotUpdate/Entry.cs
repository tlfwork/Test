using FairyGUI;
using MyBag;
using System;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using YooAsset;

public class Entry
{
    // Start is called before the first frame update
    public static async void Start()
    {
        Debug.Log("热更成功");

        await RegisterFont();

        ResourcePackage resourcePackage = YooAssets.GetPackage("FirstPackage");

        UIPackage uIPackage = UIPackage.AddPackage("MyBag", LoadPackageXML);

        MyBagBinder.BindAll();

        UI_Component1 uI_Component1 = UIPackage.CreateObject("MyBag", "Component1").asCom as UI_Component1;

        UI_Component2 uI_Component2 = UIPackage.CreateObject("MyBag", "Component2").asCom as UI_Component2;

        //uI_Component1.fairyBatching = true;

        GRoot.inst.AddChild(uI_Component2);

        GRoot.inst.AddChild(uI_Component1);

       

        //uI_Component1.m_btn_1.onClick.Add(context => {
            
        //    Debug.Log("aaaa");
        
        //});
    }

    public static object LoadPackageXML(string name, string extension, Type type, out DestroyMethod destroyMethod)
    {
        destroyMethod = DestroyMethod.None;

        ResourcePackage package = YooAssets.GetPackage("FirstPackage");

        AssetHandle assetHandle = package.LoadAssetSync(name);

        return assetHandle.AssetObject;
    }

    public static async Task RegisterFont()
    {
        ResourcePackage resourcePackage = YooAssets.GetPackage("FirstPackage");

        AssetHandle assetHandle = resourcePackage.LoadAssetAsync("fangsong");

        await assetHandle.Task;

        DynamicFont dynamicFont = new DynamicFont();

        dynamicFont.name = "fangsong";

        dynamicFont.nativeFont = assetHandle.AssetObject as Font;

        FontManager.RegisterFont(dynamicFont);

        UIConfig.defaultFont = "fangsong";
    }
}
