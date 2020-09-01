using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


// https://docs.microsoft.com/en-us/dotnet/csharp/codedoc
// https://www.youtube.com/watch?v=mr5xkf6zSzk

//TODO:
//  organize code
//  document
//  remove useless stuff
//  figure out consistency

/// <summary>
/// The main MRExtensionsMethods class
/// Uses the MR.CustomExtensions namespace
/// </summary>
namespace MR.CustomExtensions
{
    public static class MRExtensionMethods
    {
/*******************************************************************/
//
//                    GameObject Functions
//
/*******************************************************************/

#region
        /// <summary>
        /// Removes a component from the current Gameobject.
        /// </summary>
        /// <param name="obj">GameObject from which to remove Component.</param>
        /// <param name="immediate">If set to <c>true</c> destroy immediatly otherwise wait until end of next frame.</param>
        /// <typeparam name="Component">The Component to be removed.</typeparam>
        public static void RemoveComponent<Component>(this GameObject obj, bool immediate = false)
        {
            Component component = obj.GetComponent<Component>();

            if (component != null)
            {
                if (immediate)
                {
                    Object.DestroyImmediate(component as Object, true);
                }
                else
                {
                    Object.Destroy(component as Object);
                }
            }
        }
/*******************************************************************/
        public static void ScaleAllRight(this GameObject obj, float scale)
        {
            //---------------------------------------------------
            //(bigSize / 2.0) - (originalSize / 2.0)

            Vector3 origSize = obj.transform.localScale;
            Vector3 bigSize = origSize * scale;

            obj.transform.localScale = bigSize;

            obj.transform.position = obj.transform.position + ((bigSize * 0.5f) - (origSize * 0.5f));
            //---------------------------------------------------
        }
/*******************************************************************/
        /// <summary>
        /// Scale GameObject to the right only
        /// </summary>
        /// <param name="obj">GameObject to scale</param>
        /// <param name="scale">scaling factor</param>
        public static void ScaleXRight(this GameObject obj, float scale)
        {
            //---------------------------------------------------
            //(bigSize / 2.0) - (originalSize / 2.0)

            Vector3 origSize = obj.transform.localScale;
            Vector3 bigSize = origSize.MultX(scale);

            obj.transform.localScale = bigSize;

            obj.transform.position = obj.transform.position + ((bigSize * 0.5f) - (origSize * 0.5f));
            //---------------------------------------------------
        }
/*******************************************************************/
        /// <summary>
        /// Remove GameObject from List, then destroy it
        /// </summary>
        /// <param name="obj">GameObject to destroy and look for in list.</param>
        /// <param name="list">list of GameObjects.</param>
        /// <param name="time">time until GO is destroyed.</param>
        /// <example>
        /// <code>
        ///  List<GameObject> enemies;
        ///  enemy.DestroyRemoveFromList(enemies, 1f);
        /// </code>
        /// </example>
        public static void DestroyRemoveFromList(this GameObject obj, List<GameObject> list, float timeToDeath)
        {
            list.Remove(obj);

            //destroy the object regardless of if it was in the list
            Object.Destroy(obj, timeToDeath);
        }
/*******************************************************************/
 /// <summary>
        /// Remove GameObject from List, then destroy it
        /// </summary>
        /// <param name="obj">GameObject to destroy and look for in list.</param>
        /// <param name="list">list of GameObjects.</param>
        /// <param name="time">time until GO is destroyed.</param>
        /// <example>
        /// <code>
        ///  List<GameObject> enemies;
        ///  enemies.DestroyRemoveFromList(enemy, 1f);
        /// </code>
        /// </example>
         public static void DestroyRemoveFromList(this List<GameObject> list, GameObject obj, float timeToDeath)
        {
            if (list.Remove(obj))
            {
                //destroy the object ONLY if it was in the list
                Object.Destroy(obj, timeToDeath);
            }
        }
/*******************************************************************/
#endregion
/*******************************************************************/
//
//                    IEnumerator Functions
//
/*******************************************************************/
#region
        /// <summary>
        /// Moves the GameObject to new position over seconds.
        /// </summary>
        /// <param name="objectToMove">Object to move.</param>
        /// <param name="end">end position.</param>
        /// <param name="seconds">time in secs.</param>
        /// <example>
        /// <code>
        /// StartCoroutine(player.transform.position.MoveOverSeconds(targetPosition, 10f));
        /// </code>
        /// </example>
        public static IEnumerator MoveTo(this GameObject objectToMove, Vector3 end, float seconds)
        {

            float elapsedTime = 0;
            Vector3 startingPos = objectToMove.transform.position;
            WaitForEndOfFrame eof = new WaitForEndOfFrame();

            while (elapsedTime < seconds)
            {
                Vector3 lerper = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
                objectToMove.transform.position = lerper;

                elapsedTime += Time.deltaTime;
                yield return eof;
            }
            objectToMove.transform.position = end;

        }
/*******************************************************************/
        public static IEnumerator MoveTo(this GameObject objectToMove, Vector3 end, float seconds, float delay)
        {

            float elapsedTime = 0;
            Vector3 startingPos = objectToMove.transform.position;
            WaitForEndOfFrame eof = new WaitForEndOfFrame();

            yield return new WaitForSeconds(delay);

            while (elapsedTime < seconds)
            {
                Vector3 lerper = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
                objectToMove.transform.position = lerper;

                elapsedTime += Time.deltaTime;
                yield return eof;
            }
            objectToMove.transform.position = end;
        }
/*******************************************************************/
        /// <summary>
        /// Moves/Interpolates Vector3 to new value over seconds.
        /// </summary>
        /// <param name="vectorToMove">Vector3 to change.</param>
        /// <param name="end">Vector3 to move to.</param>
        /// <param name="seconds">time in secs to move to new Vector3.</param>
        /// <example>
        /// <code>
        /// StartCoroutine(currentPosition.MoveOverSeconds(targetPosition, 10f, 2f));
        /// </code>
        /// </example>
        public static IEnumerator MoveTo(this Vector3 vectorToMove, Vector3 end, float seconds, float delay)
        {

            float elapsedTime = 0;
            Vector3 startingPos = vectorToMove;
            WaitForEndOfFrame eof = new WaitForEndOfFrame();

            yield return new WaitForSeconds(delay);

            while (elapsedTime < seconds)
            {
                Vector3 lerper = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
                vectorToMove = lerper;

                elapsedTime += Time.deltaTime;
                yield return eof;
            }
            vectorToMove = end;
        }
/*******************************************************************/
        public static IEnumerator MoveTo(this Vector3 vectorToMove, Vector3 end, float seconds)
        {

            float elapsedTime = 0;
            Vector3 startingPos = vectorToMove;
            WaitForEndOfFrame eof = new WaitForEndOfFrame();

            while (elapsedTime < seconds)
            {
                Vector3 lerper = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
                vectorToMove = lerper;

                elapsedTime += Time.deltaTime;
                yield return eof;
            }
            vectorToMove = end;

        }
/*******************************************************************/
        /// <summary>
        /// Moves the GameObject to new LOCAL position over seconds.
        /// </summary>
        /// <param name="objectToMove">Object to move.</param>
        /// <param name="end">end position in LOCAL coordinate system.</param>
        /// <param name="seconds">time in secs.</param>
        /// <example>
        /// <code>
        /// StartCoroutine(player.transform.localPosition.MoveOverSecondsLocal(localTargetPosition, 10f));
        /// </code>
        /// </example>
        public static IEnumerator MoveToLocal(this GameObject objectToMove, Vector3 end, float seconds)
        {

            float elapsedTime = 0;
            Vector3 startingPos = objectToMove.transform.localPosition;
            WaitForEndOfFrame eof = new WaitForEndOfFrame();
            while (elapsedTime < seconds)
            {
                Vector3 lerper = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
                objectToMove.transform.localPosition = lerper;

                elapsedTime += Time.deltaTime;
                yield return eof;
            }
            objectToMove.transform.localPosition = end;

        }
/*******************************************************************/
  public static IEnumerator MoveToLocal(this GameObject objectToMove, Vector3 end, float seconds, bool die)
        {

            float elapsedTime = 0;
            Vector3 startingPos = objectToMove.transform.localPosition;
            WaitForEndOfFrame eof = new WaitForEndOfFrame();
            while (elapsedTime < seconds)
            {
                Vector3 lerper = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
                objectToMove.transform.localPosition = lerper;

                elapsedTime += Time.deltaTime;
                yield return eof;
            }
            objectToMove.transform.localPosition = end;

             if (die)
                yield return eof;
                Object.Destroy(objectToMove, 0f);
        }
/*******************************************************************/
        public static IEnumerator MoveToLocal(this GameObject objectToMove, Vector3 end, float seconds, float delay)
        {
            yield return new WaitForSeconds(delay);

            float elapsedTime = 0;
            Vector3 startingPos = objectToMove.transform.localPosition;
            WaitForEndOfFrame eof = new WaitForEndOfFrame();

            while (elapsedTime < seconds)
            {
                Vector3 lerper = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
                objectToMove.transform.localPosition = lerper;

                elapsedTime += Time.deltaTime;
                yield return eof;
            }
            objectToMove.transform.localPosition = end;

        }
/*******************************************************************/
        /// <summary>
        /// Moves GameObject then destroy it.
        /// </summary>
        /// <param name="objectToMove">GameObject to move.</param>
        /// <param name="end">End position for GO.</param>
        /// <param name="seconds">Time in secs.</param>
        /// <param name="list">List of Gos.</param>
        /// <example>
        /// <code>
        /// StartCoroutine(player.transform.position.MoveTo(targetPosition, 10f, true));
        /// </code>
        /// </example>
        public static IEnumerator MoveTo(this GameObject objectToMove, Vector3 end, float seconds, bool die)
        {
            float elapsedTime = 0;
            Vector3 startingPos = objectToMove.transform.position;
            WaitForEndOfFrame eof = new WaitForEndOfFrame();

            while (elapsedTime < seconds)
            {
                Vector3 lerper = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
                objectToMove.transform.position = lerper;

                elapsedTime += Time.deltaTime;
                yield return eof;
            }

            objectToMove.transform.position = end;

            if (die)
                yield return eof;
                Object.Destroy(objectToMove, 0f);
        }
/*******************************************************************/
        /// <summary>
        /// Rotate GameObject over seconds.
        /// </summary>
        /// <param name="objectToRotate">GameObject to rotate.</param>
        /// <param name="end">rotate to this Quaternion.</param>
        /// <param name="seconds">time in secs over which to rotate.</param>
        /// <example>
        /// <code>
        /// StartCoroutine(player.transform.rotation.RotateOverSeconds(rotateTo, 2f));
        /// </code>
        /// </example>
        public static IEnumerator RotateTo(this GameObject objectToRotate, Quaternion end, float seconds)
        {
            float elapsedTime = 0;
            Quaternion startingRotation = objectToRotate.transform.rotation;
            WaitForEndOfFrame eof = new WaitForEndOfFrame();

            while (elapsedTime < seconds)
            {
                Quaternion lerper = Quaternion.Lerp(startingRotation, end, (elapsedTime / seconds));
                objectToRotate.transform.rotation = lerper;

                elapsedTime += Time.deltaTime;
                yield return eof;
            }
            objectToRotate.transform.rotation = end;

        }
/*******************************************************************/
        /// <summary>
        /// Rotate GameObject over seconds.
        /// </summary>
        /// <param name="objectToRotate">GameObject to rotate.</param>
        /// <param name="end">rotate to this Quaternion.</param>
        /// <param name="seconds">time in secs over which to rotate.</param>
        public static IEnumerator RotateTo(this Quaternion objectToRotate, Quaternion end, float seconds)
        {
            float elapsedTime = 0;
            Quaternion startingRotation = objectToRotate;
            WaitForEndOfFrame eof = new WaitForEndOfFrame();
            while (elapsedTime < seconds)
            {
                Quaternion lerper = Quaternion.Lerp(startingRotation, end, (elapsedTime / seconds));
                objectToRotate = lerper;

                elapsedTime += Time.deltaTime;
                yield return eof;
            }
            objectToRotate = end;
        }
/*******************************************************************/
        public static IEnumerator RotateTo(this GameObject objectToRotate, Vector3 end, float seconds)
        {
            float elapsedTime = 0;
            Quaternion startingRotation = objectToRotate.transform.rotation;
            Quaternion endRotation = Quaternion.Euler(end);

            WaitForEndOfFrame eof = new WaitForEndOfFrame();
            while (elapsedTime < seconds)
            {

                Quaternion lerper = Quaternion.Lerp(startingRotation, endRotation, (elapsedTime / seconds));
                objectToRotate.transform.rotation = lerper;

                elapsedTime += Time.deltaTime;
                yield return eof;
            }
            objectToRotate.transform.rotation = endRotation;
        }
/*******************************************************************/
        /// <summary>
        /// Rotate LOCAL Quaternion over seconds.
        /// </summary>
        /// <param name="objectToRotate">Quaternion to rotate.</param>
        /// <param name="end">rotate to this LOCAL Quaternion.</param>
        /// <param name="seconds">time in secs over which to rotate.</param>
        public static IEnumerator RotateToLocal(this GameObject objectToRotate, Quaternion end, float seconds)
        {
            float elapsedTime = 0;
            Quaternion startingRotation = objectToRotate.transform.localRotation;
            WaitForEndOfFrame eof = new WaitForEndOfFrame();
            while (elapsedTime < seconds)
            {
                Quaternion lerper = Quaternion.Lerp(startingRotation, end, (elapsedTime / seconds));

                objectToRotate.transform.localRotation = lerper;

                elapsedTime += Time.deltaTime;
                yield return eof;
            }
            objectToRotate.transform.localRotation = end;

        }
/*******************************************************************/
 public static IEnumerator RotateToLocal(this GameObject objectToRotate, Vector3 end, float seconds)
        {
            float elapsedTime = 0;
            Quaternion startingRotation = objectToRotate.transform.localRotation;
            Quaternion endRotation = Quaternion.Euler(end);

            WaitForEndOfFrame eof = new WaitForEndOfFrame();
            while (elapsedTime < seconds)
            {
                Quaternion lerper = Quaternion.Lerp(startingRotation, endRotation, (elapsedTime / seconds));

                objectToRotate.transform.localRotation = lerper;

                elapsedTime += Time.deltaTime;
                yield return eof;
            }

            objectToRotate.transform.localRotation = endRotation;
        }
/*******************************************************************/
        /// <summary>
        /// Rotates the GameObject over seconds; start after initial delay.
        /// </summary>
        /// <param name="objectToRotate">GameObject to rotate.</param>
        /// <param name="end">Final Quaternion.</param>
        /// <param name="seconds">Time in secs for rotation.</param>
        /// <param name="delay">Delay in secs before starting rotation.</param>
        public static IEnumerator RotateTo(this GameObject objectToRotate, Quaternion end, float seconds, float delay)
        {
            float elapsedTime = 0;
            Quaternion startingRotation = objectToRotate.transform.rotation;
            WaitForEndOfFrame eof = new WaitForEndOfFrame();

            yield return new WaitForSeconds(delay);

            while (elapsedTime < seconds)
            {
                Quaternion lerper = Quaternion.Lerp(startingRotation, end, (elapsedTime / seconds));
                objectToRotate.transform.rotation = lerper;

                elapsedTime += Time.deltaTime;
                yield return eof;
            }
            objectToRotate.transform.rotation = end;

        }
/*******************************************************************/
        /// <summary>
        /// Rotates the GameObject over seconds in LOCAL system; start after initial delay.
        /// </summary>
        /// <param name="objectToRotate">GameObject to rotate.</param>
        /// <param name="end">Final Quaternion.</param>
        /// <param name="seconds">Time in secs for rotation.</param>
        /// <param name="delay">Delay in secs before starting rotation.</param>
        public static IEnumerator RotateToLocal(this GameObject objectToRotate, Quaternion end, float seconds, float delay)
        {
            float elapsedTime = 0;
            Quaternion startingRotation = objectToRotate.transform.localRotation;
            WaitForEndOfFrame eof = new WaitForEndOfFrame();

            yield return new WaitForSeconds(delay);

            while (elapsedTime < seconds)
            {
                Quaternion lerper = Quaternion.Lerp(startingRotation, end, (elapsedTime / seconds));
                objectToRotate.transform.localRotation = lerper;

                elapsedTime += Time.deltaTime;
                yield return eof;
            }

            objectToRotate.transform.localRotation = end;
        }
/*******************************************************************/
        /// <summary>
        /// Scales GameObject over seconds.
        /// </summary>
        /// <param name="objectToScale">GameObject to scale.</param>
        /// <param name="end">Final size for GO.</param>
        /// <param name="seconds">Time in secs.</param>
        public static IEnumerator ScaleTo(this GameObject objectToScale, Vector3 end, float seconds)
        {
            float elapsedTime = 0;
            Vector3 start = objectToScale.transform.localScale;
            WaitForEndOfFrame eof = new WaitForEndOfFrame();

            while (elapsedTime < seconds)
            {
                objectToScale.transform.localScale = Vector3.Lerp(start, end, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;

                yield return eof;
            }

            objectToScale.transform.localScale = end;
        }
/*******************************************************************/
        /// <summary>
        /// Scales GameObject over seconds with delay.
        /// </summary>
        /// <param name="objectToScale">GameObject to scale.</param>
        /// <param name="end">Final size for GO.</param>
        /// <param name="seconds">Time in secs.</param>
        /// <param name="delay">Delay before starting scaling.</param>
        public static IEnumerator ScaleTo(this GameObject objectToScale, Vector3 end, float seconds, float delay)
        {
            float elapsedTime = 0;
            Vector3 start = objectToScale.transform.localScale;
            WaitForEndOfFrame eof = new WaitForEndOfFrame();

            yield return new WaitForSeconds(delay);

            while (elapsedTime < seconds)
            {
                objectToScale.transform.localScale = Vector3.Lerp(start, end, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;

                yield return eof;
            }
            objectToScale.transform.localScale = end;

        }
/*******************************************************************/
        public static IEnumerator ScaleTo(this GameObject objectToScale, Vector3 end, float seconds, bool die)
        {
            float elapsedTime = 0;
            Vector3 start = objectToScale.transform.localScale;
            WaitForEndOfFrame eof = new WaitForEndOfFrame();

            while (elapsedTime < seconds)
            {
                objectToScale.transform.localScale = Vector3.Lerp(start, end, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;

                yield return eof;
            }
            objectToScale.transform.localScale = end;

            if (die)
                yield return eof;
                Object.Destroy(objectToScale, 0f);
        }
/*******************************************************************/
        public static IEnumerator LightIntensity(this Light light, float endLight, float seconds)
        {
            float elapsedTime = 0;
            float startingLight = light.intensity;
            WaitForEndOfFrame eof = new WaitForEndOfFrame();

            while (elapsedTime < seconds)
            {
                light.intensity = Mathf.Lerp(startingLight, endLight, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return eof;
            }
            light.intensity = endLight;
        }
/*******************************************************************/
        public static IEnumerator IntensityPulse(this Light light, float endLight, float seconds)
        {
            float elapsedTime = 0;
            float startingLight = light.intensity;

            WaitForEndOfFrame eof = new WaitForEndOfFrame();

            while (elapsedTime < (seconds * 0.5))
            {
                light.intensity = Mathf.Lerp(startingLight, endLight, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return eof;
            }
            light.intensity = endLight;

            while (elapsedTime >= (seconds * 0.5) && elapsedTime < seconds)
            {
                light.intensity = Mathf.Lerp(endLight, startingLight, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return eof;
            }
            light.intensity = startingLight;
        }
/*******************************************************************/
        public static IEnumerator LightRange(this Light light, float endRange, float seconds)
        {
            float elapsedTime = 0;
            float startingRange = light.range;
            WaitForEndOfFrame eof = new WaitForEndOfFrame();

            while (elapsedTime < seconds)
            {
                light.range = Mathf.Lerp(startingRange, endRange, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            light.range = endRange;
        }
/*******************************************************************/
        public static IEnumerator ColourPulse(this Light myLight, Color toColor, float seconds)
        {
            float elapsedTime = 0;
            Color startingColor = myLight.color;
            WaitForEndOfFrame eof = new WaitForEndOfFrame();

            while (elapsedTime < (seconds * 0.5))
            {
                myLight.color = Color.Lerp(startingColor, toColor, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return eof;
            }
            myLight.color = toColor;

            while (elapsedTime >= (seconds * 0.5) && elapsedTime < seconds)
            {
                myLight.color = Color.Lerp(toColor, startingColor, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return eof;
            }
            myLight.color = startingColor;
        }
/*******************************************************************/
        public static IEnumerator ColourPulse(this Material myMat, Color toColor, float seconds)
        {
            float elapsedTime = 0;
            Color startingColor = myMat.color;

            WaitForEndOfFrame eof = new WaitForEndOfFrame();

            while (elapsedTime < (seconds * 0.5))
            {
                myMat.color = Color.Lerp(startingColor, toColor, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return eof;
            }
            myMat.color = toColor;

            while (elapsedTime >= (seconds * 0.5) && elapsedTime < seconds)
            {
                myMat.color = Color.Lerp(toColor, startingColor, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return eof;
            }
            myMat.color = startingColor;
        }
/*******************************************************************/
        public static IEnumerator ColorChange(this Light start, Color end, float seconds)
        {
            float elapsedTime = 0;
            Color startColor = start.color;
            WaitForEndOfFrame eof = new WaitForEndOfFrame();

            while (elapsedTime < seconds)
            {
                start.color = Color.Lerp(startColor, end, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return eof;
            }

            start.color = end;
        }
/*******************************************************************/
#endregion
/*******************************************************************/
/*******************************************************************/
//
//                      Vector Functions
//
/*******************************************************************/
#region
        /// <summary>
        /// Get x and y from Vector3.
        /// </summary>
        /// <returns>new Vector2.</returns>
        /// <param name="v">Vector3</param>
        public static Vector2 xy(this Vector3 v)
        {
            return new Vector2(v.x, v.y);
        }
/*******************************************************************/
        /// <summary>
        /// Get random Vector3 from given min and max.
        /// </summary>
        ///<remark>
        /// uses Unityengine.Random.Range
        ///</remark>
        /// <returns>
        /// random Vector3
        ///</returns>
        /// <param name="v">Vector3</param>
        /// <param name="min">min random value</param>
        /// <param name="max">max random value</param>
        public static Vector3 Random(this Vector3 v, Vector3 min, Vector3 max)
        {
            v = new Vector3(UnityEngine.Random.Range(min.x, max.x), UnityEngine.Random.Range(min.y, max.y), UnityEngine.Random.Range(min.z, max.z));
            return v;
        }
/*******************************************************************/
        /// <summary>
        /// Replace x in Vector3
        /// </summary>
        /// <returns>New Vector3</returns>
        /// <param name="v">Vector3</param>
        /// <param name="x">The x replacement</param>
        public static Vector3 WithX(this Vector3 v, float x)
        {
            return new Vector3(x, v.y, v.z);
        }
/*******************************************************************/
        /// <summary>
        /// Replace y in Vector3
        /// </summary>
        /// <returns>New Vector3.</returns>
        /// <param name="v">Vector3.</param>
        /// <param name="y">The y replacement.</param>
        public static Vector3 WithY(this Vector3 v, float y)
        {
            return new Vector3(v.x, y, v.z);
        }
/*******************************************************************/
        /// <summary>
        /// Replace z in Vector3
        /// </summary>
        /// <returns>New Vector3.</returns>
        /// <param name="v">Vector3.</param>
        /// <param name="z">The z replacement.</param>
        public static Vector3 WithZ(this Vector3 v, float z)
        {
            return new Vector3(v.x, v.y, z);
        }
/*******************************************************************/
        /// <summary>
        /// Adds the x to Vector3.x
        /// </summary>
        /// <returns>New Vector3.</returns>
        /// <param name="v">V.</param>
        /// <param name="x">The value to add to x.</param>
        public static Vector3 AddX(this Vector3 v, float x)
        {
            return new Vector3(v.x + x, v.y, v.z);
        }
/*******************************************************************/
        /// <summary>
        /// Adds the y to Vector3.y
        /// </summary>
        /// <returns>New Vector3.</returns>
        /// <param name="v">V.</param>
        /// <param name="y">The value to add to y.</param>
        public static Vector3 AddY(this Vector3 v, float y)
        {
            return new Vector3(v.x, v.y + y, v.z);
        }
/*******************************************************************/
        /// <summary>
        /// Adds the z to Vector3.z
        /// </summary>
        /// <returns>New Vector3.</returns>
        /// <param name="v">V.</param>
        /// <param name="z">The value to add to z.</param>
        public static Vector3 AddZ(this Vector3 v, float z)
        {
            return new Vector3(v.x, v.y, v.z + z);
        }
/*******************************************************************/
        /// <summary>
        /// Multiply Vector3.x by x
        /// </summary>
        /// <returns>new Vector3</returns>
        /// <param name="v">V.</param>
        /// <param name="x">The value to multiply to x.</param>
        public static Vector3 MultX(this Vector3 v, float x)
        {
            return new Vector3(v.x * x, v.y, v.z);
        }
/*******************************************************************/
        /// <summary>
        /// Multiply Vector3.y by y
        /// </summary>
        /// <returns>New Vector3.</returns>
        /// <param name="v">V.</param>
        /// <param name="y">The value to multiply to y.</param>
        public static Vector3 MultY(this Vector3 v, float y)
        {
            return new Vector3(v.x , v.y * y, v.z);
        }
/*******************************************************************/
        /// <summary>
        /// Multiply Vector3.z by z
        /// </summary>
        /// <returns>New Vector3.</returns>
        /// <param name="v">V.</param>
        /// <param name="z">The value to multiply to z .</param>
        public static Vector3 MultZ(this Vector3 v, float z)
        {
            return new Vector3(v.x, v.y, v.z * z);
        }
/*******************************************************************/
        /// <summary>
        /// Divide Vector3.x by x
        /// </summary>
        /// <returns>new Vector3</returns>
        /// <param name="v">Vector3.</param>
        /// <param name="x">The value to divide Vector3.x by.</param>
        public static Vector3 DivX(this Vector3 v, float x)
        {
            try {

                return new Vector3(v.x / x, v.y, v.z);
            }
            catch (System.DivideByZeroException) {
                Debug.LogError($"Division of {v.x} by zero.");
            }

            return Vector3.zero;
        }
/*******************************************************************/
        /// <summary>
        /// Divide Vector3.y by y
        /// </summary>
        /// <returns>New Vector3.</returns>
        /// <param name="v">V.</param>
        /// <param name="y">The value to divide Vector3.y by.</param>
        public static Vector3 DivY(this Vector3 v, float y)
        {
             try {

                return new Vector3(v.x, v.y / y, v.z);
            }
            catch (System.DivideByZeroException) {
                Debug.LogError($"Division of {v.y} by zero.");
            }

            return Vector3.zero;
        }
/*******************************************************************/
        /// <summary>
        /// Divide Vector3.z by z
        /// </summary>
        /// <returns>New Vector3.</returns>
        /// <param name="v">V.</param>
        /// <param name="z">The value to divide Vector3.y by.</param>
        public static Vector3 DivZ(this Vector3 v, float z)
        {
            try {

                return new Vector3(v.x, v.y, v.z / z);
            }
            catch (System.DivideByZeroException) {
                Debug.LogError($"Division of {v.z} by zero.");
            }

            return Vector3.zero;
        }
/*******************************************************************/
        /// <summary>
        /// Center of Volume described by a Vector3.
        /// </summary>
        /// <returns>The center of Vector3 .</returns>
        /// <param name="v">Vector3</param>
        public static Vector3 Center(this Vector3 v)
        {
            return new Vector3(v.x + v.x * 0.5f, v.y + v.y * 0.5f, v.z + v.z * 0.5f);
        }
/*******************************************************************/
    public static Vector3 Flattened(this Vector3 vector)
	{
		return new Vector3(vector.x, 0f, vector.z);
	}
/*******************************************************************/
	public static float DistanceFlat(this Vector3 origin, Vector3 destination)
	{
		return Vector3.Distance(origin.Flattened(), destination.Flattened());
	}
/*******************************************************************/
#endregion
/*******************************************************************/
//
//                      List Functions
//
/*******************************************************************/
#region
        /// <summary>
        /// Initialize a List<T> with a default value
        /// </summary>
        /// <param name="lst">List to fill.</param>
        /// <param name="val">Value that will be added to list.</param>
        /// <example>
        /// <code>
        /// List<int> x  = new List(10); x.Fill();
        /// List<int> x  = new List(10); x.Fill(44);
        /// List<float> x  = new List(10); x.Fill(74.47);
        /// List<GameObject> x  = new List(10); x.Fill(BulletPrefab);
        /// </code>
        /// </example>
        public static void Fill<T>(this List<T> lst, T val)
        {
            // lst = Enumerable.Repeat(val, lst.Capacity).ToList();
            for (int i = 0; i < lst.Capacity; i++)
            {
                lst.Add(val);
            }
        }
/*******************************************************************/
        /// <remarks>
        /// default(T) will return 0 for numeric types and null for reference types!
        /// </remarks>
        public static void Fill<T>(this List<T> lst)
        {
            Fill(lst, default(T));
        }
/*******************************************************************/
    /// <summary>
    /// Return a random item from the list.
    /// Sampling with replacement.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T RandomItem<T>(this IList<T> list)
    {
        if (list.Count == 0) throw new System.IndexOutOfRangeException("empty list");
        return list[UnityEngine.Random.Range(0, list.Count)];
    }
/*******************************************************************/
#endregion
/*******************************************************************/
//
//
//
/*******************************************************************/
        public static void ColorChange(this Light myLight, Color toColor)
        {
            myLight.color = toColor;
        }
/*******************************************************************/
        /// <summary>
        /// Change alpha of this colour
        /// </summary>
        /// <returns>new Color</returns>
        /// <param name="c">Color</param>
        /// <param name="a">The new alpha component.</param>
        public static Color WithAlpha(this Color c, float a)
        {
            return new Color(c.r, c.g, c.b, a);
        }
/*******************************************************************/
        public static void ClearAndDestroyAllGos(this List<GameObject> gos)
        {
            foreach (GameObject go in gos)
            {
                if (Application.isEditor)
                {
                    Object.DestroyImmediate(go);
                }
                else
                {
                    Object.Destroy(go, 0f);
                }
            }
            gos.Clear();
        }
/*******************************************************************/
        public static float scale(this float oldValue, float oldMin, float oldMax, float newMin, float newMax)
        {
            float oldRange = oldMax - oldMin;
            float newRange = newMax - newMin;
            float newValue = (((oldValue - oldMin) * newRange) / oldRange) + newMin;

            return newValue;
        }
/*******************************************************************/
        public static float scaleClamp(this float oldValue, float oldMin, float oldMax, float newMin, float newMax)
        {
            float oldRange = oldMax - oldMin;
            float newRange = newMax - newMin;
            float newValue = (((oldValue - oldMin) * newRange) / oldRange) + newMin;

            return Mathf.Clamp(newValue, newMin, newMax);
        }
/*******************************************************************/
        /// <summary>
        /// Looks at a given target.
        /// </summary>
        /// <param name="self">Transform from which to look.</param>
        /// <param name="target">Target to look at.</param>
        public static void LookAt(this Transform self, GameObject target)
        {
            self.LookAt(target.transform);
        }
/*******************************************************************/
        /// <summary>
        /// Looks at a given target.
        /// </summary>
        /// <param name="self">Transform from which to look.</param>
        /// <param name="target">Target to look at.</param>
        public static void LookAt(this Transform self, Transform target)
        {
            self.LookAt(target);
        }
/*******************************************************************/
        /// <summary>
        /// Looks at a given target.
        /// </summary>
        /// <param name="self">Transform from which to look.</param>
        /// <param name="target">Target to look at.</param>
        public static void LookAt(this GameObject self, GameObject target)
        {
            self.transform.LookAt(target);
        }
/*******************************************************************/
        /// <summary>
        /// Rotation of this object to target
        /// </summary>
        /// <returns>Quaternion needed to look at target.</returns>
        /// <param name="self">Starting Transform.</param>
        /// <param name="target">Target Position (Vector3).</param>
        public static Quaternion GetLookAtRotation(this Transform self, Vector3 target)
        {
            return Quaternion.LookRotation(target - self.position);
        }
/*******************************************************************/
        /// <summary>
        /// Rotation of this object to target
        /// </summary>
        /// <returns>Quaternion needed to look at target.</returns>
        /// <param name="self">Starting Transform.</param>
        /// <param name="target">Target Position (Transform).</param>
        public static Quaternion GetLookAtRotation(this Transform self, Transform target)
        {
            return GetLookAtRotation(self, target.position);
        }
/*******************************************************************/
    }
}