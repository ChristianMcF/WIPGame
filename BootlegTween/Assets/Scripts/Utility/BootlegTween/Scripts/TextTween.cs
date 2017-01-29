using Util;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextTween : BaseTween
{

    #region Variables
    private BootlegTween.ActionType actionType;
    private Text textComponent;
    private Vector3 startValue;
    #endregion

    #region Initialisers
    void Awake()
    {
        //Set the start value to the currect transform of this object
        startValue = transform.position;
    }

    /// <summary>
    /// Practically this is a constructor for the animation.
    /// </summary>
    /// <param name="endValue">The final Vector for the animation to play</param>
    /// <param name="timeOfAnim">The amount of time in seconds to play this animation for</param>
    /// <param name="curve">The motion curve to follow</param>
    public void ColourTween(Vector3 endValue, float timeOfAnim, CurveObject curve)
    {
        textComponent = GetComponent<Text>();
        this.actionType = BootlegTween.ActionType.Colour;
        this.endValueVector3 = endValue;
        this.timeOfAnim = timeOfAnim;
        this.selectedCurve = curve;
    }

    public void PositionTween(Vector3 endValue, float timeOfAnim, CurveObject curve)
    {
        this.actionType = BootlegTween.ActionType.Position;
        this.endValueVector3 = endValue;
        this.timeOfAnim = timeOfAnim;
        this.selectedCurve = curve;
    }
    #endregion

    #region Update Methods
    void Update()
    {
        switch (actionType)
        {
            case BootlegTween.ActionType.Colour:
                TweenColour();
                break;
            case BootlegTween.ActionType.Position:
                TweenPosition();
                break;
            case BootlegTween.ActionType.Scale:
                break;
            default:
                break;
        }
    }
    #endregion

    #region TweenMethods
    void TweenPosition()
    {
        //While the animation is not yet finished.
        if (transform.position != endValueVector3)
        {
            //Increment the current lerp time by deltatime
            curLerpTime += Time.deltaTime;

            if (curLerpTime > timeOfAnim)
            {
                curLerpTime = timeOfAnim;
            }
            //Call the time curve method to return the time percentage
            t = timeCurve(t, selectedCurve);
            //Set the scale of the object to the Lerp
            transform.position = Vector3.Lerp(startValue, endValueVector3, t);
        }
        else //If the animation is finished
        {
            //Destroy this component
            Destroy(this);
        }
    }

    void TweenColour()
    {
        //While the animation is not yet finished.
        if (textComponent.color != ConvertUtil.Vector3ToColour(endValueVector3))
        {
            //Increment the current lerp time by deltatime
            curLerpTime += Time.deltaTime;

            if (curLerpTime > timeOfAnim)
            {
                curLerpTime = timeOfAnim;
            }
            //Call the time curve method to return the time percentage
            t = timeCurve(t, selectedCurve);
            //Set the scale of the object to the Lerp
            textComponent.color = ConvertUtil.Vector3ToColour(Vector3.Lerp(startValue, endValueVector3, t));
        }
        else //If the animation is finished
        {
            //Destroy this component
            Destroy(this);
        }
    }
    #endregion
}
