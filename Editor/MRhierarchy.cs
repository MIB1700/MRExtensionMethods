using UnityEngine;
using UnityEditor;
using System;


namespace MR.CustomExtensions
{
    [InitializeOnLoad]
    public class MRhierarchy : /*MonoBehaviour*/ Editor
    {
        static Texture2D texturedMR;

        static bool isInited = false;

        //types we want to be able to use
        //TODO: would be nice if we could say here what types they are or if they are switches
        static private string[] types= {"bg:", "b:", "t:", "bs:", "ts:"};

        static MRhierarchy()
        {
            EditorApplication.hierarchyWindowItemOnGUI -= HierarchyWindowItemOnGUI;
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
        }

        //initialize the png asset...
        static void Initialize()
        {
            if (isInited)
            {
                return;
            }

            string[] guids2 = AssetDatabase.FindAssets("MR_icon_blue_32x32 t:texture2D");

            if (guids2.Length >= 1)
            {
                var pathToPng = AssetDatabase.GUIDToAssetPath(guids2[0]);
                texturedMR = (Texture2D)AssetDatabase.LoadAssetAtPath<Texture2D>(pathToPng);
            }
            else
            {
                UnityEngine.Debug.LogError($"MR_icon_blue_32x32.png NOT FOUND...");
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

            Rect BackgroundRect = new Rect();

            //if the name of the GO starts with a double "//" i.e. like a comment...
            if (gameObject.name.StartsWith("//", System.StringComparison.Ordinal))
            {
                Color backGroundColour  = UnityEngine.Color.black;
                Color borderColour      = UnityEngine.Color.white;
                Color textColour        = UnityEngine.Color.red;

                float textSize = 12f;
                float borderSize = 2f;

                float xPos  = selectionRect.position.x + 60f - 28f - selectionRect.xMin;
                float yPos  = selectionRect.position.y;
                float xSize = selectionRect.size.x + selectionRect.xMin + 28f - 60 + 16f;
                float ySize = selectionRect.size.y;

                BackgroundRect = new Rect(xPos, yPos, xSize, ySize);

                var offset = BackgroundRect.Shrink(borderSize);
                gameObject.SetActive(false);

                //make sure "/" is removed even if no types are changed!
                var name = gameObject.name.Replace("/", "");

                //loop through array of known types and do magic...
                foreach (var type in types)
                {
                    (string after, string finalName) = GetStringAfterType(name, type);

                    if (after == null) {
                        //if that type wasn't found in string => bail
                        continue;
                    }

                    //only use the "finalName" if it isn't null
                    name = finalName ?? name;

                    //deal with the types
                    switch (type)
                    {
                        case "bg:":
                            backGroundColour = ConvertStringToColor(after, Color.white);
                            break;
                        case "b:":
                            borderColour = ConvertStringToColor(after, Color.white);
                            break;
                        case "t:":
                            textColour = ConvertStringToColor(after, Color.white);
                            break;
                        case "bs:":
                            offset = BackgroundRect.Shrink(ConvertStringToFloat(after, 2));
                            break;
                        case "ts:":
                            textSize = ConvertStringToFloat(after, 12);
                            break;
                    }
                }

                //all variables are set, now draw and put the text
                EditorGUI.DrawRect(BackgroundRect, borderColour);
                EditorGUI.DrawRect(offset, backGroundColour);

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

            //TODO: figure out why this does not draw the image... check proper rect location!
            if (gameObject.GetComponent(typeof(IMR))){

                DrawIcon(BackgroundRect, texturedMR);
            }
        }

        private static (string withoutType, string final) GetStringAfterType(string cString, string type) {

            string newStr = "";
            string finalName = CheckForWhiteSpaceAfterType(cString, type);

            //remove the "//" since we done't want to see this on the GO in the hierarchy
            finalName = finalName.Replace("/", "");

            //split string into words, delimited by spaces
            var strgs = finalName.Split(' ');

            //loop through words, checking for the "type", return the string after the type and a string
            //containing the type AND the string after it!
            foreach (var str in strgs)
            {
                if (str.Contains(type))
                {
                    //remove any of the filler characters
                    newStr = str.Replace("_", "").Replace("-", "").Replace(type, "").Replace(" ", "");
                    finalName = finalName.Replace(str, "");

                    return (newStr, finalName);
                }
            }
            return (null, null);
        }

        //provide the name of the gameobject and the type (e.g. bg, t, b) to look for colour name, also return the
        //name of the gameobject without the colour name in it

        //try to convert the string to a Unity Color...
        private static Color ConvertStringToColor(string noType, Color defCol)
        {
            if (noType == null) {

                return defCol;
            }
            if (ColorUtility.TryParseHtmlString(noType, out Color finalColor))
            {
                return finalColor;
            }
            else
            {
                UnityEngine.Debug.LogError($"Color: {noType} could not be converted...");
                return defCol;
            }
        }

        private static float ConvertStringToFloat(string noType, float def) {

            if (Single.TryParse(noType, out float offset))
            {
                return offset;
            }
            return def;
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


        //this is for drawing the image... not useful yet...
        private static void DrawIcon(Rect selectionRect, Texture2D icon) {

            //look for assets only if we are actually trying to use them...
            Initialize();
            selectionRect.x = selectionRect.x + selectionRect.width - 15f;

                // Debug.Log($"rect: {rect}");
            GUI.Label(selectionRect, icon);
        }
    }
}