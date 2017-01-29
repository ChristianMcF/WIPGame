using Util;
using UnityEngine;
using System.Collections;

public class CameraScaleTweener : BaseTween
{
    #region Variables
    private Camera cameraObj;
    private float startValue;
    #endregion

    #region Initialisers
    void Awake()
    {
        //Get the RectTransform component so it's easier to access
        cameraObj = this.gameObject.GetComponent<Camera>();
        //Set the start value to the currect transform of this object
        startValue = cameraObj.orthographicSize;
    }
    #endregion

    #region Update Methods
    void Update()
    {

        //While the animation is not yet finished.
        if (cameraObj.orthographicSize != endValueFloat)
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
            cameraObj.orthographicSize = Mathf.Lerp(startValue, endValueFloat, t);
        }
        else //If the animation is finished
        {
            //Destroy this component
            Destroy(this);
        }

    }
    #endregion
}
