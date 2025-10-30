using System.IO;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class FolderColorizer
{
    static FolderColorizer()
    {
        // Make sure the handler name matches this event's signature.
        EditorApplication.projectWindowItemOnGUI += OnProjectWindowItemGUI;

        // Extra repaint hooks to be safe:
        EditorApplication.projectChanged += RepaintProjectWindow;     // 2021.2+
        Selection.selectionChanged += RepaintProjectWindow;
    }

    static void RepaintProjectWindow()
    {
        EditorApplication.RepaintProjectWindow();
    }

    static void OnProjectWindowItemGUI(string guid, Rect rect)
    {
        string path = AssetDatabase.GUIDToAssetPath(guid);
        if (!AssetDatabase.IsValidFolder(path)) return;

        // Only color folders that are inside ANY '_' root in their ancestry
        if (!IsUnderAnyUnderscoreRoot(path)) return;

        string folderName = Path.GetFileName(path);

        // Pull colors from your ScriptableObject settings (or hardcode if you prefer)
        var s = EasyProjectColorSettings.Instance;

        Color bg = Color.clear;

        if (folderName.StartsWith("_")) bg = s.FolderRoot;
        else if (folderName == "Animations") bg = s.FolderAnimations;
        else if (folderName == "Audio") bg = s.FolderAudio;
        else if (folderName == "Materials") bg = s.FolderMaterials;
        else if (folderName == "Prefabs") bg = s.FolderPrefabs;
        else if (folderName == "Scenes") bg = s.FolderScenes;
        else if (folderName == "Scripts") bg = s.FolderScripts;
        else if (folderName == "Shaders") bg = s.FolderShaders;
        else if (folderName == "Textures") bg = s.FolderTextures;

        if (bg.a <= 0f) return;

        // Draw full row background (works for 1/2 column and icon modes)
        //var full = new Rect(0, rect.y, GUIClip.visibleRect.width, rect.height);
        var full = new Rect(rect.x, rect.y, rect.width, rect.height);

        EditorGUI.DrawRect(full, bg);
    }

    static bool IsUnderAnyUnderscoreRoot(string assetPath)
    {
        // assetPath like: "Assets/_Project/Textures" or "Assets/_Dev/Scenes"
        var parts = assetPath.Split('/');
        for (int i = 1; i < parts.Length; i++)
        {
            if (parts[i].StartsWith("_")) return true;
        }
        return false;
    }

}