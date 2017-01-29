using UnityEngine;
using System.Collections;

public static class ConvertUtil
{

    #region Colour
    public static Color Vector3ToColour(Vector3 RGB)
    {
        if (RGB.x <= 1 || RGB.y <= 1 || RGB.z <= 1)
        {
            return new Color(RGB.x, RGB.y, RGB.z);
        }
        else
        {
            return new Color(NormaliseColour(RGB.x), NormaliseColour(RGB.y), NormaliseColour(RGB.z));
        }
    }

    public static float NormaliseColour(float number)
    {
        return number / 255;
    }
    #endregion

}
