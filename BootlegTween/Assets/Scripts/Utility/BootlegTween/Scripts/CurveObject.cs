using UnityEngine;
using System.Collections;

[System.Serializable]
[CreateAssetMenu(fileName = "Assets/Resources/Utility/BootlegTween/TweenShapes/NewSystemCurve", menuName = "BootlegTween/NewMotionCurve", order = 1)]
public class CurveObject : ScriptableObject
{
    public string objectName = "NewMotionCurve";
    public AnimationCurve curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
}
