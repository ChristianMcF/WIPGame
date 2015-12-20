using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(UDPMultiControllerClient))]
public class UDPMultiControllerClientEditor : Editor
{
	GUIContent controlName = new GUIContent ("Control Name", "Name to match on the corresponding serverside object");
	GUIContent methodToCall = new GUIContent ("Method to Call", "Name of the corresponding method on the server object");
	GUIContent controlType = new GUIContent ("Controller Type", "Type of controller");

	public SerializedProperty 
		//debug,
		udpManager,
		controllerType,
		defaultButtonImg,
		clickButtonImg;


	void OnEnable ()
	{
		// Setup the SerializedProperties
		//debug = serializedObject.FindProperty ("_DeleteMe");
		udpManager = serializedObject.FindProperty ("udpManager");
		controllerType = serializedObject.FindProperty ("selectedControllerType");
		defaultButtonImg = serializedObject.FindProperty ("DefaultButtonImage");
		clickButtonImg = serializedObject.FindProperty ("ClickedButtonImage");
	}

	public override void OnInspectorGUI ()
	{
		serializedObject.Update ();

		UDPMultiControllerClient udpControllerScript = (UDPMultiControllerClient)target;
		EditorGUILayout.PropertyField (controllerType, controlType);
		UDPMultiControllerClient.ControllerType conType = (UDPMultiControllerClient.ControllerType)controllerType.enumValueIndex;
		udpControllerScript.selectedControllerType = (UDPMultiControllerClient.ControllerType)controllerType.enumValueIndex;

		EditorGUILayout.PropertyField (udpManager, controlName);
		//EditorGUILayout.PropertyField (debug, controlName);

		switch (conType) {
		case UDPMultiControllerClient.ControllerType.Button:
			DefaultControls ();
			EditorGUILayout.PropertyField (defaultButtonImg);
			EditorGUILayout.PropertyField (clickButtonImg);
			break;

		case UDPMultiControllerClient.ControllerType.DPad:
			DefaultControls ();
			break;
		case UDPMultiControllerClient.ControllerType.Joystick:
			DefaultControls ();
			break;
		case UDPMultiControllerClient.ControllerType.Gyro:
			DefaultControls ();
			break;
		}
		serializedObject.ApplyModifiedProperties ();
	}

	void DefaultControls ()
	{
		UDPMultiControllerClient udpControllerScript = (UDPMultiControllerClient)target;
		udpControllerScript.controlName = EditorGUILayout.TextField (controlName, udpControllerScript.controlName);
		udpControllerScript.methodToCall = EditorGUILayout.TextField (methodToCall, udpControllerScript.methodToCall);
	}
}