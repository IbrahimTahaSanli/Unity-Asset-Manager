using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

//Load Asset From Storage
public class AssetLoader
{
    //Asset Bundle   
    private AssetBundle _assetBundle;

    // Is Avaliable
    public bool isAv = false;

    //Get Gameobject With Name
    public GameObject this[string name] {
        get => _GetAsset(name);
    }

    //Constructer For Loading Bundle
    public AssetLoader(string path) {

        //For Windows Build
#if UNITY_EDITOR
        //Path To File
        this._assetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + path);

        //Is Error Happend
        if (this._assetBundle == null) {
            //Error Message
            Debug.LogError("AssetBundle Can Not Loaded(" + path + ")");

            //Set Is Avaliable False
            this.isAv = false;
            return;
        }

        this.isAv = true;
        //For Android Build
#elif UNITY_ANDROID
        //Async Network Request For Android
        this.GetAssetBundleNetwork(path);
#endif


        //Set Is Avaliable True If Nothing Happend

    }

    //Get Asset From Bundle
    private GameObject _GetAsset(string name) {
        GameObject tmp = _assetBundle.LoadAsset<GameObject>(name);
        if (tmp == null) Debug.LogError("Asset Not Found(" + name + ")");
        this.isAv = true;
        return tmp;
    }

    public Object GetAsset( string name)
    {
        Object tmp = _assetBundle.LoadAsset(name);
        if (tmp == null) Debug.LogError("Asset Not Found(" + name + ")");
        this.isAv = true;
        return tmp;
    }

    public string[] GetAllName()
    {
        return _assetBundle.AllAssetNames();
    } 

    public static string PathToName(string path)
    {
        return path.Substring(path.LastIndexOf("/"), path.LastIndexOf(".") - path.LastIndexOf("/"));
    }

    public static string[] PathToName(string[] paths)
    {
        List<string> tmp = new List<string>(paths);
        tmp.ForEach(name => AssetLoader.PathToName(name));
        return tmp.ToArray();
    }

    //For Android Network Request
    private void GetAssetBundleNetwork( string path)
    {
        //Path To Asset Bundle
        string url = "jar:file://" + Application.dataPath + "!assets/" + path;
        
        //Request Preapering
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url);

        //Sending Request
        request.SendWebRequest();

        while (!request.isDone) ;

        //Set Response Data To Variable
        this._assetBundle = DownloadHandlerAssetBundle.GetContent(request);
        this.isAv = true;

    }

}
