using Util;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BaseTween : MonoBehaviour
{
    #region Variables
    protected CurveObject selectedCurve;
    protected float endValueFloat;
    protected Vector3 endValueVector3;
    protected Vector2 endValueVector2;
    protected float curLerpTime;
    protected float timeOfAnim;
    protected float t = 0;
    protected string endCommand;
    protected BootlegTween.MotionCurve deprecatedCurve;
    #endregion

    #region Initialisers

    #region SetAnimationOverrides

    /// <summary>
    /// Practically this is a constructor for the animation.
    /// </summary>
    /// <param name="endValue">The final Vector for the animation to play</param>
    /// <param name="timeOfAnim">The amount of time in seconds to play this animation for</param>
    /// <param name="curve">The motion curve to follow</param>
    public void SetAnimation(float endValue, float timeOfAnim, CurveObject curve, string endCommand = "")
    {
        this.endValueFloat = endValue;
        this.timeOfAnim = timeOfAnim;
        this.selectedCurve = curve;
        this.endCommand = endCommand;
        Begin();
    }

    /// <summary>
    /// Practically this is a constructor for the animation.
    /// </summary>
    /// <param name="endValue">The final Vector for the animation to play</param>
    /// <param name="timeOfAnim">The amount of time in seconds to play this animation for</param>
    /// <param name="curve">The motion curve to follow</param>
    public void SetAnimation(Vector3 endValue, float timeOfAnim, CurveObject curve, string endCommand = "")
    {
        this.endValueVector3 = endValue;
        this.timeOfAnim = timeOfAnim;
        this.selectedCurve = curve;
        this.endCommand = endCommand;
        Begin();
    }

    /// <summary>
    /// Practically this is a constructor for the animation.
    /// </summary>
    /// <param name="endValue">The final Vector for the animation to play</param>
    /// <param name="timeOfAnim">The amount of time in seconds to play this animation for</param>
    /// <param name="curve">The motion curve to follow</param>
    public void SetAnimation(Vector2 endValue, float timeOfAnim, CurveObject curve, string endCommand = "")
    {
        this.endValueVector2 = endValue;
        this.timeOfAnim = timeOfAnim;
        this.selectedCurve = curve;
        this.endCommand = endCommand;
        Begin();
    }

    #endregion

    protected virtual void Begin()
    {

    }

    #endregion

    #region Enders

    protected void OnEnd()
    {
        switch (endCommand)
        {
            case "DisableObject":
                DisableObject();
                break;
            case "DisableCanvas":
                DisableCanvas();
                break;
            default:
                break;
        }
    }

    protected virtual void DisableObject()
    {
        gameObject.SetActive(false);
    }

    protected virtual void DisableCanvas()
    {
    }

    #endregion

    #region Utility Methods

    #region Curve Methods
    protected float timeCurve(float time, CurveObject motionCurve)
    {
        time = curLerpTime / timeOfAnim;
        return motionCurve.curve.Evaluate(time);
    }

    /// <summary>
    /// Method which determines the curve
    /// </summary>
    /// <param name="time">The current time of the anim</param>
    /// <returns></returns>
    protected float timeCurve(float time)
    {
        //Math stuff
        time = curLerpTime / timeOfAnim;
        //Depending on the curve type
        switch (deprecatedCurve)
        {
            case BootlegTween.MotionCurve.Linear:
                return time;
            case BootlegTween.MotionCurve.EaseIn:
                return 1f - Mathf.Cos(time * Mathf.PI * 0.5f);
            case BootlegTween.MotionCurve.EaseOut:
                return Mathf.Sin(time * Mathf.PI * 0.5f);
            case BootlegTween.MotionCurve.SmoothStep:
                return time * time * (3f - 2f * time);
            case BootlegTween.MotionCurve.SmootherStep:
                return time * time * time * (time * (6f * time - 15f) + 10f);
            case BootlegTween.MotionCurve.Quadratic:
                if (time < 0.5f)
                {
                    return (time * time * (3 - 4 * time)) * 2;
                }
                else
                {
                    return time * time * (3f - 2f * time);
                }
            default:
                return time += Time.deltaTime / timeOfAnim;
        }
    }

    protected static float EaseOutElastic(float t, float b, float c, float d)
    {
        if (t == 0) return b;
        if ((t /= d) == 1) return b + c;
        float p = d * .3f;
        float s = 0;
        float a = 0;
        if (a == 0f || a < Mathf.Abs(c))
        {
            a = c;
            s = p / 4;
        }
        else
        {
            s = p / (2 * Mathf.PI) * Mathf.Asin(c / a);
        }
        return (a * Mathf.Pow(2, -10 * t) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p) + c + b);
    }

    #endregion

    #region ReturnTween

    protected Vector3 ReturnTween(float _curLerpTime, Vector3 _startValue, Vector3 _endValue, float _timeOfAnim)
    {
        float x = EaseOutElastic(_curLerpTime, _startValue.x, _endValue.x, _timeOfAnim);
        float y = EaseOutElastic(_curLerpTime, _startValue.y, _endValue.y, _timeOfAnim);
        float z = EaseOutElastic(_curLerpTime, _startValue.z, _endValue.z, _timeOfAnim);
        return new Vector3(x, y, z);
    }

    protected float ReturnTween(float _curLerpTime, float _startValue, float _endValue, float _timeOfAnim)
    {
        return EaseOutElastic(_curLerpTime, _startValue, _endValue, _timeOfAnim);
    }

    protected Vector3 LerpUnclamped(Vector3 a, Vector3 b, float t)
    {
        return new Vector3((a.x + t * (b.x - a.x)), (a.y + t * (b.y - a.y)), (a.z + t * (b.z - a.z)));
    }

    #endregion

    #endregion

}