using FairyGUI;
using MyBag;
using System;
using System.Threading.Tasks;
using UnityEngine;
using YooAsset;

public class Entry
{
    // Start is called before the first frame update
    public static async void Start()
    {
        Debug.Log("热更成功");

        ResourcePackage resourcePackage = YooAssets.GetPackage("FirstPackage");

        UIPackage uIPackage = UIPackage.AddPackage("MyBag", LoadPackageXML);

        MyBagBinder.BindAll();

        UI_Component1 uI_Component1 = UIPackage.CreateObject("MyBag", "Component1").asCom as UI_Component1;

        uI_Component1.fairyBatching = true;

        GRoot.inst.AddChild(uI_Component1);

        //for (int i = 0; i < 10; i++)
        //{
        //    GRichTextField gRichTextField = new GRichTextField();

        //    gRichTextField.SetSize(300, 300);

        //    gRichTextField.SetXY(500, 500);

        //    gRichTextField.text = "abcd<img src='ui://MyBag/panda' width='200' height='200'/>abcd";

        //    uI_Component1.AddChild(gRichTextField);
        //}
    }

    public static object LoadPackageXML(string name, string extension, Type type, out DestroyMethod destroyMethod)
    {
        destroyMethod = DestroyMethod.None;

        ResourcePackage package = YooAssets.GetPackage("FirstPackage");

        AssetHandle assetHandle = package.LoadAssetSync(name);

        return assetHandle.AssetObject;
    }
}
