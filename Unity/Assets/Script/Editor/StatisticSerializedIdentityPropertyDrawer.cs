using Game;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(StatisticSerializedIdentity), true)]
public class StatisticSerializedIdentityPropertyDrawer : PropertyDrawer
{
    private List<StatisticDefinition> statisticDefinitions = null;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        float yOffset = position.y;
        float fieldHeight = EditorGUIUtility.singleLineHeight + 2;

        SerializedProperty iterator = property.Copy();
        SerializedProperty endProperty = iterator.GetEndProperty();

        Rect fieldRect = new Rect(position.x, yOffset, position.width, EditorGUIUtility.singleLineHeight);

        SerializedProperty nameProperty = property.FindPropertyRelative("name");
        EditorGUI.PropertyField(fieldRect, nameProperty, true);
        yOffset += EditorGUI.GetPropertyHeight(nameProperty, true) + 2;

        SerializedProperty definitionIdProperty = property.FindPropertyRelative("definitionId");

        if (statisticDefinitions == null)
        {
            string[] statisticDefinitionGuids = AssetDatabase.FindAssets("t:StatisticDefinition");
            statisticDefinitions = statisticDefinitionGuids.Select(x => AssetDatabase.LoadAssetAtPath<StatisticDefinition>(AssetDatabase.GUIDToAssetPath(x))).ToList();
        }

        EditorGUI.BeginChangeCheck();
        fieldRect = new Rect(position.x, yOffset, position.width, EditorGUIUtility.singleLineHeight);
        StatisticDefinition statisticDefinition = EditorGUI.ObjectField(fieldRect, new GUIContent("Definition"), statisticDefinitions.FirstOrDefault(x => x.HumanReadableId == definitionIdProperty.stringValue), typeof(StatisticDefinition), false) as StatisticDefinition;
        if (EditorGUI.EndChangeCheck())
        {
            if (statisticDefinition == null)
            {
                definitionIdProperty.stringValue = string.Empty;
            }
            else
            {
                definitionIdProperty.stringValue = statisticDefinition.HumanReadableId;
            }

            property.serializedObject.ApplyModifiedProperties();
        }

        yOffset += EditorGUI.GetPropertyHeight(definitionIdProperty, true) + 2;

        while (iterator.NextVisible(true) && !SerializedProperty.EqualContents(iterator, endProperty))
        {
            if (SerializedProperty.EqualContents(iterator, definitionIdProperty) || SerializedProperty.EqualContents(iterator, nameProperty))
                continue;

            fieldRect = new Rect(position.x, yOffset, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(fieldRect, iterator, true);
            yOffset += EditorGUI.GetPropertyHeight(iterator, true) + 2;  // Dynamically determine the height
        }


        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float totalHeight = 0;


        if (property.isExpanded)
        {
            SerializedProperty iterator = property.Copy();
            SerializedProperty endProperty = iterator.GetEndProperty();

            while (iterator.NextVisible(true) && !SerializedProperty.EqualContents(iterator, endProperty))
            {
                totalHeight += EditorGUI.GetPropertyHeight(iterator, true) + 2;
            }
        }

        return totalHeight;
    }
}
