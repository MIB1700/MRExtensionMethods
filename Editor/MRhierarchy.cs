using System.Diagnostics;
using UnityEngine;
using UnityEditor;
// using UnityEditorInternal;
// using System.Reflection;
// using System.IO;
using System.Linq;

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

            if (guids2.Length >= 1)
            {

                var pathToPng = AssetDatabase.GUIDToAssetPath(guids2[0]);

                texturedMR = (Texture2D)AssetDatabase.LoadAssetAtPath<Texture2D>(pathToPng);

                // Debug.Log(texturedMR);
            }
            else
            {

                UnityEngine.Debug.Log($"MR_icon_blue_32x32.png NOT FOUND...");
            }
            isInited = true;
        }

        static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

            if (!gameObject)
            {
                return;
            }

            if (gameObject.name.StartsWith("//", System.StringComparison.Ordinal) || gameObject.name.StartsWith("__", System.StringComparison.Ordinal) ||
                gameObject.name.EndsWith("//", System.StringComparison.Ordinal) || gameObject.name.EndsWith("__", System.StringComparison.Ordinal))
            {
                selectionRect.xMin -= 20;
                selectionRect.xMax += 10;

                var offset = selectionRect;
                offset.xMin += 2;
                offset.yMin += 2;
                offset.width -= 2;
                offset.height -= 2;

                gameObject.SetActive(false);
                // EditorGUI.DrawRect(selectionRect, borderC);

                //check if anything is null...
                var name = gameObject.name;

                (Color backGroundColour, string finalName) = GetColorFromString(name, "bg:");

                name = finalName ?? name;
                (Color textColour, string finalTextName) = GetColorFromString(name, "t:");

                name = finalTextName ?? name;
                (Color borderColour, string finalBorderName) = GetColorFromString(name, "b:");

                name = finalBorderName ?? name;
                name = name.Replace("/", "");

                if (finalBorderName != null)
                {
                    EditorGUI.DrawRect(selectionRect, borderColour);
                }
                else
                {
                    EditorGUI.DrawRect(selectionRect, borderC);
                }

                if (finalName != null)
                {
                    EditorGUI.DrawRect(offset, backGroundColour);
                }
                else
                {
                    EditorGUI.DrawRect(offset, bgC);
                }

                if (finalTextName != null)
                {
                    EditorGUI.LabelField(selectionRect, name, new GUIStyle()
                    {
                        normal = new GUIStyleState() { textColor = textColour },
                        fontStyle = FontStyle.BoldAndItalic,
                        wordWrap = true,
                        alignment = TextAnchor.MiddleCenter
                    }
                    );
                }
                else
                {
                    EditorGUI.LabelField(selectionRect, name, new GUIStyle()
                    {
                        normal = new GUIStyleState() { textColor = textC },
                        fontStyle = FontStyle.BoldAndItalic,
                        wordWrap = true,
                        alignment = TextAnchor.MiddleCenter
                    }
                    );
                }
            }

            if (gameObject.GetComponent(typeof(IMR)))
            {
                Rect rect = new Rect(selectionRect.x - 25f, selectionRect.y, selectionRect.height, selectionRect.height);
                rect.x = selectionRect.x + selectionRect.width - 15f;

                // Debug.Log($"rect: {rect}");
                GUI.Label(rect, texturedMR);
                return;
            }
        }

        //provide the name of the gameobject and the type (e.g. bg, t, b) to look for colour name, also return the
        //name of the gameobject without the colour name in it
        private static (Color color, string finalName) GetColorFromString(string cString, string type)
        {

            Color finalColor = new Color();
            string newCol = "";
            string finalName = cString.Replace("/", "");

            //split string into words, delimited by spaces
            var cols = cString.ToLower().Split(' ');

            //loop through words, checking for the "type" to create the final colour
            foreach (var col in cols)
            {
                if (col.ToLower().Contains(type))
                {
                    //remove any of the filler characters
                    newCol = col.Replace("/", "").Replace("_", "").Replace("-", "").Replace(type, "").Replace(" ", "");
                    finalName = finalName.Replace(type, "").Replace(newCol, "");

                    //convert string colour name to actual Unity Color...
                    if (ColorUtility.TryParseHtmlString(newCol, out finalColor))
                    {
                        return (finalColor, finalName);
                    }
                }
            }
            return (finalColor, null); //Color cannot be null => return null for the string so we can test if
                                       //function was succesful or not
        }
    }
}