using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Manager For AssetBundle ( Preventing From Duplicate Linking )
public static class AssetBundleManager
{
    //Loaders
    private static Dictionary<string, AssetLoader> loader = new Dictionary<string,AssetLoader>();

    //Get Asset Bundle Is Avalible If Not Return False
    public static bool TryGetAssetBundle(string name)
    {
        //Check Is Linked
        if (loader.ContainsKey(name)) {

            //Check Is Avalible If Linked 
            if (!loader[name].isAv) return false;

            //If Linked And Avaliable Return True;
            return true;   
            }


        //If Not Loaded Try Load
        AssetLoader tmp = new AssetLoader(name);
        
        //If Encountred Error Return False
        if (tmp.isAv == false) return false;

        //If Everything Is Fine Return
        AssetBundleManager.loader.Add(name, tmp);
        return true;

    }

    //Get Prefab And Inısıante It
    public static GameObject GetLevel(string bundleName,string assetname /*, Transform Parent*/) {
        return GameObject.Instantiate(AssetBundleManager.loader[bundleName][assetname] , null /* Parent */);
    }

    public static T[] GetAll<T>( string bundleName ) where T: Object
    {
        string[] names = loader[bundleName].GetAllName();

        List<T> list = new List<T>();

        foreach( string name in names )
        {
            list.Add( loader[bundleName].GetAsset(name) as T);
        }

        return list.ToArray();
    }

    //Set Location Of Prefab From Template


}
