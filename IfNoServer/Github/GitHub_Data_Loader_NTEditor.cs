using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GitHubDataLoader))]
public class GitHub_Data_Loader_NTEditor : Editor
{
    private SerializedProperty githubRawUrlProp;
    private SerializedProperty imagePathProp;
    private SerializedProperty dataPathProp;
    private SerializedProperty targetImageProp;
    private SerializedProperty titleTextProp;
    private SerializedProperty descriptionTextProp;
    private SerializedProperty statusTextProp;
    private SerializedProperty cacheSettingsProp;
    private SerializedProperty refreshIntervalProp;
    private SerializedProperty timeoutDurationProp;

    private void OnEnable()
    {
        githubRawUrlProp = serializedObject.FindProperty("githubRawUrl");
        imagePathProp = serializedObject.FindProperty("imagePath");
        dataPathProp = serializedObject.FindProperty("dataPath");
        targetImageProp = serializedObject.FindProperty("targetImage");
        titleTextProp = serializedObject.FindProperty("titleText");
        descriptionTextProp = serializedObject.FindProperty("descriptionText");
        statusTextProp = serializedObject.FindProperty("statusText");
        cacheSettingsProp = serializedObject.FindProperty("cacheSettings");
        refreshIntervalProp = serializedObject.FindProperty("refreshInterval");
        timeoutDurationProp = serializedObject.FindProperty("timeoutDuration");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("GitHub Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(githubRawUrlProp);
        EditorGUILayout.PropertyField(imagePathProp);
        EditorGUILayout.PropertyField(dataPathProp);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("UI References", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(targetImageProp);
        EditorGUILayout.PropertyField(titleTextProp);
        EditorGUILayout.PropertyField(descriptionTextProp);
        EditorGUILayout.PropertyField(statusTextProp);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(cacheSettingsProp);
        EditorGUILayout.PropertyField(refreshIntervalProp);
        EditorGUILayout.PropertyField(timeoutDurationProp);

        EditorGUILayout.Space();
        if (GUILayout.Button("Force Refresh"))
        {
            ((GitHubDataLoader)target).StartLoading();
        }

        if (GUILayout.Button("Clear Cache"))
        {
            ((GitHubDataLoader)target).ClearCache();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
