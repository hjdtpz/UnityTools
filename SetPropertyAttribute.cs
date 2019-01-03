using UnityEngine;
using System.Collections;

public class SetPropertyAttribute : PropertyAttribute
{
   
    public string PropertyName { get; private set; }

    public float MinValue { get; private set; }

    public float MaxValue { get; private set; }

    public bool NeedDrawRange { get; private set; }
    
    public SetPropertyAttribute(string propertyName=null)
    {
        this.PropertyName=PropertyName;
    }

    public SetPropertyAttribute(float min, float max,string propertyName=null):this(propertyName)
    {
        MinValue = min;
        MaxValue = max;
        NeedDrawRange = true;
    }
    
}
