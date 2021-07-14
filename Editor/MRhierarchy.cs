using System.Diagnostics;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections;
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
                selectionRect.xMin -= 20;
                selectionRect.xMax += 10;

                var offset = selectionRect;
                offset.xMin += 2;
                offset.yMin += 2;
                offset.width -= 2;
                offset.height -= 2;

                gameObject.SetActive(false);
                // EditorGUI.DrawRect(selectionRect, borderC);

                var name = gameObject.name;

                (UnityEngine.Color backGroundColour, string finalName) = GetColorFromString(name, "bg:");

                name = finalName ?? name;
                (UnityEngine.Color textColour, string finalTextName) = GetColorFromString(name, "t:");

                name = finalTextName ?? name;
                (UnityEngine.Color borderColour, string finalBorderName) = GetColorFromString(name, "b:");

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
        private static (UnityEngine.Color color, string finalName) GetColorFromString(string cString, string type)
        {
            UnityEngine.Color finalColor = UnityEngine.Color.white;
            string newCol = "";
            string finalName = "";
            string nameCheck = cString;

            var typeExists = nameCheck.IndexOf(type, System.StringComparison.Ordinal);

            if (typeExists >= 0) {

                var typeLength = type.Length;
                var spaceLocation = (typeExists + typeLength);
                var stillSpace = true;

                //remove any whitespace after a "type"; e.g.: bg:  red needs to be bg:red
                //otherwise the split won't work below
                while (stillSpace) {

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

            //remove the "//" since we done't want to see this on the GO in the hierarchy
            finalName = nameCheck.Replace("/", "");

            //split string into words, delimited by spaces
            var cols = finalName.ToLower().Split(' ');

            //loop through words, checking for the "type" to create the final colour
            foreach (var col in cols)
            {
                if (col.ToLower().Contains(type))
                {
                    //remove any of the filler characters
                    newCol = col.Replace("_", "").Replace("-", "").Replace(type, "");

                    //either a colour name is left or nothing (i.e. something went wrong)
                    if (!String.IsNullOrEmpty(newCol)) {
                        //this is the display name of the GO
                        finalName = finalName.Replace(newCol, "").Replace(type, "");
                    }

                    // if (newCol.Equals("rand", System.StringComparison.OrdinalIgnoreCase)) {

                    //     newCol = allColors[UnityEngine.Random.Range(0, allColors.Length)];
                    //     UnityEngine.Debug.Log($"randCol: {newCol}");
                    // }

                    //convert string colour name to actual Unity Color...
                    if (ColorUtility.TryParseHtmlString(newCol, out finalColor))
                    {
                        return (finalColor, finalName);
                    }
                    else {
                        //if unity can't convert the string then let C# try...
                        // UnityEngine.Debug.Log($"{newCol} is not a UNITY COLOR, try C#...");
                        var fColor = System.Drawing.Color.FromName(newCol);
                        finalColor = ConvertSystemToUnityColor(fColor);

                        //.FromName() returns a color or ARGB==0000 if the string can't be converted
                        if (finalColor.a == 0 && finalColor.r == 0 &&
                            finalColor.g == 0 && finalColor.b == 0) {

                            finalColor = UnityEngine.Color.white;
                        }

                        //default to white if the color string isn't known
                         return (finalColor, finalName);
                    }
                }
            }

            return (finalColor, null); //Color cannot be null => return null for the string so we can test if
                                       //function was succesful or not
        }

        private static UnityEngine.Color ConvertSystemToUnityColor(System.Drawing.Color color) {

            UnityEngine.Color unityColor = UnityEngine.Color.white;

            unityColor.a = color.A;
            unityColor.r = color.R;
            unityColor.g = color.G;
            unityColor.b = color.B;

            return unityColor;
        }

        static string[] allColors = {
            "AliceBlue",
            "AntiqueWhite",
            "Aqua",
            "Aquamarine",
            "Azure",
            "Beige",
            "Bisque",
            "Black",
            "BlanchedAlmond",
            "Blue",
            "BlueViolet",
            "Brown",
            "BurlyWood",
            "CadetBlue",
            "Chartreuse",
            "Chocolate",
            "Coral",
            "CornflowerBlue",
            "Cornsilk",
            "Crimson",
            "Cyan",
            "DarkBlue",
            "DarkCyan",
            "DarkGoldenrod",
            "DarkGray",
            "DarkGreen",
            "DarkKhaki",
            "DarkMagenta",
            "DarkOliveGreen",
            "DarkOrange",
            "DarkOrchid",
            "DarkRed",
            "DarkSalmon",
            "DarkSeaGreen",
            "DarkSlateBlue",
            "DarkSlateGray",
            "DarkTurquoise",
            "DarkViolet",
            "DeepPink",
            "DeepSkyBlue",
            "DimGray",
            "DodgerBlue",
            "Firebrick",
            "FloralWhite",
            "ForestGreen",
            "Fuchsia",
            "Gainsboro",
            "GhostWhite",
            "Gold",
            "Goldenrod",
            "Gray",
            "Green",
            "GreenYellow",
            "Honeydew",
            "HotPink",
            "IndianRed",
            "Indigo",
            "IsKnownColor",
            "IsNamedColor",
            "IsSystemColor",
            "Ivory",
            "Khaki",
            "Lavender",
            "LavenderBlush",
            "LawnGreen",
            "LemonChiffon",
            "LightBlue",
            "LightCoral",
            "LightCyan",
            "LightGoldenrodYellow",
            "LightGray",
            "LightGreen",
            "LightPink",
            "LightSalmon",
            "LightSeaGreen",
            "LightSkyBlue",
            "LightSlateGray",
            "LightSteelBlue",
            "LightYellow",
            "Lime",
            "LimeGreen",
            "Linen",
            "Magenta",
            "Maroon",
            "MediumAquamarine",
            "MediumBlue",
            "MediumOrchid",
            "MediumPurple",
            "MediumSeaGreen",
            "MediumSlateBlue",
            "MediumSpringGreen",
            "MediumTurquoise",
            "MediumVioletRed",
            "MidnightBlue",
            "MintCream",
            "MistyRose",
            "Moccasin",
            "NavajoWhite",
            "Navy",
            "OldLace",
            "Olive",
            "OliveDrab",
            "Orange",
            "OrangeRed",
            "Orchid",
            "PaleGoldenrod",
            "PaleGreen",
            "PaleTurquoise",
            "PaleVioletRed",
            "PapayaWhip",
            "PeachPuff",
            "Peru",
            "Pink",
            "Plum",
            "PowderBlue",
            "Purple",
            "Red",
            "RosyBrown",
            "RoyalBlue",
            "SaddleBrown",
            "Salmon",
            "SandyBrown",
            "SeaGreen",
            "SeaShell",
            "Sienna",
            "Silver",
            "SkyBlue",
            "SlateBlue",
            "SlateGray",
            "Snow",
            "SpringGreen",
            "SteelBlue",
            "Tan",
            "Teal",
            "Thistle",
            "Tomato",
            "Transparent",
            "Turquoise",
            "Violet",
            "Wheat",
            "White",
            "WhiteSmoke",
            "Yellow",
            "YellowGreen"
        };
    }
}