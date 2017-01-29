using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(CurveObject))]
public class CurveObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CurveObject myTarget = (CurveObject)target;

        myTarget.objectName = myTarget.name;
        EditorGUILayout.LabelField("Name", myTarget.objectName);
        myTarget.curve = EditorGUILayout.CurveField("Tween Curve", myTarget.curve);
        ValidateCurve();
    }

    void ValidateCurve()
    {
        CurveObject myTarget = (CurveObject)target;
        bool zerozero = false;
        bool oneone = false;
        for (int i = 0; i < myTarget.curve.keys.Length; i++)
        {
            if (myTarget.curve.keys[i].time == 0)
            {
                myTarget.curve.keys[i].value = 0;
                zerozero = true;
            }
            else if (myTarget.curve.keys[i].time == 1)
            {
                myTarget.curve.keys[i].value = 1;
                oneone = true;
            }
        }
        if (!zerozero)
        {
            myTarget.curve.AddKey(0, 0);
        }
        if (!oneone)
        {
            myTarget.curve.AddKey(1, 1);
        }
    }
}
