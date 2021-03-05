using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DisableAttribute : PropertyAttribute
{
}

#if UNITY_EDITOR    //Unity エディターでのみ有効となる
[CustomPropertyDrawer(typeof(DisableAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}
#endif