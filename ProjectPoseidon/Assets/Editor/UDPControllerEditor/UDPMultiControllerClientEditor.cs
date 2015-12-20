using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(UDPMultiControllerClient))]
public class UDPMultiControllerClientEditor : Editor
{
    GUIContent UDPManagerContent = new GUIContent("UDP Manager", "Place here the empty gameobject which has the UDP manager script on it");
    GUIContent controlNameContent = new GUIContent("Control Name", "Name to match on the corresponding serverside object");
    GUIContent methodToCallContent = new GUIContent("Method to Call", "Name of the corresponding method on the server object");
    GUIContent controlTypeContent = new GUIContent("Controller Type", "Type of controller");
    GUIContent sendDataContent = new GUIContent("Enable Data Sending?", "Will send data to object depending on option");
    GUIContent buttonTransTypeContent = new GUIContent("Transition", "Transition for the button to undergo when clicked");

    public SerializedProperty
    #region GenericProperties
        udpManager,
        sendDataEnum,
        controllerType,
    #endregion
    #region ButtonProperties
        buttonTransType,
        defaultButtonImg,
        clickButtonImg,
        disableButtonImg,
        normalColourImg,
        clickedColourImg,
        disabledColourImg,
        fadeDurationImg;
    #endregion


    void OnEnable()
    {
        // Setup the SerializedProperties
        #region GenericSetup
        udpManager = serializedObject.FindProperty("udpManager");
        sendDataEnum = serializedObject.FindProperty("selectedSendData");
        controllerType = serializedObject.FindProperty("selectedControllerType");
        #endregion
        #region ButtonSetup
        buttonTransType = serializedObject.FindProperty("selectedButtonTransition");
        defaultButtonImg = serializedObject.FindProperty("defaultButtonImage");
        clickButtonImg = serializedObject.FindProperty("clickedButtonImage");
        disableButtonImg = serializedObject.FindProperty("disabledButtonImage");
        normalColourImg = serializedObject.FindProperty("normalColour");
        clickedColourImg = serializedObject.FindProperty("pressedColour");
        disabledColourImg = serializedObject.FindProperty("disabledColour");
        fadeDurationImg = serializedObject.FindProperty("fadeDuration");
        #endregion
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        UDPMultiControllerClient udpControllerScript = (UDPMultiControllerClient)target;

        #region Enum Variables
        EditorGUILayout.PropertyField(sendDataEnum, sendDataContent);
        UDPMultiControllerClient.SendData sendType = (UDPMultiControllerClient.SendData)sendDataEnum.enumValueIndex;
        udpControllerScript.selectedSendData = (UDPMultiControllerClient.SendData)sendDataEnum.enumValueIndex;

        EditorGUILayout.PropertyField(controllerType, controlTypeContent);
        UDPMultiControllerClient.ControllerType conType = (UDPMultiControllerClient.ControllerType)controllerType.enumValueIndex;
        udpControllerScript.selectedControllerType = (UDPMultiControllerClient.ControllerType)controllerType.enumValueIndex;
        #endregion

        EditorGUILayout.PropertyField(udpManager, UDPManagerContent);
        switch (conType)
        {
            #region ButtonGUIRegion
            case UDPMultiControllerClient.ControllerType.Button:
                EditorGUILayout.HelpBox("None of this is implemented Yet", MessageType.Error, true);
                DefaultControls();
                //
                EditorGUILayout.PropertyField(buttonTransType, buttonTransTypeContent);
                UDPMultiControllerClient.ButtonTransitionType TransType = (UDPMultiControllerClient.ButtonTransitionType)buttonTransType.enumValueIndex;
                udpControllerScript.selectedButtonTransition = (UDPMultiControllerClient.ButtonTransitionType)buttonTransType.enumValueIndex;
                //
                switch (TransType)
                {
                    case UDPMultiControllerClient.ButtonTransitionType.None:
                        EditorGUILayout.PropertyField(defaultButtonImg);
                        break;
                    case UDPMultiControllerClient.ButtonTransitionType.Sprite_Swap:
                        EditorGUILayout.PropertyField(defaultButtonImg);
                        EditorGUILayout.PropertyField(clickButtonImg);
                        EditorGUILayout.PropertyField(disableButtonImg);
                        break;
                    case UDPMultiControllerClient.ButtonTransitionType.Colour_Tint:
                        EditorGUILayout.PropertyField(defaultButtonImg);
                        EditorGUILayout.PropertyField(normalColourImg);
                        EditorGUILayout.PropertyField(clickedColourImg);
                        EditorGUILayout.PropertyField(disabledColourImg);
                        EditorGUILayout.PropertyField(fadeDurationImg);
                        break;
                }
                break;

            #endregion

            case UDPMultiControllerClient.ControllerType.DPad:
                EditorGUILayout.HelpBox("None of this is implemented Yet", MessageType.Error, true);
                DefaultControls();
                break;
            case UDPMultiControllerClient.ControllerType.Joystick:
                EditorGUILayout.HelpBox("None of this is implemented Yet", MessageType.Error, true);
                DefaultControls();
                break;
            case UDPMultiControllerClient.ControllerType.Gyro:
                break;
        }
        switch (sendType)
        {
            default:
                break;
        }
        serializedObject.ApplyModifiedProperties();
    }

    void DefaultControls()
    {
        UDPMultiControllerClient udpControllerScript = (UDPMultiControllerClient)target;
        udpControllerScript.controlName = EditorGUILayout.TextField(controlNameContent, udpControllerScript.controlName);
        udpControllerScript.methodToCall = EditorGUILayout.TextField(methodToCallContent, udpControllerScript.methodToCall);
    }
}