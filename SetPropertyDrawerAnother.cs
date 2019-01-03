using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

[CustomPropertyDrawer(typeof(SetPropertyAttribute))]
public class SetPropertyDrawer : PropertyDrawer
{
    private PropertyInfo propertyInfo = null;


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginChangeCheck();
        EditorGUI.PropertyField(position, property, label);

        if (EditorGUI.EndChangeCheck())
        {
            SetPropertyAttribute setPropertyAttribute = attribute as SetPropertyAttribute;
            property.serializedObject.ApplyModifiedProperties();
            object targetObj = property.serializedObject.targetObject;
            Type targetObjType = targetObj.GetType();
            if (propertyInfo == null)
            {
                propertyInfo = GetPropertyInfo(setPropertyAttribute, property, targetObjType);
            }
            if (propertyInfo == null)
            {
                Debug.LogError("Invalid property name: " + setPropertyAttribute.GetPropertyName() + "\nCheck your [SetProperty] attribute");
            }
            else
            {
                propertyInfo.SetValue(targetObj, fieldInfo.GetValue(targetObj), null);
            }
        }
    }
      
            
    private PropertyInfo GetPropertyInfo(SetPropertyAttribute setPropertyAttribute, SerializedProperty property, Type type)
    {
        PropertyInfo propertyInfo;
        string propertyName = null;
        if (setPropertyAttribute.GetPropertyName() != null)
        {
            propertyName = setPropertyAttribute.GetPropertyName();
        }
        else
        {
            char[] chars = property.name.ToCharArray();
            chars[0] = char.ToUpper(chars[0]);
            propertyName = new string(chars);
        }
        propertyInfo = type.GetProperty(propertyName);
        return propertyInfo;
    }
}
