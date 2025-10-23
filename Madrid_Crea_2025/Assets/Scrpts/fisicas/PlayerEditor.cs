using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(PlayerData))]
public class PlayerEditor : Editor
{
    #region Properties

    #region Statds
    private SerializedProperty moveSpeed;

    private SerializedProperty jumpHeight;

    private SerializedProperty gravity;

    private SerializedProperty terminalSpeed;

    private SerializedProperty inputBuffer;

    private SerializedProperty coyoteTime;

    private SerializedProperty heigth2AirStand;

    private SerializedProperty airStandGravityMod;

    private SerializedProperty jumpVelocity;
    #endregion

    #endregion

    #region Foldout Bools

    private bool foldoutComponents;

    private bool foldoutStads = true;

    #endregion

    private void OnEnable()
    {

        moveSpeed = serializedObject.FindProperty("moveSpeed");
        jumpHeight = serializedObject.FindProperty("jumpHeight");
        gravity = serializedObject.FindProperty("gravity");
        terminalSpeed = serializedObject.FindProperty("terminalSpeed");
        inputBuffer = serializedObject.FindProperty("inputBuffer");
        coyoteTime = serializedObject.FindProperty("coyoteTime");
        heigth2AirStand = serializedObject.FindProperty("heigth2AirStand");
        airStandGravityMod = serializedObject.FindProperty("airStandGravityMod");

        jumpVelocity = serializedObject.FindProperty("jumpVelocity");

    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        PlayerData player = (PlayerData)target;

        foldoutStads = EditorGUILayout.BeginFoldoutHeaderGroup(foldoutStads, "Stads");
        if (foldoutStads)
        {
            EditorGUILayout.PropertyField(moveSpeed);
            EditorGUILayout.PropertyField(jumpHeight);
            EditorGUILayout.PropertyField(gravity);
            EditorGUILayout.PropertyField(terminalSpeed);
            EditorGUILayout.PropertyField(inputBuffer);
            EditorGUILayout.PropertyField(coyoteTime);
            EditorGUILayout.PropertyField(heigth2AirStand);
            EditorGUILayout.PropertyField(airStandGravityMod);

        }
        heigth2AirStand.floatValue = Mathf.Clamp(heigth2AirStand.floatValue, 0, jumpHeight.floatValue);
        jumpVelocity.floatValue = MathF.Sqrt(2 * jumpHeight.floatValue * Math.Abs(gravity.floatValue * 9.81f));
        EditorGUILayout.PropertyField(jumpVelocity);
        serializedObject.ApplyModifiedProperties();
    }
}
