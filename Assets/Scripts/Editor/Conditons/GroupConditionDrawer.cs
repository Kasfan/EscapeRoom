using System;
using System.Collections.Generic;
using System.Linq;
using EscapeRoom.EditorScripts.Helpers;
using UnityEditor;
using UnityEngine;

namespace EscapeRoom.QuestLogic.EditorScripts
{
    [CustomPropertyDrawer(typeof(GroupCondition))]
    public class GroupConditionDrawer : PropertyDrawer
    {
        private List<Type> conditions = null;

        private List<Type> Conditions
        {
            get
            {
                if (conditions == null)
                {
                    conditions = ReflectionTools.GetTypesImplementingInterface(typeof(Condition)).ToList();
                    conditions.Remove(typeof(GroupCondition)); // nesting group condition does not work yet
                }

                return conditions;
            }   
        }

        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            var groupCondition = (GroupCondition)property.boxedValue;

            void MenuCallback(object data)
            {
                groupCondition.AddCondition((Type)data);
            }

            EditorGUILayout.LabelField(new GUIContent($"State: {groupCondition.IsTrue}"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("all"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("invert"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("conditions"));
            
            if(GUILayout.Button("Add condition"))
            {
                GenericMenu menu = new GenericMenu();
                foreach (var condition in Conditions)
                    menu.AddItem(new GUIContent(condition.Name), false, MenuCallback, condition);
                
                menu.ShowAsContext();
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}