using UnityEngine;
using System.Collections;
using XboxCtrlrInput;

public class MarbleGameController : MonoBehaviour
{
    public float playerTimeScale;
    //
    public enum InputTypes { KeyboardMouse, XboxController, Phone };

    public InputTypes lastUsedInputType;

    #region KeyboardButtons
    private string[] _keyboardButtonPossibilities = { "space", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
    private string[] _keyboardAxisPossibilities = { "Horizontal", "Vertical" };
    #endregion
    #region ControllerButtons
    private string[] _controllerButtonPossibilities = { };
    private string[] _controllerAxisPossibilities = { };
    #endregion

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(GetInputType());
    }

    InputTypes GetInputType()
    {
        #region KeyboardTests
        foreach (var _buttonInput in _keyboardButtonPossibilities)
        {
            if (Input.GetKey(_buttonInput))
            {
                return InputTypes.KeyboardMouse;
            }
        }
        foreach (var _axisInput in _keyboardAxisPossibilities)
        {
            if (Input.GetAxis(_axisInput) != 0)
            {
                return InputTypes.KeyboardMouse;
            }
        }
        #endregion

        #region ControllerTests
        if (XCI.GetButton(XboxButton.A))
        {
            return InputTypes.XboxController;
        }
        #endregion
        return InputTypes.Phone;
    }
}
