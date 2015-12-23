using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(CameraController))]
public class CameraControllerEditor : Editor
{
    GUIContent MovementStateContent = new GUIContent("Movement Type", "Swap how the camera will operate under certain circumstances");
    GUIContent LayerMaskContent = new GUIContent("Collision Layer Mask", "Selecting an option means that a raycast will collide with it.");

    public SerializedProperty
    #region GenericProperties
        _movementStateEnumProperty,
        _targetTransformProperty,
    #endregion
    #region OtherProperties
        _layerMaskProperty,
        _startDistanceProperty,
        _xSpeedProperty,
        _ySpeedProperty,
        _zoomSensitivityProperty,
        _yMinLimitProperty,
        _yMaxLimitProperty,
        _distanceMinProperty,
        _distanceMaxProperty,
        _startCamHeightProperty;
    #endregion

    void OnEnable()
    {
        // Setup the SerializedProperties
        #region GenericSetup
        _movementStateEnumProperty = serializedObject.FindProperty("selectedMoveState");
        _targetTransformProperty = serializedObject.FindProperty("targTransform");
        #endregion
        #region OtherSetup
        _layerMaskProperty = serializedObject.FindProperty("ignoreCollisionLayers");
        _startDistanceProperty = serializedObject.FindProperty("startDistance");
        _xSpeedProperty = serializedObject.FindProperty("xSpeed");
        _ySpeedProperty = serializedObject.FindProperty("ySpeed");
        _zoomSensitivityProperty = serializedObject.FindProperty("zoomSensitivity");
        _yMinLimitProperty = serializedObject.FindProperty("yMinLimit");
        _yMaxLimitProperty = serializedObject.FindProperty("yMaxLimit");
        _distanceMinProperty = serializedObject.FindProperty("distanceMin");
        _distanceMaxProperty = serializedObject.FindProperty("distanceMax");
        _startCamHeightProperty = serializedObject.FindProperty("startCamHeight");
        #endregion
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        CameraController cameraControllerScript = (CameraController)target;

        #region Enum Variables
        EditorGUILayout.PropertyField(_movementStateEnumProperty, MovementStateContent);
        CameraController.MoveState _moveState = (CameraController.MoveState)_movementStateEnumProperty.enumValueIndex;
        cameraControllerScript.selectedMoveState = (CameraController.MoveState)_movementStateEnumProperty.enumValueIndex;
        #endregion

        switch (_moveState)
        {
            case CameraController.MoveState.ViewMode1:
                DefaultControls();
                EditorGUILayout.PropertyField(_layerMaskProperty, LayerMaskContent);
                EditorGUILayout.PropertyField(_startDistanceProperty);
                EditorGUILayout.PropertyField(_xSpeedProperty);
                EditorGUILayout.PropertyField(_ySpeedProperty);
                EditorGUILayout.PropertyField(_zoomSensitivityProperty);
                EditorGUILayout.PropertyField(_yMinLimitProperty);
                EditorGUILayout.PropertyField(_yMaxLimitProperty);
                EditorGUILayout.PropertyField(_distanceMinProperty);
                EditorGUILayout.PropertyField(_distanceMaxProperty);
                break;
            case CameraController.MoveState.ViewMode2:
                DefaultControls();
                EditorGUILayout.PropertyField(_startDistanceProperty);
                EditorGUILayout.PropertyField(_xSpeedProperty);
                EditorGUILayout.PropertyField(_ySpeedProperty);
                EditorGUILayout.PropertyField(_zoomSensitivityProperty);
                EditorGUILayout.PropertyField(_yMinLimitProperty);
                EditorGUILayout.PropertyField(_yMaxLimitProperty);
                EditorGUILayout.PropertyField(_distanceMinProperty);
                EditorGUILayout.PropertyField(_distanceMaxProperty);
                break;
            case CameraController.MoveState.UnInteractablePan1:
                DefaultControls();
                EditorGUILayout.PropertyField(_layerMaskProperty, LayerMaskContent);
                EditorGUILayout.PropertyField(_startDistanceProperty);
                EditorGUILayout.PropertyField(_xSpeedProperty);
                EditorGUILayout.PropertyField(_distanceMinProperty);
                EditorGUILayout.PropertyField(_startCamHeightProperty);
                break;
            case CameraController.MoveState.UnInteractablePan2:
                DefaultControls();
                EditorGUILayout.PropertyField(_startDistanceProperty);
                EditorGUILayout.PropertyField(_xSpeedProperty);
                EditorGUILayout.PropertyField(_distanceMinProperty);
                EditorGUILayout.PropertyField(_startCamHeightProperty);
                break;
        }
        serializedObject.ApplyModifiedProperties();
    }

    void DefaultControls()
    {
        EditorGUILayout.PropertyField(_targetTransformProperty);
    }
}
