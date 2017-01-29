using Util;
using UnityEngine;
using System.Collections;

public class TransformTween : BaseTween
{

    #region Variables
    private Transform scale;
    private Vector3 startValue;
    #endregion

    #region Initialisers
    void Awake()
    {
        //Get the RectTransform component so it's easier to access
        scale = this.gameObject.GetComponent<Transform>();
        //Set the start value to the currect transform of this object
        startValue = scale.position;
    }
    #endregion

    #region Update Methods
    void Update()
    {

        //While the animation is not yet finished.
        if (curLerpTime != timeOfAnim)
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
            scale.position = LerpUnclamped(startValue, endValueVector3, t);
        }
        else //If the animation is finished
        {
            //Destroy this component
            Destroy(this);
        }

    }
    #endregion
}
