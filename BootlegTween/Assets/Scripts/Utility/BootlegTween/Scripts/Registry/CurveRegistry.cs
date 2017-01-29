using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Util
{
    public class CurveRegistry
    {
        public static Dictionary<string, CurveObject> curveReg;

        /// <summary>
        /// Loads all Curves from the resources folder
        /// </summary>
        public static void LoadAllCurves()
        {
            //Check if the dictionary is null. That means it is the first run
            if (curveReg == null)
            {
                //Initialise it as not null
                curveReg = new Dictionary<string, CurveObject>();
                //Creates a string holding the file location
                string fileLocation = "Utility/BootlegTween/TweenShapes";
                //Load all curves to an array to be iterated through
                object[] curves = Resources.LoadAll(fileLocation, typeof(CurveObject));
                if (curves != null)
                {
                    for (int i = 0; i < curves.Length; i++)
                    {
                        CurveObject temp = (CurveObject)curves[i];
                        //add each curve to the dictionary
                        curveReg.Add(temp.name.ToLower(), temp);
                    }
                }
                else
                {
                    Debug.LogError("Could not load/find any items in ''Assets/Resources/Utility/BootlegTween/TweenShapes''");
                }
            }
        }

        /// <summary>
        /// Retrieves the needed curve and returns the specified curve if it exists
        /// </summary>
        /// <returns></returns>
        public static CurveObject GetCurve(string identifier)
        {
            LoadAllCurves();
            identifier = identifier.ToLower();
            //See if the curve exists
            if (curveReg.ContainsKey(identifier))
            {
                return curveReg[identifier];
            }
            //If nothing exists, return nothing
            Debug.LogError("Could not find item " + identifier + " in curveRegistry");
            return null;
        }
    }
}