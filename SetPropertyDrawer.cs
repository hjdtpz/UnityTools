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
        object targetObj = property.serializedObject.targetObject;
        object oldValue = fieldInfo.GetValue(targetObj);
        SetPropertyAttribute setPropertyAttribute = attribute as SetPropertyAttribute;

        EditorGUI.BeginChangeCheck();

        if (setPropertyAttribute.NeedDrawRange)
        {
            if (property.propertyType == SerializedPropertyType.Float)
               EditorGUI.Slider(position, property, setPropertyAttribute.MinValue, setPropertyAttribute.MaxValue, label);
            else if (property.propertyType == SerializedPropertyType.Integer)
                EditorGUI.IntSlider(position, property, (int)setPropertyAttribute.MinValue, (int)setPropertyAttribute.MaxValue, label);
        }
        else
        {
            EditorGUI.PropertyField(position, property, label);
        }


        // Change property value
        if (EditorGUI.EndChangeCheck())
        {
            property.serializedObject.ApplyModifiedProperties();
            Type targetObjType = targetObj.GetType();
            if (propertyInfo == null)
            {
                propertyInfo = GetPropertyInfo(setPropertyAttribute, property, targetObjType);

                //Check property
                if (propertyInfo == null)
                {
                    Debug.LogError("Invalid property name: " + setPropertyAttribute.PropertyName + "\nCheck your [SetProperty] attribute");
                }
            }
            else
            {
                Debug.Log(oldValue);
                object newValue = fieldInfo.GetValue(targetObj);
                Debug.Log(newValue);
                fieldInfo.SetValue(targetObj, oldValue);
                propertyInfo.SetValue(targetObj,newValue, null);
            }
        }
    }



    
    private PropertyInfo GetPropertyInfo(SetPropertyAttribute setPropertyAttribute, SerializedProperty property, Type type)
    {
        PropertyInfo propertyInfo;
        //Obtaining property names based on parameters
        string propertyName = null;
        if (!string.IsNullOrEmpty(setPropertyAttribute.PropertyName))
        {
            propertyName = setPropertyAttribute.PropertyName;
        }
        else
        {
            char[] chars = property.name.ToCharArray();
            chars[0] = char.ToUpper(chars[0]);
            propertyName = new string(chars);
        }
        //Get property
        propertyInfo = type.GetProperty(propertyName);
        return propertyInfo;
    }
}
