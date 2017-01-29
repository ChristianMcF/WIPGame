using Util;
using UnityEngine;
using System.Collections;

public class CanvasGroupTween : BaseTween
{
    #region Variables
    private CanvasGroup canvasGroupComp;
    private float startValue;
    #endregion

    #region Initialisers
    void Awake()
    {
        //Get the RectTransform component so it's easier to access
        canvasGroupComp = this.gameObject.GetComponent<CanvasGroup>();
        //Set the start value to the current transform of this object
        startValue = canvasGroupComp.alpha;
    }
    #endregion

    #region Update Methods
    void Update()
    {
        //While the animation is not yet finished.
        if (canvasGroupComp.alpha != endValueFloat)
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
            canvasGroupComp.alpha = Mathf.Lerp(startValue, endValueFloat, t);
        }
        else //If the animation is finished
        {
            OnEnd();
            //Destroy this component
            Destroy(this);
        }

    }
    #endregion

    #region EndCommands
    protected override void DisableCanvas()
    {
        gameObject.GetComponent<Canvas>().enabled = false;
    }
    #endregion
}
