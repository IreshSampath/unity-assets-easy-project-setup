using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProjectSetupTool : EditorWindow
{
    string _rootFolderName = "_Project Root Folder";
    string _mainSceneName = "Main";

    [MenuItem("Tools/EasyProjectSetup")]
    public static void ShowWindow()
    {
        GetWindow<ProjectSetupTool>("EasyProjectSetup");
    }

    void OnGUI()
    {
        if (string.IsNullOrEmpty(_rootFolderName))
            EditorGUILayout.HelpBox("⚠️ Please enter a valid root folder name before continuing.", MessageType.Warning);

        else if (string.IsNullOrEmpty(_mainSceneName))
            EditorGUILayout.HelpBox("⚠️ Please enter a valid scene name before continuing.", MessageType.Warning);

        else
            EditorGUILayout.HelpBox("✅ All set! You can now start working on your setup.", MessageType.Info);


        GUILayout.Space(10);
        GUILayout.Label("🗃️ Project Setup", EditorStyles.boldLabel);

        _rootFolderName = EditorGUILayout.TextField("Root Folder Name", _rootFolderName);
        // --- Info ---
        EditorGUILayout.HelpBox("⚠️ To enable folder coloring, start your root folder name with '_'.", MessageType.Info);

        GUILayout.Space(10);
        if (GUILayout.Button("Create Project Root Folders"))
            CreateRootFolders();

        GUILayout.Space(10);
        GUILayout.Label("💡 Scene Setup", EditorStyles.boldLabel);

        _mainSceneName = EditorGUILayout.TextField("Scene Name", _mainSceneName);

        if (GUILayout.Button("Create Main Scene"))
            CreateBaseScene();

        GUILayout.Space(10);
        GUILayout.Label("📍 Hierarchy Setup", EditorStyles.boldLabel);

        if (GUILayout.Button("Create UI")) CreateUI();
        if (GUILayout.Button("Create Managers")) CreateManagers();
        if (GUILayout.Button("Create Basic Environment")) CreateEnvironment();

        GUILayout.Space(10);
        GUILayout.Label("🚀 All-in-One Setup", EditorStyles.boldLabel);
        if (GUILayout.Button("Create all-in-One Setup"))
        {
            CreateRootFolders();
            CreateBaseScene();
            CreateUI();
            CreateManagers();
            CreateEnvironment();
        }

        GUILayout.Space(10);
        GUILayout.Label("🧡 Color Settings", EditorStyles.boldLabel);
        if (GUILayout.Button("Change Colors")) OpenSettings();
        if (GUILayout.Button("Reset Colors")) ResetSettings();
    }

    void CreateRootFolders()
    {
        if (string.IsNullOrEmpty(_rootFolderName))
        {
            Debug.LogWarning("❌ Please enter a valid root folder name.");
            return;
        }

        string basePath = Path.Combine("Assets", _rootFolderName);

        string[] folders =
        {
            $"{basePath}/Animations",
            $"{basePath}/Audio",
            $"{basePath}/Materials",
            $"{basePath}/Prefabs",
            $"{basePath}/Scenes",
            $"{basePath}/Scripts/Managers",
            $"{basePath}/Scripts/Handlers",
            $"{basePath}/Shaders",
            $"{basePath}/Textures"
        };

        foreach (string folder in folders)
        {
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
        }

        AssetDatabase.Refresh();
        Debug.Log($"✅ Default folders created under: Assets/{_rootFolderName}");

        //// If name starts with "_", enable coloring
        //if (_rootFolderName.StartsWith("_"))
        //{
        //    FolderColorizer.SetActiveRoot(_rootFolderName);
        //    Debug.Log($"🎨 Folder coloring activated for '{_rootFolderName}'");
        //}
        //else
        //{
        //    FolderColorizer.SetActiveRoot(""); // disables coloring
        //    Debug.Log($"🧹 Folder coloring disabled (root folder doesn’t start with '_').");
        //}

        // After creating folders / when user changes the root name:
        //if (_rootFolderName.StartsWith("_"))
        //    FolderColorizer.SetActiveRoot(_rootFolderName);
        //else
        //    FolderColorizer.SetActiveRoot(""); // disables all coloring
    }

    void CreateBaseScene()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        scene.name = _mainSceneName;
        SaveScene();
    }

    void SaveScene()
    {
        string basePath = Path.Combine("Assets", _rootFolderName);
        string saveFolder = Path.Combine(basePath, "Scenes");

        if (!Directory.Exists(saveFolder))
            Directory.CreateDirectory(saveFolder);

        string savePath = Path.Combine(saveFolder, $"{_mainSceneName}.unity");

        var scene = EditorSceneManager.GetActiveScene();
        if (!scene.IsValid())
        {
            Debug.LogError("❌ No valid active scene to save.");
            return;
        }

        EditorSceneManager.MarkSceneDirty(scene);
        bool ok = EditorSceneManager.SaveScene(scene, savePath, true);
        if (ok)
            Debug.Log($"✅ Scene saved to: {savePath}");

        AssetDatabase.Refresh();
    }

    void CreateUI()
    {
        var ui = new GameObject("# ---- UI ----");

        if (Object.FindObjectOfType<EventSystem>() == null)
            new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));

        var canvasGO = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        canvasGO.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

        Debug.Log($"✅ UI created!");
        SaveScene();
    }

    void CreateManagers()
    {
        new GameObject("# ---- Managers ----");
        new GameObject("App Manager");
        new GameObject("UI Manager");
        Debug.Log($"✅ Managers created!");
        SaveScene();
    }

    void CreateEnvironment()
    {
        new GameObject("# ---- Environment ----");

        var lightGO = new GameObject("Directional Light");
        var light = lightGO.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1f;
        light.color = Color.white;
        lightGO.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

        var camGO = new GameObject("Main Camera");
        var cam = camGO.AddComponent<Camera>();
        cam.tag = "MainCamera";
        camGO.transform.position = new Vector3(0f, 1f, -10f);

        Debug.Log($"✅ Environment created!");
        SaveScene();
    }

    void OpenSettings()
    {
        var settings = EasyProjectColorSettings.Instance;
        Selection.activeObject = settings;
        EditorGUIUtility.PingObject(settings);
    }

    void ResetSettings()
    {
        var s = EasyProjectColorSettings.Instance;
        var path = AssetDatabase.GetAssetPath(s);
        AssetDatabase.DeleteAsset(path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        // Recreate fresh defaults
        Selection.activeObject = EasyProjectColorSettings.Instance;
    }
}