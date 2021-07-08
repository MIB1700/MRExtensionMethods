using UnityEditor;
using MR.CustomExtensions;
public class MRhierarchyWindow : EditorWindow
{
    [MenuItem("Tools/Hierarchy Editor")]
    public static void ShowWindow()
    {
        GetWindow<MRhierarchyWindow>("HierarchyEditor");
    }
    private void OnGUI()
    {
        MRhierarchy.bgC = EditorGUILayout.ColorField("Background Colour", MRhierarchy.bgC);
        MRhierarchy.borderC = EditorGUILayout.ColorField("Border Colour", MRhierarchy.borderC);
        MRhierarchy.textC = EditorGUILayout.ColorField("Text Colour", MRhierarchy.textC);
    }
}