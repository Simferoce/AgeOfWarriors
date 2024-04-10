using Game;
using SerializeReferenceDropdown.Editor;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Statistic))]
public class StatisticPropertyDrawer : SerializeReferenceDropdownPropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Statistic statistic = property.boxedValue as Statistic;
        statistic?.SetDescription();
        base.OnGUI(position, property, label);
    }
}