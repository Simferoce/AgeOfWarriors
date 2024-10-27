using Game.Statistics;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomPropertyDrawer(typeof(StatisticRegistry))]
public class StatisticRegistryPropertyDrawer : PropertyDrawer
{
    private ReorderableList reorderableList;
    private Dictionary<int, bool> isEditingLabel = new Dictionary<int, bool>();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Rect test = new Rect(position.x, position.y, position.width, position.height);
        EditorGUI.LabelField(test, label);
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty statisticsProperty = property.FindPropertyRelative("statistics");

        if (reorderableList == null)
            reorderableList = new ReorderableList(property.serializedObject, statisticsProperty, true, true, true, true)
            {
                drawHeaderCallback = (Rect rect) => EditorGUI.LabelField(rect, "Statistics"),

                drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    SerializedProperty statisticProperty = statisticsProperty.GetArrayElementAtIndex(index);
                    SerializedProperty definitionProperty = statisticProperty.FindPropertyRelative("definition");

                    if (!isEditingLabel.ContainsKey(index))
                        isEditingLabel[index] = false;

                    Rect foldoutRect = new Rect(rect.x, rect.y, 15, EditorGUIUtility.singleLineHeight);
                    statisticProperty.isExpanded = EditorGUI.Foldout(foldoutRect, statisticProperty.isExpanded, GUIContent.none, true);

                    Rect labelRect = new Rect(rect.x + 15, rect.y, rect.width - 15, EditorGUIUtility.singleLineHeight);

                    Event evt = Event.current;
                    if (evt.type == EventType.MouseDown)
                    {
                        if (labelRect.Contains(evt.mousePosition) && evt.clickCount == 2)
                        {
                            isEditingLabel[index] = true;
                            evt.Use();
                        }
                        else if (!labelRect.Contains(evt.mousePosition))
                        {
                            isEditingLabel[index] = false;
                        }
                    }

                    if (definitionProperty.objectReferenceValue is StatisticDefinition definition)
                    {
                        Rect iconRect = new Rect(rect.x + 15, rect.y, 16, 16);
                        DrawTexturePreview(iconRect, definition.Icon);

                        if (GUI.Button(iconRect, GUIContent.none, GUIStyle.none))
                            EditorGUIUtility.PingObject(definition);

                        labelRect.x += iconRect.width + 5;
                    }

                    if (isEditingLabel[index])
                    {
                        statisticProperty.FindPropertyRelative("name").stringValue = EditorGUI.TextField(labelRect, statisticProperty.FindPropertyRelative("name").stringValue);

                        if (evt.keyCode == KeyCode.Return || evt.keyCode == KeyCode.KeypadEnter)
                            isEditingLabel[index] = false;
                    }
                    else
                    {
                        EditorGUI.LabelField(labelRect, statisticProperty.displayName);
                    }

                    if (statisticProperty.isExpanded)
                    {
                        EditorGUI.indentLevel++;
                        float yOffset = rect.y + EditorGUIUtility.singleLineHeight;

                        SerializedProperty iterator = statisticProperty.Copy();
                        SerializedProperty endProperty = iterator.GetEndProperty();

                        iterator.NextVisible(true);
                        while (iterator.propertyPath != endProperty.propertyPath)
                        {
                            if (iterator.name != "definition" && iterator.name != "name")
                            {
                                Rect propertyRect = new Rect(rect.x + 15, yOffset, rect.width - 15, EditorGUIUtility.singleLineHeight);
                                EditorGUI.PropertyField(propertyRect, iterator, true);
                                yOffset += EditorGUI.GetPropertyHeight(iterator, true);
                            }
                            iterator.NextVisible(false);
                        }
                        EditorGUI.indentLevel--;
                    }
                },

                elementHeightCallback = (int index) =>
                {
                    SerializedProperty statisticProperty = statisticsProperty.GetArrayElementAtIndex(index);
                    float height = EditorGUIUtility.singleLineHeight;

                    if (statisticProperty.isExpanded)
                    {
                        SerializedProperty iterator = statisticProperty.Copy();
                        SerializedProperty endProperty = iterator.GetEndProperty();

                        iterator.NextVisible(true);
                        while (iterator.propertyPath != endProperty.propertyPath)
                        {
                            if (iterator.name != "definition" && iterator.name != "name")
                                height += EditorGUI.GetPropertyHeight(iterator, iterator.isExpanded);
                            iterator.NextVisible(false);
                        }
                    }
                    return height;
                },

                onAddCallback = (ReorderableList list) =>
                    ShowDefinitionSelectionPopup(statisticsProperty),

                onRemoveCallback = (ReorderableList list) =>
                {
                    statisticsProperty.DeleteArrayElementAtIndex(list.index);
                    statisticsProperty.serializedObject.ApplyModifiedProperties();
                }
            };

        reorderableList.DoLayoutList();

        EditorGUI.EndProperty();
    }

    private void ShowDefinitionSelectionPopup(SerializedProperty statisticsProperty)
    {
        List<StatisticDefinition> definitions = GetStatisticDefinitions();
        GenericMenu menu = new GenericMenu();

        foreach (StatisticDefinition definition in definitions)
            menu.AddItem(new GUIContent(definition.Title), false, () =>
            {
                statisticsProperty.InsertArrayElementAtIndex(statisticsProperty.arraySize);
                Statistic newStatistic = definition.BuildStatistic();
                statisticsProperty.GetArrayElementAtIndex(statisticsProperty.arraySize - 1).managedReferenceValue = newStatistic;
                statisticsProperty.serializedObject.ApplyModifiedProperties();
            });

        menu.ShowAsContext();
    }

    private List<StatisticDefinition> GetStatisticDefinitions()
    {
        string[] guids = AssetDatabase.FindAssets("t:StatisticDefinition");
        List<StatisticDefinition> definitions = new List<StatisticDefinition>();

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            StatisticDefinition definition = AssetDatabase.LoadAssetAtPath<StatisticDefinition>(path);
            if (definition != null)
                definitions.Add(definition);
        }

        return definitions;
    }

    private void DrawTexturePreview(Rect position, Sprite sprite)
    {
        Vector2 fullSize = new Vector2(sprite.texture.width, sprite.texture.height);
        Vector2 size = new Vector2(sprite.textureRect.width, sprite.textureRect.height);

        Rect coords = sprite.textureRect;
        coords.x /= fullSize.x;
        coords.width /= fullSize.x;
        coords.y /= fullSize.y;
        coords.height /= fullSize.y;

        Vector2 ratio;
        ratio.x = position.width / size.x;
        ratio.y = position.height / size.y;
        float minRatio = Mathf.Min(ratio.x, ratio.y);

        Vector2 center = position.center;
        position.width = size.x * minRatio;
        position.height = size.y * minRatio;
        position.center = center;

        GUI.DrawTextureWithTexCoords(position, sprite.texture, coords);
    }
}
