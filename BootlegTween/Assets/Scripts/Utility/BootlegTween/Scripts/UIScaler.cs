using Util;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIScaler : BaseTween
{
    #region Variables
    private RectTransform scale;
    private Vector3 startValue;
    #endregion

    #region Initialisers
    void Awake()
    {
        //Get the RectTransform component so it's easier to access
        scale = this.gameObject.GetComponent<RectTransform>();
        //Set the start value to the currect transform of this object
        startValue = scale.localScale;
    }

    #endregion

    #region Update Methods
    void Update()
    {

        //While the animation is not yet finished.
        if (scale.localScale != endValueVector3)
        {
            //Increment the current lerp time by deltatime
            curLerpTime += Time.deltaTime;

            if (curLerpTime > timeOfAnim)
            {
                curLerpTime = timeOfAnim;
            }
            //Do ease out elastic if selected
            //Call the time curve method to return the time percentage
            t = timeCurve(t, selectedCurve);
            //Set the scale of the object to the Lerp
            scale.localScale = Vector3.Lerp(startValue, endValueVector3, t);
        }
        else //If the animation is finished
        {
            //Destroy this component
            Destroy(this);
        }

    }
    #endregion
}