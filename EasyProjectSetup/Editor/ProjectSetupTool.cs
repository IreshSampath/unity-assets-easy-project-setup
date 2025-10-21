using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProjectSetupTool : EditorWindow
{
    string _rootFolderName = "_Project Root Folder"; // Default value
    string _mainSceneName = "Main"; // Default value

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

        GUILayout.Label("🗃️ Project Setup", EditorStyles.boldLabel);

        GUILayout.Space(5);
        // Input field for folder name
        _rootFolderName = EditorGUILayout.TextField("Root Folder Name", _rootFolderName);
        GUILayout.Space(10);

        GUI.enabled = !string.IsNullOrEmpty(_rootFolderName);

        if (GUILayout.Button("Create Project Root Folders"))
        {
            CreateRootFolders();
        }

        GUILayout.Label("💡 Scene Setup", EditorStyles.boldLabel);
        GUILayout.Space(5);
        // Input field for scene name
        _mainSceneName = EditorGUILayout.TextField("Scene Name", _mainSceneName);
        GUILayout.Space(10);
        GUI.enabled = !string.IsNullOrEmpty(_mainSceneName);


        if (GUILayout.Button("Create Main Scene"))
        {
            CreateBaseScene();
        }

        GUILayout.Label("📍 Hierarchy Setup", EditorStyles.boldLabel);

        if (GUILayout.Button("Create UI"))
        {
            CreateUI();
        }

        if (GUILayout.Button("Create Managers"))
        {
            CreateManagers();
        }

        if (GUILayout.Button("Create Basic Environment"))
        {
            CreateEnvironment();
        }

        GUILayout.Label("🚀 All-in-One Setup", EditorStyles.boldLabel);


        if (GUILayout.Button("Create all-in-One Setup"))
        {
            CreateRootFolders();
            CreateBaseScene();
            CreateUI();
            CreateManagers();
            CreateEnvironment();
        }


        GUI.enabled = true; // re-enable GUI


    }

    void CreateRootFolders()
    {
        if (string.IsNullOrEmpty(_rootFolderName))
        {
            Debug.LogWarning("❌ Please enter a valid root folder name.");
            return;
        }

        string basePath = Path.Combine("Assets", _rootFolderName);

        string[] folders = {
            $"{basePath}/Scenes",
            $"{basePath}/Scripts",
            $"{basePath}/Prefabs",
            $"{basePath}/Materials",
            $"{basePath}/Textures"
        };

        foreach (string folder in folders)
        {
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
        }

        AssetDatabase.Refresh();
        Debug.Log($"✅ Default folders created under: Assets/{_rootFolderName}");
    }

    void CreateBaseScene()
    {
        // Create a new empty scene
        var scene = UnityEditor.SceneManagement.EditorSceneManager.NewScene(
            UnityEditor.SceneManagement.NewSceneSetup.EmptyScene);
        scene.name = _mainSceneName;

        //// === ENSURE SAVE PATH EXISTS ===
        //string basePath = Path.Combine("Assets", _rootFolderName);
        //string saveFolder = $"{basePath}/Scenes";
        //if (!Directory.Exists(saveFolder))
        //    Directory.CreateDirectory(saveFolder);

        //string savePath = Path.Combine(saveFolder, "Main.unity");

        //// === SAVE THE SCENE ===
        //bool saved = EditorSceneManager.SaveScene(scene, savePath);
        //if (saved)
        //    Debug.Log($"✅ Scene created and saved to: {savePath}");
        //else
        //    Debug.LogError("❌ Failed to save scene!");

        //AssetDatabase.Refresh();

        SaveScene();
    }

    void SaveScene()
    {
                // === ENSURE SAVE PATH EXISTS ===
        string basePath = Path.Combine("Assets", _rootFolderName);
        string saveFolder = Path.Combine(basePath,"Scenes");

        if (!Directory.Exists(saveFolder))
            Directory.CreateDirectory(saveFolder);

        string savePath = Path.Combine(saveFolder, $"{_mainSceneName}.unity");

        var scene = EditorSceneManager.GetActiveScene();
        if (!scene.IsValid())
        {
            Debug.LogError("❌ No valid active scene to save.");
            return;
        }

        // Make sure Unity considers the scene modified (procedural edits may happen between frames)
        EditorSceneManager.MarkSceneDirty(scene);

        // Save *to the target path* (works whether scene was previously unsaved or already saved elsewhere)
        bool ok = EditorSceneManager.SaveScene(scene, savePath, true);
        if (ok)
            Debug.Log($"✅ Scene saved to: {savePath}");
        else
            Debug.LogError("❌ Failed to save scene.");

        AssetDatabase.Refresh();
    }

    void CreateUI()
    {
        // === UI ===
        var ui = new GameObject("# ---- UI ----");

        // === UI CANVAS & EVENT SYSTEM ===
        // Event System (avoid duplicates)
        if (Object.FindObjectOfType<EventSystem>() == null)
        {
            var eventSys = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            //eventSys.transform.SetParent(ui.transform);
        }

        var canvasGO = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        var canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        //canvasGO.transform.SetParent(ui.transform);
        Debug.Log($"✅ UI created !");
        SaveScene();
    }

    void CreateManagers()
    {
        // === Managers ===
        var managers = new GameObject("# ---- Managers ----");
        var appManager = new GameObject("App Manager");
        var uiManager = new GameObject("UI Manager");

        Debug.Log($"✅ Managers created !");
        SaveScene();
    }

    void CreateEnvironment()
    {
        // === Environment ===
        var env = new GameObject("# ---- Environment ----");

        // === DIRECTIONAL LIGHT ===
        var lightGO = new GameObject("Directional Light");
        var light = lightGO.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1f;
        light.color = Color.white;
        lightGO.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
        //lightGO.transform.SetParent(lighting.transform);

        // === MAIN CAMERA ===
        var camGO = new GameObject("Main Camera");
        var cam = camGO.AddComponent<Camera>();
        cam.tag = "MainCamera";
        camGO.transform.position = new Vector3(0f, 1f, -10f);
        //camGO.transform.SetParent(player.transform);

        // === POST-PROCESSING (URP/HDRP COMPATIBLE) ===
#if USING_HDRP
    var volumeGO = new GameObject("Global Volume");
    var volume = volumeGO.AddComponent<UnityEngine.Rendering.Volume>();
    volume.isGlobal = true;
    volume.priority = 1;
    volume.sharedProfile = new UnityEngine.Rendering.VolumeProfile();
    //volumeGO.transform.SetParent(lighting.transform);
#elif USING_URP
    var volumeGO = new GameObject("Global Volume");
    var volume = volumeGO.AddComponent<UnityEngine.Rendering.Volume>();
    volume.isGlobal = true;
    volume.priority = 1;
    volume.sharedProfile = new UnityEngine.Rendering.VolumeProfile();

    // Optional: Add default overrides
    var bloom = volume.sharedProfile.Add<UnityEngine.Rendering.Universal.Bloom>();
    bloom.active = true;
    bloom.intensity.value = 0.5f;

    var vignette = volume.sharedProfile.Add<UnityEngine.Rendering.Universal.Vignette>();
    vignette.active = true;
    vignette.intensity.value = 0.2f;

    //volumeGO.transform.SetParent(lighting.transform);
#endif

        // === ORGANIZE HIERARCHY ===
        //env.transform.SetSiblingIndex(0);
        //managers.transform.SetSiblingIndex(1);
        ////player.transform.SetSiblingIndex(2);
        //ui.transform.SetSiblingIndex(2);

        Debug.Log($"✅ Environment created !");

        SaveScene();
    }
}
