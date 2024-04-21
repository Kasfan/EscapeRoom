using UnityEditor;
using UnityEngine;

namespace EscapeRoom.QuestLogic.EditorScripts
{
    [CustomEditor(typeof(StateDatabaseComponent))]
    public class StateDatabaseComponentEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if(GUILayout.Button("Detect providers"))
            {
                var database = (StateDatabaseComponent)serializedObject.targetObject;
                database.FindChildrenStateProviders();
                EditorUtility.SetDirty(database);
            }
        }
    }
}