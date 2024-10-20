using UnityEngine;
using System.Collections;
using UnityEditor;
using SFCore.MonoBehaviours;

[CustomEditor(typeof(MonoTest))]
[CanEditMultipleObjects]
public class MonoTestEditor : Editor
{
    //public override void OnInspectorGUI()
    //{
    //    serializedObject.Update();

    //    EditorGUILayout.PropertyField(serializedObject.FindProperty("Test"), new GUIContent("Test"));

    //    //Save all changes made on the inspector
    //    serializedObject.ApplyModifiedProperties();
    //}
}
