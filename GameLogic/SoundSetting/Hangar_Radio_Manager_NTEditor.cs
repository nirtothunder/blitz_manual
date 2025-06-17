using UnityEditor;
using UnityEngine;

namespace nirtothunder
{
    [CustomEditor(typeof(Hangar_Radio_Manager_NT))]
    public class Hangar_Radio_Manager_NT_Editor : Editor
    {
        SerializedProperty Hangar_Music_PlayerProp;
        SerializedProperty Radio_Music_SourceProp;
        SerializedProperty Fade_DurationProp;
        SerializedProperty Hangar_Music_DelayProp;

        void OnEnable()
        {
            Hangar_Music_PlayerProp = serializedObject.FindProperty("Hangar_Music_Player");
            Radio_Music_SourceProp = serializedObject.FindProperty("Radio_Music_Source");
            Fade_DurationProp = serializedObject.FindProperty("Fade_Duration");
            Hangar_Music_DelayProp = serializedObject.FindProperty("Hangar_Music_Delay");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Управление радио в ангаре\nПервое нажатие: Включает радио\nВторое нажатие: Выключает радио (музыка ангара включится через указанную задержку)", MessageType.Info);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Основные настройки", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(Hangar_Music_PlayerProp, new GUIContent("Плеер музыки ангара"));
            EditorGUILayout.PropertyField(Radio_Music_SourceProp, new GUIContent("Источник радио"));

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Настройки переходов", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(Fade_DurationProp, new GUIContent("Длительность fade эффекта"));
            EditorGUILayout.PropertyField(Hangar_Music_DelayProp, new GUIContent("Задержка включения музыки ангара (сек)"));

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Для работы требуется:\n1. Коллайдер на объекте\n2. Назначенные аудио источники", MessageType.Warning);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
