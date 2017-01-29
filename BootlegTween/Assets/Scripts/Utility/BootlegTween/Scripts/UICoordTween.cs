using Util;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICoordTween : BaseTween
{
    #region Variables
    private RectTransform transfrm;
    private RectTransform parentCanvas;
    private Vector2 startValue;
    #endregion

    #region Initialisers

    /// <summary>
    /// Practically this is a constructor for the animation.
    /// </summary>
    /// <param name="endValue">The final Vector for the animation to play</param>
    /// <param name="timeOfAnim">The amount of time in seconds to play this animation for</param>
    /// <param name="curve">The motion curve to follow</param>
    public void SetAnimation(RectTransform parentCanvas, Vector2 endValue, float timeOfAnim, CurveObject curve)
    {
        this.parentCanvas = parentCanvas;
        this.endValueVector2 = endValue;
        this.timeOfAnim = timeOfAnim;
        this.selectedCurve = curve;
        Begin();
    }

    protected override void Begin()
    {
        //Get the RectTransform component so it's easier to access
        transfrm = this.gameObject.GetComponent<RectTransform>();
        startValue = new Vector2(transfrm.rect.width / (parentCanvas.sizeDelta.x), transfrm.rect.height / (parentCanvas.sizeDelta.y));
    }
    #endregion

    #region Update Methods
    void Update()
    {
        Vector2 currentValue = new Vector2(transfrm.rect.width / (parentCanvas.sizeDelta.x), transfrm.rect.height / (parentCanvas.sizeDelta.y));
        //While the animation is not yet finished.
        if (currentValue != endValueVector2)
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
            CanvasUtil.CoordDialogBox(parentCanvas, transfrm, new Vector2(0, 0), Vector2.Lerp(startValue, endValueVector2, t));

        }
        else //If the animation is finished
        {
            //Destroy this component
            Destroy(this);
        }

    }
    #endregion
}
