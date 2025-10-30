// Assets/.../Editor/Shared/EasyProjectColorSettings.cs
using UnityEditor;
using UnityEngine;

public class EasyProjectColorSettings : ScriptableObject
{
    // ---------- Project (Folders) ----------
    [Header("Project (Folders)")]
    public Color FolderRoot = new(0f, 0f, 0f, 0.31f); // _root
    public Color FolderAnimations = new(0.90f, 0.55f, 0.25f, 0.20f);
    public Color FolderAudio = new(0.85f, 0.30f, 0.40f, 0.20f);
    public Color FolderMaterials = new(0.75f, 0.45f, 0.80f, 0.20f);
    public Color FolderPrefabs = new(0.35f, 0.75f, 0.90f, 0.20f);
    public Color FolderScenes = new(0.25f, 0.55f, 0.90f, 0.20f);
    public Color FolderScripts = new(0.45f, 0.70f, 0.35f, 0.20f);
    public Color FolderShaders = new(0.55f, 0.75f, 0.25f, 0.20f);
    public Color FolderTextures = new(0.20f, 0.65f, 0.35f, 0.20f);

    // ---------- Hierarchy (GameObjects) ----------
    [Header("Hierarchy (GameObjects)")]
    public Color H_SectionHeader = new(0.25f, 0.45f, 0.75f, 0.25f); // names starting with '#'
    public Color H_Manager = new(0.95f, 0.65f, 0.25f, 0.25f); // contains 'Manager'
    public Color H_UI = new(0.25f, 0.85f, 0.95f, 0.25f); // contains 'UI'
    public Color H_Environment = new(0.25f, 0.65f, 0.35f, 0.25f); // contains 'Environment'
    public Color H_LightCamera = new(0.65f, 0.45f, 0.85f, 0.25f); // contains 'Light'/'Camera'
    public Color H_Volume = new(0.95f, 0.45f, 0.65f, 0.25f); // contains 'Volume'

    [Header("Hierarchy Text Colors")]
    public Color H_TextLight = new(0.95f, 0.95f, 0.95f, 1f);
    public Color H_TextDark = new(0.25f, 0.25f, 0.25f, 1f);

    // ---------- Loader / Creator ----------
    private const string DefaultAssetPath = "Assets/Settings/EasyProjectColors.asset";
    private static EasyProjectColorSettings _cached;

    public static EasyProjectColorSettings Instance
    {
        get
        {
            if (_cached != null) return _cached;

            // Try find
            var guids = AssetDatabase.FindAssets("t:EasyProjectColorSettings");
            if (guids != null && guids.Length > 0)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                _cached = AssetDatabase.LoadAssetAtPath<EasyProjectColorSettings>(path);
                if (_cached) return _cached;
            }

            // Create default
            var dir = System.IO.Path.GetDirectoryName(DefaultAssetPath);
            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);

            _cached = CreateInstance<EasyProjectColorSettings>();
            AssetDatabase.CreateAsset(_cached, DefaultAssetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return _cached;
        }
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        EditorApplication.RepaintProjectWindow();
        EditorApplication.RepaintHierarchyWindow();
    }
#endif
}
