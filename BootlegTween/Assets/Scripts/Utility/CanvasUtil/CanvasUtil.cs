using UnityEngine;
using System.Collections;

public static class CanvasUtil
{

    /// <summary>
    /// Positions the Dialogbox according to percentages of the screensize
    /// </summary>
    /// <param name="rect">The rectTransform of the object</param>
    /// <param name="x">The percentage value of x</param>
    /// <param name="y">the percentage value of y</param>
    /// <returns></returns>
    public static RectTransform PositionDialogBox(RectTransform rect, float x, float y)
    {
        rect.position = new Vector3((float)Screen.width * x, (float)Screen.height * y, rect.position.x);
        return rect;
    }

    /// <summary>
    /// Scales and positions dialogbox according to a percentage of the reference resolution
    /// </summary>
    /// <param name="rect">The transform to change values of</param>
    /// <param name="bottomLeft">The bottom left vector for positioning</param>
    /// <param name="topRight">The top right vector for positioning</param>
    /// <returns></returns>
    public static RectTransform CoordDialogBox(RectTransform parentCanvas, RectTransform rect, Vector2 bottomLeft, Vector2 topRight)
    {
        Vector2 bottomLeftScreen = new Vector2((parentCanvas.sizeDelta.x) * bottomLeft.x, (parentCanvas.sizeDelta.y) * bottomLeft.y);
        Vector2 topRightScreen = new Vector2((parentCanvas.sizeDelta.x) * topRight.x, (parentCanvas.sizeDelta.y) * topRight.y);
        rect.sizeDelta = new Vector2((topRightScreen.x - bottomLeftScreen.x), (topRightScreen.y - bottomLeftScreen.y));
        rect = PositionDialogBox(rect, ((topRight.x + bottomLeft.x) / 2), ((topRight.y + bottomLeft.y) / 2));
        rect.position = new Vector3(rect.position.x, rect.position.y, 0);
        return rect;
    }
}
