using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class HierarchyColorizer
{
    static HierarchyColorizer()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyGUI;
    }

    static void HandleHierarchyGUI(int instanceID, Rect rect)
    {
        var go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (!go) return;

        var s = EasyProjectColorSettings.Instance;
        var name = go.name;

        Color bg = Color.clear;
        Color txt = s.H_TextDark;
        var style = new GUIStyle(EditorStyles.label);

        if (name.StartsWith("#"))
        {
            bg = s.H_SectionHeader; txt = s.H_TextLight; style.fontStyle = FontStyle.Bold;
        }
        else if (name.Contains("Manager"))
        {
            bg = s.H_Manager; style.fontStyle = FontStyle.Bold;
        }
        else if (name.Contains("UI") || name.Contains("Canvas") || name.Contains("EventSystem"))
        {
            bg = s.H_UI;
        }
        else if (name.Contains("Environment"))
        {
            bg = s.H_Environment;
        }
        else if (name.Contains("Light") || name.Contains("Camera"))
        {
            bg = s.H_LightCamera;
        }
        else if (name.Contains("Volume"))
        {
            bg = s.H_Volume;
        }

        if (bg.a > 0f) EditorGUI.DrawRect(rect, bg);

        style.normal = new GUIStyleState { textColor = txt };
        //EditorGUI.LabelField(rect, name, style);
    }
}
