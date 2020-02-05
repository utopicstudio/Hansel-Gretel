using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SceneBundleIndex))]
public class SceneBundleIndexEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SceneBundleIndex Index = (SceneBundleIndex)target;
        IEnumerable<SceneBundleIndex.SceneBundleIndexEntry> Entries = Index.GetEntries();

        //Draw the list
        GUILayout.Label("Scene Bundles:", EditorStyles.boldLabel);

        if(Index.Num() > 0)
        {
            foreach (SceneBundleIndex.SceneBundleIndexEntry e in Entries)
            {
                GUILayout.Label("Scene: " + e.Scene + " Bundle: " + e.AssetBundleName);
            }
        }
        else
        {
            GUILayout.Label("No bundles associated.", EditorStyles.miniLabel);
        }
    }
}
