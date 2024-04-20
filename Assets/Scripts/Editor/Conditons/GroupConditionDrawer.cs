using System;
using UnityEditor;
using UnityEngine;

namespace EscapeRoom.QuestLogic.EditorScripts
{
    [CustomPropertyDrawer(typeof(GroupCondition))]
    public class GroupConditionDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            var groupCondition = (GroupCondition)property.boxedValue;
            GenericMenu.MenuFunction2 menuCallback = data =>
            {
                groupCondition.AddCondition((Type)data);
            };

            EditorGUILayout.LabelField(new GUIContent($"State: {groupCondition.IsTrue}"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("all"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("invert"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("conditions"));
            
            if(GUILayout.Button("Add condition"))
            {
                GenericMenu menu = new GenericMenu();
                // menu.AddItem(new GUIContent(nameof(GroupCondition)), false, menuCallback, typeof(GroupCondition)); // nested group conditions are not supported yet
                menu.AddItem(new GUIContent(nameof(StateConditionBool)), false, menuCallback, typeof(StateConditionBool));
                menu.AddItem(new GUIContent(nameof(StateConditionInt)), false, menuCallback, typeof(StateConditionInt));
                menu.ShowAsContext();
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}