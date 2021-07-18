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

            //* This needs to be here (and not inside the if "//" statement) because the
            //* icon drawer needs the BackgroundRect info...
            Rect BackgroundRect = new Rect();
            float xPos  = selectionRect.position.x + 60f - 28f - selectionRect.xMin;
            float yPos  = selectionRect.position.y;
            float xSize = selectionRect.size.x + selectionRect.xMin + 28f - 60 + 16f;
            float ySize = selectionRect.size.y;

            BackgroundRect = new Rect(xPos, yPos, xSize, ySize);

            //if the name of the GO starts with a double "//" i.e. like a comment...
            if (gameObject.name.StartsWith("//", System.StringComparison.Ordinal))
            {
                //*default colours!!
                Color backGroundColour  = UnityEngine.Color.black;
                Color borderColour      = UnityEngine.Color.white;
                Color textColour        = UnityEngine.Color.red;

                float textSize = 12f;
                float borderSize = 2f;
                bool borderOn = true;

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
                    //this means we are incrementally removing the type and formatting info
                    //from our name string => only the final string for the LabelField should be left when done
                    name = finalName ?? name;

                    //deal with the types
                    switch (type)
                    {
                        case "bg:":
                            backGroundColour = ConvertStringToColor(after, Color.white);
                            break;
                        case "b:":
                        //* "b:" is always checked  AFTER the "bg:"; that's why we can do this now
                            if (after.Equals("=")) {
                                //when border "=" (i.e. equals) the background color, simple don't draw border at all
                                borderOn = false;
                                //make sure the backgroundColour Rect is resized properly
                                offset = BackgroundRect;
                            }
                            else {
                                borderOn = true;
                                borderColour = ConvertStringToColor(after, Color.white);
                            }
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
                if (borderOn)
                    EditorGUI.DrawRect(BackgroundRect, borderColour);
                EditorGUI.DrawRect(offset, backGroundColour);

                //only draw the text if there is text to draw
                if (!String.IsNullOrEmpty(name)){
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
            }

            if (gameObject.GetComponent(typeof(IMR))){

                //  Debug.Log($"ICON BackgroundRect: {BackgroundRect}");
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

                //remove any whitespace after a "type"; e.g.: "bg:  red" needs to be "bg:red"
                while (stillSpace)
                {
                    if (Char.IsWhiteSpace(nameCheck, spaceLocation)) {

                        nameCheck = nameCheck.Remove(spaceLocation, 1);
                    }
                    else {

                        stillSpace = false;
                    }
                }
            }
            return nameCheck;
        }


        //this is for drawing the image... not useful yet...
        private static void DrawIcon(Rect rect, Texture2D icon) {

            //look for assets only if we are actually trying to use them...
            Initialize();
            // Debug.Log($"ICON ORIGrect: {selectionRect}");
            rect.x = rect.x + rect.width - 15 - 2f;
            // Debug.Log($"ICONrect: {selectionRect}");

            GUI.Label(rect, icon);
        }
    }
}