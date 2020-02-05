using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class SceneBundleHelper
{
    /// <summary>
    /// Configuration var for asset bundle directory
    /// </summary>
    private static string kAssetsDirectory = "Assets";

    /// <summary>
    /// Configuration var for asset bundle directory
    /// </summary>
    private static string kAssetBundlesDirectory = "AssetBundles";

    /// <summary>
    /// Obtains the root asset directory 
    /// </summary>
    /// <returns></returns>
    public static string GetAssetsDirectory()
    {
        return kAssetsDirectory;
    }

    /// <summary>
    /// Obtains the name of the asset bundles directory
    /// </summary>
    /// <returns></returns>
    public static string GetAssetBundlesDirectoryName()
    {
        return kAssetBundlesDirectory;
    }

    /// <summary>
    /// Return the local path for a scene bundle
    /// </summary>
    /// <param name="BundleName"></param>
    /// <returns></returns>
    public static string GetLocalPathForBundle(string BundleName)
    {
        return kAssetsDirectory + "/" + kAssetBundlesDirectory + "/" + BundleName;
    }
    
    public static string GetSceneBundleName(string Path)
    {
        SceneAsset Asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(Path);
        return "Scenes/" + Asset.name;
    }

    public static void BuildSceneBundles()
    {
        //Check if the asset bundle directory actually exists.
        if (!AssetDatabase.IsValidFolder(kAssetsDirectory + "/" + kAssetBundlesDirectory))
        {
            AssetDatabase.CreateFolder(kAssetsDirectory, kAssetBundlesDirectory);
        }

        //Create the bundles
        try
        {
            BuildPipeline.BuildAssetBundles(kAssetsDirectory + "/" + kAssetBundlesDirectory, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }
}