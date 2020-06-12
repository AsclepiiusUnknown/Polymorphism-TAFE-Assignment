using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(Flock)), CanEditMultipleObjects]
public class FlockEditor : Editor
{

    public SerializedProperty
        colorChangeState_Prop,
        startColor_Prop,
        fadeToColor_Prop,
        colorLerpDivider_Prop;

    void OnEnable()
    {
        // Setup the SerializedProperties
        colorChangeState_Prop = serializedObject.FindProperty("colorChangeState");
        startColor_Prop = serializedObject.FindProperty("startColor");
        fadeToColor_Prop = serializedObject.FindProperty("fadeToColor");
        colorLerpDivider_Prop = serializedObject.FindProperty("colorLerpDivider");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(colorChangeState_Prop);

        ColorChangeState CCS = (ColorChangeState)colorChangeState_Prop.enumValueIndex;

        switch (CCS)
        {
            case ColorChangeState.Single:
                EditorGUILayout.ColorField(Color.white); //Start Color
                break;

            case ColorChangeState.Gradient:
                EditorGUILayout.ColorField(Color.white); //Start Color
                EditorGUILayout.ColorField(Color.white); //End COlor
                EditorGUILayout.IntSlider(colorLerpDivider_Prop, 0, 1, new GUIContent("colorLerpDivider"));
                break;

            case ColorChangeState.None:
                break;

        }

        serializedObject.ApplyModifiedProperties();
    }
}