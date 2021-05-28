using UnityEngine;

public class ArrayElementTitleAttribute : PropertyAttribute
{
    public string varname;
    public ArrayElementTitleAttribute(string elementTitleVar)
    {
        varname = elementTitleVar;
    }
}

