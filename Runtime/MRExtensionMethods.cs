using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// https://docs.microsoft.com/en-us/dotnet/csharp/codedoc
// https://www.youtube.com/watch?v=mr5xkf6zSzk

//TODO:
//  organize code
//  document
//  remove useless stuff
//  figure out consistency


namespace MR.CustomExtensions
{
    public static class MRExtensionMethods
    {
        /// <summary>
        /// Removes the component.
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

        public static IEnumerator EmissionIntensityOverSeconds(this GameObject go, float intensity, float seconds)  {

            float elapsedTime = 0;

            Material material = go.GetComponent<Renderer>().material;
            Color colour = material.GetColor("_EmissionColor");

            while (elapsedTime < seconds)
            {
                float lerper = Mathf.Lerp(1, intensity, (elapsedTime / seconds));
                Color newC = colour * lerper;

                material.SetColor("_EmissionColor", newC);

                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            material.SetColor("_EmissionColor", colour * intensity);
        }

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
        public static IEnumerator MoveOverSeconds(this GameObject objectToMove, Vector3 end, float seconds)
        {

            float elapsedTime = 0;
            Vector3 startingPos = objectToMove.transform.position;

            while (elapsedTime < seconds)
            {
                Vector3 lerper = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
                objectToMove.transform.position = lerper;

                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            objectToMove.transform.position = end;

        }

        public static IEnumerator MoveOverSeconds(this GameObject objectToMove, Vector3 end, float seconds, float delay)
        {

            float elapsedTime = 0;
            Vector3 startingPos = objectToMove.transform.position;

            yield return new WaitForSeconds(delay);

            while (elapsedTime < seconds)
            {
                Vector3 lerper = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
                objectToMove.transform.position = lerper;


                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            objectToMove.transform.position = end;
        }

        /// <summary>
        /// Moves/Interpolates Vector3 to new value over seconds.
        /// </summary>
        /// <param name="vectorToMove">Vector3 to change.</param>
        /// <param name="end">Vector3 to move to.</param>
        /// <param name="seconds">time in secs to move to new Vector3.</param>
        /// <example>
        /// <code>
        /// StartCoroutine(currentPosition.MoveOverSeconds(targetPosition, 10f));
        /// </code>
        /// </example>
        public static IEnumerator MoveOverSeconds(this Vector3 vectorToMove, Vector3 end, float seconds, float delay)
        {

            float elapsedTime = 0;
            Vector3 startingPos = vectorToMove;

            yield return new WaitForSeconds(delay);

            while (elapsedTime < seconds)
            {
                Vector3 lerper = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
                vectorToMove = lerper;

                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            vectorToMove = end;
        }
            public static IEnumerator MoveOverSeconds(this Vector3 vectorToMove, Vector3 end, float seconds)
        {

            float elapsedTime = 0;
            Vector3 startingPos = vectorToMove;

            while (elapsedTime < seconds)
            {
                Vector3 lerper = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
                vectorToMove = lerper;

                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            vectorToMove = end;

        }

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
        public static IEnumerator MoveOverSecondsLocal(this GameObject objectToMove, Vector3 end, float seconds)
        {

            float elapsedTime = 0;
            Vector3 startingPos = objectToMove.transform.localPosition;

            while (elapsedTime < seconds)
            {
                Vector3 lerper = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
                objectToMove.transform.localPosition = lerper;

                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            objectToMove.transform.localPosition = end;

        }

        public static IEnumerator MoveOverSecondsLocal(this GameObject objectToMove, Vector3 end, float seconds, float delay)
        {
            yield return new WaitForSeconds(delay);

            float elapsedTime = 0;
            Vector3 startingPos = objectToMove.transform.localPosition;

            while (elapsedTime < seconds)
            {
                Vector3 lerper = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
                objectToMove.transform.localPosition = lerper;
                Debug.Log($"lerper: {lerper}");

                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            objectToMove.transform.localPosition = end;

        }
        /// <summary>
        /// Moves GameObject then destroy it.
        /// </summary>
        /// <param name="objectToMove">GameObject to move.</param>
        /// <param name="end">End position for GO.</param>
        /// <param name="seconds">Time in secs.</param>
        /// <param name="list">List of Gos.</param>
        /// <see cref="IsAliveInList(GameObject, bool, float, List{GameObject})"/>
        /// <example>
        /// <code>
        /// StartCoroutine(player.transform.position.MoveToThenDie(targetPosition, 10f, ListOfGos));
        /// </code>
        /// </example>
        public static IEnumerator MoveToThenDie(this GameObject objectToMove, Vector3 end, float seconds, List<GameObject> list)
        {
            float elapsedTime = 0;
            Vector3 startingPos = objectToMove.transform.position;

            while (elapsedTime < seconds)
            {
                Vector3 lerper = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
                objectToMove.transform.position = lerper;

                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            objectToMove.transform.position = end;

            objectToMove.IsAliveInList(false, 1f, list);
        }

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
        public static IEnumerator RotateOverSeconds(this GameObject objectToRotate, Quaternion end, float seconds)
        {
            float elapsedTime = 0;
            Quaternion startingRotation = objectToRotate.transform.rotation;

            while (elapsedTime < seconds)
            {
                Quaternion lerper = Quaternion.Lerp(startingRotation, end, (elapsedTime / seconds));
                objectToRotate.transform.rotation = lerper;

                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            objectToRotate.transform.rotation = end;

        }

        /// <summary>
        /// Rotate GameObject over seconds.
        /// </summary>
        /// <param name="objectToRotate">GameObject to rotate.</param>
        /// <param name="end">rotate to this Quaternion.</param>
        /// <param name="seconds">time in secs over which to rotate.</param>
        public static IEnumerator RotateOverSeconds(this Quaternion objectToRotate, Quaternion end, float seconds)
        {
            float elapsedTime = 0;
            Quaternion startingRotation = objectToRotate;

            while (elapsedTime < seconds)
            {
                Quaternion lerper = Quaternion.Lerp(startingRotation, end, (elapsedTime / seconds));
                objectToRotate = lerper;

                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            objectToRotate = end;
        }

        /// <summary>
        /// Rotate LOCAL Quaternion over seconds.
        /// </summary>
        /// <param name="objectToRotate">Quaternion to rotate.</param>
        /// <param name="end">rotate to this LOCAL Quaternion.</param>
        /// <param name="seconds">time in secs over which to rotate.</param>
        public static IEnumerator RotateOverSecondsLocal(this GameObject objectToRotate, Quaternion end, float seconds)
        {
            float elapsedTime = 0;
            Quaternion startingRotation = objectToRotate.transform.localRotation;

            while (elapsedTime < seconds)
            {
                Quaternion lerper = Quaternion.Lerp(startingRotation, end, (elapsedTime / seconds));

                objectToRotate.transform.localRotation = lerper;

                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            objectToRotate.transform.localRotation = end;

        }

        /// <summary>
        /// Rotates the GameObject over seconds; start after initial delay.
        /// </summary>
        /// <param name="objectToRotate">GameObject to rotate.</param>
        /// <param name="end">Final Quaternion.</param>
        /// <param name="seconds">Time in secs for rotation.</param>
        /// <param name="delay">Delay in secs before starting rotation.</param>
        public static IEnumerator RotateOverSecondsWithDelay(this GameObject objectToRotate, Quaternion end, float seconds, float delay)
        {
            float elapsedTime = 0;
            Quaternion startingRotation = objectToRotate.transform.rotation;

            yield return new WaitForSeconds(delay);

            while (elapsedTime < seconds)
            {
                Quaternion lerper = Quaternion.Lerp(startingRotation, end, (elapsedTime / seconds));
                objectToRotate.transform.rotation = lerper;

                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            objectToRotate.transform.rotation = end;

        }

        /// <summary>
        /// Rotates the GameObject over seconds in LOCAL system; start after initial delay.
        /// </summary>
        /// <param name="objectToRotate">GameObject to rotate.</param>
        /// <param name="end">Final Quaternion.</param>
        /// <param name="seconds">Time in secs for rotation.</param>
        /// <param name="delay">Delay in secs before starting rotation.</param>
        public static IEnumerator RotateOverSecondsWithDelayLocal(this GameObject objectToRotate, Quaternion end, float seconds, float delay)
        {
            float elapsedTime = 0;
            Quaternion startingRotation = objectToRotate.transform.localRotation;

            yield return new WaitForSeconds(delay);

            while (elapsedTime < seconds)
            {
                Quaternion lerper = Quaternion.Lerp(startingRotation, end, (elapsedTime / seconds));
                objectToRotate.transform.localRotation = lerper;

                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            objectToRotate.transform.localRotation = end;

        }

        /// <summary>
        /// Scales GameObject over seconds.
        /// </summary>
        /// <param name="objectToScale">GameObject to scale.</param>
        /// <param name="end">Final size for GO.</param>
        /// <param name="seconds">Time in secs.</param>
        public static IEnumerator ScaleOverSeconds(this GameObject objectToScale, Vector3 end, float seconds)
        {
            float elapsedTime = 0;
            Vector3 start = objectToScale.transform.localScale;

            while (elapsedTime < seconds)
            {
                objectToScale.transform.localScale = Vector3.Lerp(start, end, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }
            objectToScale.transform.localScale = end;
        }

        /// <summary>
        /// Scales GameObject over seconds with delay.
        /// </summary>
        /// <param name="objectToScale">GameObject to scale.</param>
        /// <param name="end">Final size for GO.</param>
        /// <param name="seconds">Time in secs.</param>
        /// <param name="delay">Delay before starting scaling.</param>
        public static IEnumerator ScaleOverSecondsWithDelay(this GameObject objectToScale, Vector3 end, float seconds, float delay)
        {
            float elapsedTime = 0;
            Vector3 start = objectToScale.transform.localScale;

            yield return new WaitForSeconds(delay);

            while (elapsedTime < seconds)
            {
                objectToScale.transform.localScale = Vector3.Lerp(start, end, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }
            objectToScale.transform.localScale = end;

        }


        public static IEnumerator ScaleOverSecondsThenDie(this GameObject objectToScale, Vector3 end, float seconds)
        {
            float elapsedTime = 0;
            Vector3 start = objectToScale.transform.localScale;

            while (elapsedTime < seconds)
            {
                objectToScale.transform.localScale = Vector3.Lerp(start, end, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }
            objectToScale.transform.localScale = end;
            objectToScale.IsAlive(false);
        }

        public static void IsAlive(this GameObject obj, bool isAlive)
        {
            if (!isAlive)
            {
                Object.Destroy(obj, 0f);
            }
        }

        public static void IsAliveInList(this GameObject obj, bool isAlive, float timeToDeath, List<GameObject> list)
        {
            if (!isAlive)
            {
                if (list.Contains(obj))
                {

                    list.Remove(obj);
                }

                Object.Destroy(obj, timeToDeath);
            }
        }

        public static IEnumerator LightIntensity(this Light light, float endLight, float seconds)
        {
            float elapsedTime = 0;
            float startingLight = light.intensity;

            while (elapsedTime < seconds)
            {
                light.intensity = Mathf.Lerp(startingLight, endLight, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            light.intensity = endLight;
        }

        public static IEnumerator IntensityPulse(this Light light, float endLight, float seconds)
        {
            float elapsedTime = 0;
            float startingLight = light.intensity;

            while (elapsedTime < (seconds * 0.5))
            {
                light.intensity = Mathf.Lerp(startingLight, endLight, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            light.intensity = endLight;

            while (elapsedTime >= (seconds * 0.5) && elapsedTime < seconds)
            {
                light.intensity = Mathf.Lerp(endLight, startingLight, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            light.intensity = startingLight;
        }

        public static IEnumerator LightRange(this Light light, float endRange, float seconds)
        {
            float elapsedTime = 0;
            float startingRange = light.range;

            while (elapsedTime < seconds)
            {
                light.range = Mathf.Lerp(startingRange, endRange, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            light.range = endRange;
        }

        public static IEnumerator ColourPulse(this Light myLight, Color toColor, float seconds)
        {
            float elapsedTime = 0;
            Color startingColor = myLight.color;

            while (elapsedTime < (seconds * 0.5))
            {
                myLight.color = Color.Lerp(startingColor, toColor, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            myLight.color = toColor;

            while (elapsedTime >= (seconds * 0.5) && elapsedTime < seconds)
            {
                myLight.color = Color.Lerp(toColor, startingColor, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            myLight.color = startingColor;
        }

        public static IEnumerator ColourChange(this Light myLight, Color toColor, float seconds)
        {
            float elapsedTime = 0;
            Color startingColor = myLight.color;

            WaitForEndOfFrame eof = new WaitForEndOfFrame();

            while (elapsedTime < seconds)
            {
                myLight.color = Color.Lerp(startingColor, toColor, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return eof;
            }

            myLight.color = toColor;
        }

        public static void ColourChange(this Light myLight, Color toColor)
        {
            myLight.color = toColor;
        }


        public static IEnumerator MaterialColourPulse(this Material myMat, Color toColor, float seconds)
        {
            float elapsedTime = 0;
            Color startingColor = myMat.color;

            while (elapsedTime < (seconds * 0.5))
            {
                myMat.color = Color.Lerp(startingColor, toColor, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            myMat.color = toColor;

            while (elapsedTime >= (seconds * 0.5) && elapsedTime < seconds)
            {
                myMat.color = Color.Lerp(toColor, startingColor, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            myMat.color = startingColor;
        }
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

        /// <summary>
        /// Get x and y from Vector3.
        /// </summary>
        /// <returns>new Vector2.</returns>
        /// <param name="v">Vector3</param>

        public static Vector2 xy(this Vector3 v)
        {
            return new Vector2(v.x, v.y);
        }

        /// <summary>
        /// Get random Vector3 from given min and max.
        /// </summary>
        /// <returns></returns>
        /// <param name="v">Vector3</param>
        public static Vector3 Random(this Vector3 v, Vector3 min, Vector3 max)
        {
            v = new Vector3(UnityEngine.Random.Range(min.x, max.x), UnityEngine.Random.Range(min.y, max.y), UnityEngine.Random.Range(min.z, max.z));
            return v;
        }

        /// <summary>
        /// Replace x in Vector3
        /// </summary>
        /// <returns>New Vector3.</returns>
        /// <param name="v">Vector3.</param>
        /// <param name="x">The x replacement.</param>
        public static Vector3 WithX(this Vector3 v, float x)
        {
            return new Vector3(x, v.y, v.z);
        }

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

        /// <summary>
        /// Multiply Vector3.x by x
        /// </summary>
        /// <returns>new Vector3</returns>
        /// <param name="v">V.</param>
        /// <param name="x">The value to multiply to x.</param>
        public static Vector3 Mult(this Vector3 v, float x)
        {
            return new Vector3(v.x * x, v.y * x, v.z * x);
        }

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

        /// <summary>
        /// Center of Volume described by a Vector3.
        /// </summary>
        /// <returns>The center (Vector3).</returns>
        /// <param name="v">Vector3</param>
        public static Vector3 Center(this Vector3 v)
        {
            return new Vector3(v.x + v.x * 0.5f, v.y + v.y * 0.5f, v.z + v.z * 0.5f);
        }

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

        public static Color WithGFraction(this Color c, float f)
        {
            return new Color(c.r, c.g * f, c.b, c.a);
        }

        public static float scale(this float oldValue, float oldMin, float oldMax, float newMin, float newMax)
        {
            float oldRange = oldMax - oldMin;
            float newRange = newMax - newMin;
            float newValue = (((oldValue - oldMin) * newRange) / oldRange) + newMin;

            return newValue;
        }

        public static float scaleClamp(this float oldValue, float oldMin, float oldMax, float newMin, float newMax)
        {
            float oldRange = oldMax - oldMin;
            float newRange = newMax - newMin;
            float newValue = (((oldValue - oldMin) * newRange) / oldRange) + newMin;

            return Mathf.Clamp(newValue, newMin, newMax);
        }

        // public static void Reset(this int[] values)
        // {
        //     for (int i = 0; i < values.Length; i++)
        //     {
        //         values[i] = 0;
        //     }
        // }

        //  public static void Reset(this float[] values)
        // {
        //     for (int i = 0; i < values.Length; i++)
        //     {
        //         values[i] = 0f;
        //     }
        // }

        // public static void Reset(this List<int> values)
        // {
        //     for (int i = 0; i < values.Count; i++)
        //     {
        //         values[i] = 0;
        //     }
        // }

        //     public static void Reset(this List<float> values)
        // {
        //     for (int i = 0; i < values.Count; i++)
        //     {
        //         values[i] = 0f;
        //     }
        // }

        /// <summary>
        /// Fill List with zeros.
        /// </summary>
        /// <param name="values">List to fill.</param>
        /// <param name="length">Number of 0s to add.</param>
        public static void FillZeros(this List<int> values, int length)
        {
            //isn't there a "addRange"
            for (int i = 0; i < length; i++)
            {
                values.Add(0);
            }
        }

        /// <summary>
        /// Looks at a given target.
        /// </summary>
        /// <param name="self">Transform from which to look.</param>
        /// <param name="target">Target to look at.</param>
        public static void LookAt(this Transform self, GameObject target)
        {
            self.LookAt(target.transform);
        }

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
    }
}