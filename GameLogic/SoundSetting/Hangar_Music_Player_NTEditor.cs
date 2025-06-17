using UnityEngine;
using System.Collections;
using UnityEditor;

namespace nirtothunder
{
    [CustomEditor(typeof(Hangar_Music_Player_NT))]
    public class Hangar_Music_Player_NTEditor : Editor
    {
        SerializedProperty Audio_ClipsProp;
        SerializedProperty Delay_RangeProp;

        void OnEnable()
        {
            Audio_ClipsProp = serializedObject.FindProperty("Audio_Clips");
            Delay_RangeProp = serializedObject.FindProperty("Delay_Range");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUI.backgroundColor = new Color(0.0f, 1.0f, 0.0f, 1.0f);

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Настройки музыки в ангаре", MessageType.None, true);
            
            EditorGUILayout.PropertyField(Audio_ClipsProp, new GUIContent("Аудио клипы"), true);
            EditorGUILayout.PropertyField(Delay_RangeProp, new GUIContent("Диапазон задержки"));

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
