using System.Diagnostics;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using System.IO;

namespace MR.CustomExtensions
{
    [InitializeOnLoad]
    public class MRhierarchy : /*MonoBehaviour*/ Editor
    {
        static Texture2D texturedMR;
        // static string editorPath = "";
        // static string editorGUIPath = "";
        static bool isInited = false;

        public static Color bgC = Color.black;
        public static Color borderC = Color.white;
        public static Color textC = Color.red;

        static MRhierarchy()
        {
            EditorApplication.hierarchyWindowItemOnGUI -= HierarchyWindowItemOnGUI;
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
            Initialize();
        }

        static void Initialize()
        {
            if (isInited)
            {
                return;
            }

            string[] guids2 = AssetDatabase.FindAssets("MR_icon_blue_15x15 t:texture2D");

            if (guids2.Length >= 1) {

                var pathToPng = AssetDatabase.GUIDToAssetPath(guids2[0]);

                texturedMR =(Texture2D)AssetDatabase.LoadAssetAtPath<Texture2D>(pathToPng);

                // Debug.Log(texturedMR);
            }
            else {

                UnityEngine.Debug.Log($"MR_icon_blue_15x15.png NOT FOUND...");
            }
            isInited = true;
        }

        static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

            if (!gameObject){
                return;
            }

            if (gameObject.name.StartsWith("//", System.StringComparison.Ordinal) || gameObject.name.StartsWith("__", System.StringComparison.Ordinal) ||
                gameObject.name.EndsWith("//", System.StringComparison.Ordinal) || gameObject.name.EndsWith("__", System.StringComparison.Ordinal))
            {
                selectionRect.xMin -= 20;
                selectionRect.xMax += 10;

                var offset = selectionRect;
                offset.xMin += 1;
                offset.yMin += 1;
                offset.width -= 1;
                offset.height -= 1;

                gameObject.SetActive(false);

                EditorGUI.DrawRect(selectionRect, borderC);
                EditorGUI.DrawRect(offset, bgC);

                // EditorGUI.DropShadowLabel(selectionRect, gameObject.name.Replace("/", "").ToUpperInvariant());

                EditorGUI.LabelField(selectionRect, gameObject.name.Replace("/", ""), new GUIStyle()
                {
                    normal = new GUIStyleState() { textColor = textC },
                    fontStyle = FontStyle.BoldAndItalic,
                    wordWrap = true,
                    alignment = TextAnchor.MiddleCenter
                }
                );
            }

            // Rect rect = new Rect(selectionRect.x - 25f, selectionRect.y + 2, 15f, 15f);

            // if (gameObject.GetComponent<>())
            // {
            //     rect.x = selectionRect.x + selectionRect.width - 15f;

            //     // Debug.Log($"rect: {rect}");
            //     GUI.Label(rect, texturedMR);
            //     return;
            // }
        }
    }
}