using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SceneBundleGeneratorWindow : EditorWindow
{
    /// <summary>
    /// If the tool should force replace the bundle names for the scene, to the standard notation.
    /// Or simply use whichever bundle name is already setup
    /// </summary>
    private bool bForceBundleNames = true;

    /// <summary>
    /// If when forcing a bundle name the old name should be removed.
    /// </summary>
    private bool bClearOldBundleNames = true;

    /// <summary>
    /// The index bundle to store this information to
    /// </summary>
    private SceneBundleIndex Index = null;
       

    [MenuItem("Utopic/SceneBundle Generator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(SceneBundleGeneratorWindow));
    }

    private void OnGUI()
    {
        titleContent = new GUIContent("SceneBundle Generator");

        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        bForceBundleNames = EditorGUILayout.Toggle("Force Bundle Name Notation", bForceBundleNames);

        EditorGUI.BeginDisabledGroup(!bForceBundleNames);
        bClearOldBundleNames = EditorGUILayout.Toggle("Clear old bundle names", bClearOldBundleNames);
        EditorGUI.EndDisabledGroup();
        Index = (SceneBundleIndex)EditorGUILayout.ObjectField("Bundle Index", Index, typeof(SceneBundleIndex), false);

        EditorGUILayout.Space();
        GUILayout.Label("This will create bundles for the current BuildTarget only");
        if (GUILayout.Button("Convert"))
        {
            GenerateSceneBundles();
        }
    }

    private void GenerateSceneBundles()
    {
        UnityEditor.EditorBuildSettingsScene[] BuildScenes = UnityEditor.EditorBuildSettings.scenes;

        if(BuildScenes.Length > 0)
        {
            EditorUtility.DisplayProgressBar("Generating bundles...", "Creating bundle names...", 0.0f);

            //Clear any bundle data on the index
            if(Index)
            {
                Index.Clear();
            }

            for (int i = 0; i < BuildScenes.Length; i++)
            {
                EditorBuildSettingsScene SceneInfo = BuildScenes[i];
                EditorUtility.DisplayProgressBar("Generating bundles...", "Creating bundle names...", (float)i / (float)BuildScenes.Length);

                AssetImporter Importer = AssetImporter.GetAtPath(SceneInfo.path);
                string BundleName = SceneBundleHelper.GetSceneBundleName(SceneInfo.path);

                //Check if the asset bundle name is different
                if (string.IsNullOrEmpty(Importer.assetBundleName) || (BundleName != Importer.assetBundleName && bForceBundleNames))
                {
                    string OldBundleName = Importer.assetBundleName;
                    Importer.assetBundleName = SceneBundleHelper.GetSceneBundleName(SceneInfo.path);

                    //Try to clear old bundle if its not in use anymore
                    if (!string.IsNullOrEmpty(OldBundleName))
                    {
                        AssetDatabase.RemoveAssetBundleName(OldBundleName, false);
                    }
                }

                //Optionally store the bundle index
                if(Index)
                {
                    Index.Add(SceneInfo.path, Importer.assetBundleName);
                }
            }

            //Store the index
            if (Index)
            {
                EditorUtility.SetDirty(Index);
            }

            //Second part, generate the bundles
            EditorUtility.DisplayProgressBar("Generating bundles...", "Building bundles for current build target...", 1.0f);
            SceneBundleHelper.BuildSceneBundles();            
            EditorUtility.ClearProgressBar();
        }
    }
}
