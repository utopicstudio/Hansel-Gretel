using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEditor;
using UnityEditor.SceneManagement;


public class PrefabConvertWindow : EditorWindow
{
    /// <summary>
    /// Tag to search and replace when converting.
    /// </summary>
    private string SearchTag = "Player";

    /// <summary>
    /// If the convert should deactivate old game objects, or completely remove them.
    /// </summary>
    private bool bDeleteReplacedGameObjects;

    /// <summary>
    /// If the convert should create missing components when replacing references.
    /// </summary>
    private bool bGenerateMissingComponents;

    /// <summary>
    /// Prefab to use when replacing the main VR Character.
    /// </summary>
    private GameObject Prefab = null;

    /// <summary>
    /// Context for the scene that called the convert.
    /// </summary>
    private string SceneContext;

    /// <summary>
    /// Build scenes that we're currently working on
    /// </summary>
    private UnityEditor.EditorBuildSettingsScene[] BuildScenes;

    /// <summary>
    /// Current scene we're working on
    /// @see BuildScenes
    /// </summary>
    private int CurrentBakeScene;

    [MenuItem("Utopic/PrefabConvert")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(PrefabConvertWindow));
    }

    private void OnGUI()
    {
        titleContent = new GUIContent("PrefabConvert");

        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        SearchTag = EditorGUILayout.TagField("Search Tag", SearchTag);
        bDeleteReplacedGameObjects = EditorGUILayout.Toggle("Delete old", bDeleteReplacedGameObjects);
        bGenerateMissingComponents = EditorGUILayout.Toggle("Generate missing components", bGenerateMissingComponents);
        Prefab = (GameObject)EditorGUILayout.ObjectField("VR Prefab", Prefab, typeof(GameObject), false);

        EditorGUILayout.Space();
        //GUILayout.Label("Convert will search and save any scene on the BuildSettings list, make sure you have backed up your project before continuing!", EditorStyles.boldLabel);
        //EditorGUILayout.Space();

        EditorGUI.BeginDisabledGroup(Prefab == null || SearchTag == "");
        if (GUILayout.Button("Convert"))
        {
            BeginConvert();
        }
        EditorGUI.EndDisabledGroup();
    }

    /// <summary>
    /// Stores current scene context and begin conversion to new assets
    /// </summary>
    private void BeginConvert()
    {
        //Init convert variables
        BuildScenes = UnityEditor.EditorBuildSettings.scenes;
        CurrentBakeScene = 0;
        SceneContext = UnityEngine.SceneManagement.SceneManager.GetActiveScene().path;

        if (BuildScenes.Length > 0)
        {
            //Will need to manage level loading for resources
            UnityEditor.SceneManagement.EditorSceneManager.sceneOpened += OnConvertLevelOpened;

            //Begin baking, we will start with the current bake scene
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene(BuildScenes[CurrentBakeScene].path);
        }
    }

    private void EndConvert()
    {
        //Show loading bar
        EditorUtility.DisplayProgressBar("Converting scenes...", "Wrapping up...", 1.0f);
        UnityEditor.SceneManagement.EditorSceneManager.sceneOpened -= OnConvertLevelOpened;

        //Finally open the context scene
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene(SceneContext);
        EditorUtility.ClearProgressBar();
    }

    /// <summary>
    /// Called when loading a scene while on the convert process
    /// </summary>
    /// <param name="scene">Scene that was loaded</param>
    /// <param name="mode"> Mode in which the scene was loaded </param>
    void OnConvertLevelOpened(Scene scene, OpenSceneMode mode)
    {
        //Update progress
        EditorUtility.DisplayProgressBar("Converting scenes...", "Searching and replacing prefabs...", ((float)CurrentBakeScene) / ((float)BuildScenes.Length));

        //For any reason, the required game objects can be inactive, we need a proper way to find them
        GameObject[] Roots = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

        for (int i = 0; i < Roots.Length; i++)
        {
            GameObject go = Roots[i];

            if (go.CompareTag(SearchTag))
            {
                GameObject GeneratedPrefab = (GameObject)PrefabUtility.InstantiatePrefab(Prefab);

                //Copy values
                GeneratedPrefab.tag = SearchTag;
                GeneratedPrefab.transform.position = go.transform.position;
                GeneratedPrefab.transform.rotation = go.transform.rotation;
                GeneratedPrefab.transform.localScale = go.transform.localScale;

                //Deactivate/delete old object
                Undo.RecordObject(go, "Replace VR prefab");

                //Do a reference lookup and replace every old player reference to the new instance.
                int ReferenceCount = ReplaceReferences(Roots, go, GeneratedPrefab);
                Debug.Log("Replaced " + ReferenceCount + " component references while converting player.");

                if (bDeleteReplacedGameObjects)
                {
                    Destroy(go);
                }
                else
                {
                    go.SetActive(false);
                }

                //We only support one player, no use in continuing the search
                break;
            }
        }

        //Save the scene
        UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);

        //Finished baking of this scene, request the next one
        if (BuildScenes.Length > ++CurrentBakeScene)
        {
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene(BuildScenes[CurrentBakeScene].path);
        }
        else
        {
            EndConvert();
        }
    }


    /// <summary>
    /// Searches the current scene roots and looks for references to the original game object and tries to replace them with the new object
    /// </summary>
    /// <param name="Roots">Original set of roots which may contain the original GameObject, but not the NewObject</param>
    /// <param name="Original">Original object that was in the scene and needs to be replaced</param>
    /// <param name="NewObject">New generated object that will replace the original</param>
    /// <returns>Number of reference replacements that took place.</returns>
    int ReplaceReferences(GameObject[] Roots, GameObject Original, GameObject NewObject)
    {
        int NumberOfReplacements = 0;
        for (int i = 0; i < Roots.Length; i++)
        {
            Component[] components = Roots[i].GetComponentsInChildren<Component>();
            foreach (Component comp in components)
            {
                //Components can be null on some cases
                if (comp)
                {
                    SerializedObject sObject = new SerializedObject(comp);
                    SerializedProperty sProperty = sObject.GetIterator();
                    while (sProperty.NextVisible(true))
                    {
                        bool bObjectField = sProperty.propertyType == SerializedPropertyType.ObjectReference && sProperty.objectReferenceValue != null;
                        if (bObjectField)
                        {
                            Component componentReference = sProperty.objectReferenceValue as Component;
                            if (componentReference && componentReference.gameObject == Original)
                            {
                                //Found a referencing component, need to replace if possible.
                                Debug.Log("Found referencing component");
                                Component NewComponent = NewObject.GetComponent(componentReference.GetType());

                                //Chance to create the component if its missing
                                if(NewComponent == null && bGenerateMissingComponents)
                                {
                                    NewComponent = NewObject.AddComponent(componentReference.GetType());

                                    //We need to copy the values
                                    System.Reflection.FieldInfo[] fields = componentReference.GetType().GetFields();
                                    foreach (System.Reflection.FieldInfo field in fields)
                                    {
                                        field.SetValue(NewComponent, field.GetValue(componentReference));
                                    }
                                }

                                //Component could be null if it doesn't exist on the replaced object and we're not allowing component generation.
                                if (NewComponent)
                                {
                                    sProperty.objectReferenceValue = NewComponent;
                                    sObject.ApplyModifiedProperties();
                                    NumberOfReplacements++;
                                }
                            }
                        }
                    }
                }
            }
        }

        return NumberOfReplacements;
    }
}

