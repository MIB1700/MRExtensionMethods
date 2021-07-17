// using UnityEditor;
// using MR.CustomExtensions;
// public class MRhierarchyWindow : EditorWindow
// {
//     [MenuItem("Tools/Hierarchy Editor")]
//     public static void ShowWindow()
//     {
//         GetWindow<MRhierarchyWindow>("HierarchyEditor");
//     }
//     private void OnGUI()
//     {
//         MRhierarchy.backGroundColour = EditorGUILayout.ColorField("Background Colour", MRhierarchy.backGroundColour);
//         MRhierarchy.borderColour = EditorGUILayout.ColorField("Border Colour", MRhierarchy.borderColour);
//         MRhierarchy.textColour = EditorGUILayout.ColorField("Text Colour", MRhierarchy.textColour);
//         MRhierarchy.textSize = EditorGUILayout.FloatField("Text Size: ", MRhierarchy.textSize);
//         MRhierarchy.borderSize = EditorGUILayout.FloatField("Text Size: ", MRhierarchy.borderSize);
//     }
// }