using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BulletPatterner))]
public class VariableVisibility : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty shootingType = serializedObject.FindProperty("shootingType");
        SerializedProperty spawnObjects = serializedObject.FindProperty("spawnObjects");
        SerializedProperty bulletSpeed = serializedObject.FindProperty("bulletspeed");

        //  Continouous
        SerializedProperty rotationType = serializedObject.FindProperty("rotationType");
        SerializedProperty spawnDelay = serializedObject.FindProperty("spawnDelay");
        SerializedProperty rotationPerSpawn = serializedObject.FindProperty("rotationPerSpawn");
        SerializedProperty rotationPerTick = serializedObject.FindProperty("rotationPerTick");

        EditorGUILayout.PropertyField(shootingType);
        EditorGUILayout.PropertyField(spawnObjects, true);
        EditorGUILayout.PropertyField(bulletSpeed);


        BulletPatterner.PatternType currentPType = (BulletPatterner.PatternType)shootingType.enumValueIndex;
        if (currentPType == BulletPatterner.PatternType.Continouous)
        {
            EditorGUILayout.PropertyField(rotationType);
            EditorGUILayout.PropertyField(spawnDelay);
            EditorGUILayout.PropertyField(rotationPerSpawn);

            BulletPatterner.RotationType currentRType = (BulletPatterner.RotationType)rotationType.enumValueIndex;
            switch (currentRType)
            {
                case BulletPatterner.RotationType.Spawn:
                    EditorGUILayout.PropertyField(rotationPerSpawn);
                    break;
                case BulletPatterner.RotationType.Time:
                    EditorGUILayout.PropertyField(rotationPerTick);
                    break;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
