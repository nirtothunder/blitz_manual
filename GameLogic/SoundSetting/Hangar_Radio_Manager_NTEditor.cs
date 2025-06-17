using UnityEngine;
using UnityEditor;

namespace nirtothunder
{
    [CustomEditor(typeof(Hangar_Radio_Manager_NT))]
    public class Hangar_Radio_Manager_NTEditor : Editor
    {
        SerializedProperty Hangar_Music_PlayerProp;
        SerializedProperty Radio_Music_SourceProp;
        SerializedProperty Fade_DurationProp;
        SerializedProperty Activation_KeyProp;

        void OnEnable()
        {
            Hangar_Music_PlayerProp = serializedObject.FindProperty("Hangar_Music_Player");
            Radio_Music_SourceProp = serializedObject.FindProperty("Radio_Music_Source");
            Fade_DurationProp = serializedObject.FindProperty("Fade_Duration");
            Activation_KeyProp = serializedObject.FindProperty("Activation_Key");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUI.backgroundColor = new Color(0.8f, 0.8f, 0.2f, 1.0f);

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Контроллер радио кнопки", MessageType.None, true);
            
            EditorGUILayout.PropertyField(Hangar_Music_PlayerProp);
            EditorGUILayout.PropertyField(Radio_Music_SourceProp);
            EditorGUILayout.PropertyField(Fade_DurationProp);
            EditorGUILayout.PropertyField(Activation_KeyProp);

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
