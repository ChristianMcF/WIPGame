using UnityEngine;
using System.Collections;

namespace Util
{

    public class BootlegTween
    {
        public enum ActionType { Colour, Position, Scale }
        public enum MotionCurve { Linear, EaseIn, EaseOut, SmoothStep, SmootherStep, EaseOutElastic, Quadratic }
        public enum AnimType { Scale }


        public static void ScaleUI(GameObject tweenObject, Vector3 endValue, float animationTime, string curve, string endCommand = "")
        {
            GameObject.Destroy(tweenObject.GetComponent<UIScaler>());
            CurveObject endCurve = CurveRegistry.GetCurve(curve);
            tweenObject.AddComponent<UIScaler>().SetAnimation(endValue, animationTime, endCurve, endCommand);
        }

        public static void TweenTransform(GameObject tweenObject, Vector3 endValue, float animationTime, string curve, string endCommand = "")
        {
            GameObject.Destroy(tweenObject.GetComponent<TransformTween>());
            CurveObject endCurve = CurveRegistry.GetCurve(curve);
            tweenObject.AddComponent<TransformTween>().SetAnimation(endValue, animationTime, endCurve, endCommand);
        }
        public static void CameraScale(GameObject tweenObject, float endValue, float animationTime, string curve, string endCommand = "")
        {
            GameObject.Destroy(tweenObject.GetComponent<CameraScaleTweener>());
            CurveObject endCurve = CurveRegistry.GetCurve(curve);
            tweenObject.AddComponent<CameraScaleTweener>().SetAnimation(endValue, animationTime, endCurve, endCommand);
        }
        public static void TextTween()
        {

        }

        public static void UICoordTween(RectTransform parentCanvas, GameObject tweenObject, Vector2 endValue, float animationTime, string curve)
        {
            GameObject.Destroy(tweenObject.GetComponent<UICoordTween>());
            CurveObject endCurve = CurveRegistry.GetCurve(curve);
            tweenObject.AddComponent<UICoordTween>().SetAnimation(parentCanvas, endValue, animationTime, endCurve);
        }

        public static void CanvasGroupTween(GameObject tweenObject, float endValue, float animationTime, string curve, string endCommand = "")
        {
            GameObject.Destroy(tweenObject.GetComponent<CanvasGroupTween>());
            CurveObject endCurve = CurveRegistry.GetCurve(curve);
            tweenObject.AddComponent<CanvasGroupTween>().SetAnimation(endValue, animationTime, endCurve, endCommand);
        }

        public static void UIPositionTween(RectTransform parentCanvas, GameObject tweenObject, Vector2 endValue, float animationTime, string curve, string endCommand = "")
        {
            GameObject.Destroy(tweenObject.GetComponent<UITransformTween>());
            CurveObject endCurve = CurveRegistry.GetCurve(curve);
            tweenObject.AddComponent<UITransformTween>().SetAnimation(parentCanvas, endValue, animationTime, endCurve, endCommand);
        }
    }
}