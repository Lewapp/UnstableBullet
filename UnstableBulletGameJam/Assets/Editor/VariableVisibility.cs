using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(BulletPatterner))]
public class BulletPatternerEditor : Editor
{
    private SerializedProperty bulletPatterns;
    private bool[] foldouts;

    private void OnEnable()
    {
        bulletPatterns = serializedObject.FindProperty("bulletPatterns");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Bullet Patterns", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;

        int newSize = EditorGUILayout.IntField("Size", bulletPatterns.arraySize);
        if (newSize != bulletPatterns.arraySize)
        {
            bulletPatterns.arraySize = newSize;
        }

        BulletPatterner bulletPatterner = (BulletPatterner)target;
        if (foldouts == null || foldouts.Length != bulletPatterns.arraySize)
        {
            foldouts = new bool[bulletPatterns.arraySize];
        }

        for (int i = 0; i < bulletPatterns.arraySize; i++)
        {
            foldouts[i] = EditorGUILayout.Foldout(foldouts[i], $"Bullet Pattern {i}", true);

            if (foldouts[i])
            {
                SerializedProperty pattern = bulletPatterns.GetArrayElementAtIndex(i);

                EditorGUILayout.BeginVertical("box");
                EditorGUI.indentLevel++;

                DrawPattern(pattern);

                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }
        }

        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawPattern(SerializedProperty pattern)
    {
        SerializedProperty spawnObject = pattern.FindPropertyRelative("spawnObject");
        SerializedProperty bulletModifiers = pattern.FindPropertyRelative("bulletModifiers");
        SerializedProperty bulletSpeed = pattern.FindPropertyRelative("bulletspeed");
        SerializedProperty shootingType = pattern.FindPropertyRelative("shootingType");

        SerializedProperty spawnDelay = pattern.FindPropertyRelative("spawnDelay");

        SerializedProperty amountPerBurst = pattern.FindPropertyRelative("amountPerBurst");
        SerializedProperty delayPerBurst = pattern.FindPropertyRelative("delayPerBurst");
        SerializedProperty intervalPerShot = pattern.FindPropertyRelative("intervalPerShot");

        SerializedProperty rotationType = pattern.FindPropertyRelative("rotationType");
        SerializedProperty rotationPerSpawn = pattern.FindPropertyRelative("rotationPerSpawn");
        SerializedProperty rotationPerTick = pattern.FindPropertyRelative("rotationPerTick");
        SerializedProperty playerOffset = pattern.FindPropertyRelative("playerOffset");
        SerializedProperty randomRotRange = pattern.FindPropertyRelative("randomRotRange");
        SerializedProperty randomRotOffset = pattern.FindPropertyRelative("randomRotOffset");

        SerializedProperty frequency = pattern.FindPropertyRelative("frequency");
        SerializedProperty amplitude = pattern.FindPropertyRelative("amplitude");

        EditorGUILayout.PropertyField(spawnObject);
        EditorGUILayout.PropertyField(bulletModifiers);
        EditorGUILayout.PropertyField(bulletSpeed);
        EditorGUILayout.PropertyField(shootingType);

        var currentPType = (BulletPatterner.PatternType)shootingType.enumValueIndex;
        if (currentPType == BulletPatterner.PatternType.Continouous)
        {
            EditorGUILayout.PropertyField(spawnDelay);
        }
        else if (currentPType == BulletPatterner.PatternType.Burst)
        {
            EditorGUILayout.PropertyField(amountPerBurst);
            EditorGUILayout.PropertyField(delayPerBurst);
            EditorGUILayout.PropertyField(intervalPerShot);

            float totalWaitTime = amountPerBurst.floatValue * intervalPerShot.floatValue;
            if (totalWaitTime > delayPerBurst.floatValue)
            {
                intervalPerShot.floatValue = delayPerBurst.floatValue / amountPerBurst.floatValue;
            }
        }

            EditorGUILayout.PropertyField(rotationType);
        var currentRType = (BulletPatterner.RotationType)rotationType.enumValueIndex;
        switch (currentRType)
        {
            case BulletPatterner.RotationType.Spawn:
                EditorGUILayout.PropertyField(rotationPerSpawn);
                break;
            case BulletPatterner.RotationType.Time:
                EditorGUILayout.PropertyField(rotationPerTick);
                break;
            case BulletPatterner.RotationType.Player:
                EditorGUILayout.PropertyField(playerOffset);
                break;
            case BulletPatterner.RotationType.Random:
                EditorGUILayout.PropertyField(randomRotRange);
                EditorGUILayout.PropertyField(randomRotOffset);
                break;
        }


        if (HasModifier(bulletModifiers, BulletPatterner.BulletModifier.Sine))
        {
            EditorGUILayout.PropertyField(frequency);
            EditorGUILayout.PropertyField(amplitude);
        }
    }

    private bool HasModifier(SerializedProperty listProperty, BulletPatterner.BulletModifier targetModifier)
    {
        for (int i = 0; i < listProperty.arraySize; i++)
        {
            SerializedProperty element = listProperty.GetArrayElementAtIndex(i);
            if (element.enumValueIndex == (int)targetModifier)
            {
                return true;
            }
        }
        return false;
    }
}