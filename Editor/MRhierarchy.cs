using System.Diagnostics;
using UnityEngine;
using UnityEditor;
using System;
// using UnityEditorInternal;
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

        public static UnityEngine.Color bgC = UnityEngine.Color.black;
        public static UnityEngine.Color borderC = UnityEngine.Color.white;
        public static UnityEngine.Color textC = UnityEngine.Color.red;

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

            //if the name of the GO starts with a double "//" i.e. like a comment...
            if (gameObject.name.StartsWith("//", System.StringComparison.Ordinal))
            {
                float xPos = selectionRect.position.x + 60f - 28f - selectionRect.xMin;
                float yPos = selectionRect.position.y;
                float xSize = selectionRect.size.x + selectionRect.xMin + 28f - 60 + 16f;
                float ySize = selectionRect.size.y;

                Rect BackgroundRect = new Rect(xPos, yPos, xSize, ySize);

                // selectionRect.xMin -= 26;
                // selectionRect.xMax += 16;

                var offset = BackgroundRect;

                gameObject.SetActive(false);
                // EditorGUI.DrawRect(selectionRect, borderC);

                var name = gameObject.name;

                (UnityEngine.Color backGroundColour, string finalName)   = GetColorFromString(name, "bg:");

                name = finalName ?? name;
                (UnityEngine.Color textColour, string finalTextName)     = GetColorFromString(name, "t:");

                name = finalTextName ?? name;
                (UnityEngine.Color borderColour, string finalBorderName) = GetColorFromString(name, "b:");

                name = finalBorderName ?? name;
                (float bOffset, string offsetName) = GetNumberFromString(name, "bs:", 2);

                name = offsetName ?? name;
                (float textSize, string textSizeName) = GetNumberFromString(name, "ts:", 12);

                name = textSizeName ?? name;
                name = name.Replace("/", "");

                offset.xMin     += bOffset;
                offset.yMin     += bOffset;
                offset.width    -= bOffset;
                offset.height   -= bOffset;


                if (finalBorderName != null)
                {
                    EditorGUI.DrawRect(BackgroundRect, borderColour);
                }
                else
                {
                    EditorGUI.DrawRect(BackgroundRect, borderC);
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
                    EditorGUI.LabelField(BackgroundRect, name, new GUIStyle()
                        {
                            normal = new GUIStyleState() { textColor = textColour },
                            fontStyle = FontStyle.BoldAndItalic,
                            fontSize = (int)textSize,
                            wordWrap = true,
                            alignment = TextAnchor.MiddleCenter
                        }
                    );
                }
                else
                {
                    EditorGUI.LabelField(BackgroundRect, name, new GUIStyle()
                        {
                            normal = new GUIStyleState() { textColor = textC },
                            fontStyle = FontStyle.BoldAndItalic,
                            fontSize = (int)textSize,
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
        private static (UnityEngine.Color color, string finalName) GetColorFromString(string cString, string type)
        {
            UnityEngine.Color finalColor = UnityEngine.Color.white;
            string newCol = "";
            string finalName = CheckForWhiteSpaceAfterType(cString, type);

            //remove the "//" since we done't want to see this on the GO in the hierarchy
            finalName = finalName.Replace("/", "");

            //split string into words, delimited by spaces
            var cols = finalName.Split(' ');

            //loop through words, checking for the "type" to create the final colour
            foreach (var col in cols)
            {
                if (col.Contains(type))
                {
                    //remove any of the filler characters
                    newCol = col.Replace("_", "").Replace("-", "").Replace(type, "").Replace(" ", "");

                    finalName = finalName.Replace(col, "");

                    // // if (newCol.Equals("rand", System.StringComparison.OrdinalIgnoreCase)) {

                    // //     newCol = allColors[UnityEngine.Random.Range(0, allColors.Length)];
                    // //     UnityEngine.Debug.Log($"randCol: {newCol}");
                    // // }

                    // //convert string colour name to actual Unity Color...
                    if (ColorUtility.TryParseHtmlString(newCol, out finalColor))
                    {
                        //  UnityEngine.Debug.Log($"type: {type} col: {newCol}");
                        return (finalColor, finalName);
                    }
                    else
                    {
                        UnityEngine.Debug.LogError($"Color: {newCol} for Type: {type} could not be converted...");
                        return (UnityEngine.Color.white, finalName);
                    }
                }
            }

            return (finalColor, null); //Color cannot be null => return null for the string so we can test if
                                       //function was succesful or not
        }

        private static (float borderSize, string finalName) GetNumberFromString(string cString, string type, float def)
        {
            float offset = def;
            string newCol = "";
            string finalName = "";
            string nameCheck = CheckForWhiteSpaceAfterType(cString, type);

            //remove the "//" since we done't want to see this on the GO in the hierarchy
            finalName = nameCheck.Replace("/", "");

            //split string into words, delimited by spaces
            var cols = finalName.Split(' ');

            //loop through words, checking for the "type" to create the final colour
            foreach (var col in cols)
            {
                if (col.Contains(type))
                {
                    //remove any of the filler characters
                    newCol = col.Replace("_", "").Replace("-", "").Replace(type, "");

                    finalName = finalName.Replace(newCol, "").Replace(type, "");

                    // if (newCol.Equals("rand", System.StringComparison.OrdinalIgnoreCase)) {

                    //     newCol = allColors[UnityEngine.Random.Range(0, allColors.Length)];
                    //     UnityEngine.Debug.Log($"randCol: {newCol}");
                    // }

                    if (Single.TryParse(newCol, out offset))
                    {
                        return(offset, finalName);
                    }

                    return(def, finalName);
                }
            }
            return (def, null);
        }

        private static string CheckForWhiteSpaceAfterType(string cString, string type)
        {
            var typeExists = cString.IndexOf(type, System.StringComparison.Ordinal);
            var nameCheck = cString;

            if (typeExists >= 0)
            {
                var typeLength = type.Length;
                var spaceLocation = (typeExists + typeLength);
                var stillSpace = true;

                //remove any whitespace after a "type"; e.g.: bg:  red needs to be bg:red
                //otherwise the split won't work below
                while (stillSpace)
                {
                    // UnityEngine.Debug.Log($"type: {type} exists at: {cString[spaceLocation]}");

                    if (Char.IsWhiteSpace(nameCheck, spaceLocation)) {

                        // UnityEngine.Debug.Log($"space: {typeExists + typeLength}");
                        nameCheck = nameCheck.Remove(spaceLocation, 1);

                        // UnityEngine.Debug.Log($"space: {cString}");
                    }
                    else {
                        // UnityEngine.Debug.Log($"cString[{checkForSpace}]: {cString[typeExists + typeLength]}");
                        stillSpace = false;
                    }
                }
            }

            return nameCheck;
        }
    }
}