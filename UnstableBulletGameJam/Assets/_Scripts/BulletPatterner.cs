using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletPatterner : MonoBehaviour
{
    [Serializable]
    public class Pattern
    {
        [Header("Standard Variables")]
        public GameObject spawnObject;
        public List<BulletModifier> bulletModifiers = new List<BulletModifier>();
        public float bulletspeed;
        public PatternType shootingType;

        [Header("Continouous Variables")]
        public RotationType rotationType;
        public float spawnDelay;
        public float rotationPerSpawn;
        public float rotationPerTick;

        [Header("Sine Modifier")]
        public float frequency;
        public float amplitude;

        [HideInInspector]
        public float timeSinceLastBullet;
        [HideInInspector]
        public float currentRotation;
    }

    public Pattern[] bulletPatterns;

    private void Update()
    {
        if (!bulletPatterns?[0].spawnObject)
            return;

        for (int i = 0; i < bulletPatterns.Length; i++)
        {
            if (bulletPatterns[i].shootingType == PatternType.Continouous)
                ContinouousShooting(i);
        }
    }

    private void ContinouousShooting(int index)
    {
        bulletPatterns[index].timeSinceLastBullet += Time.deltaTime;

        if (bulletPatterns[index].rotationType == RotationType.Time)
            bulletPatterns[index].currentRotation = Mathf.Repeat(bulletPatterns[index].currentRotation + bulletPatterns[index].rotationPerTick * Time.deltaTime, 360f);

        if (bulletPatterns[index].timeSinceLastBullet >= bulletPatterns[index].spawnDelay)
        {
            GameObject spawn = Instantiate(bulletPatterns[index].spawnObject, transform.position, Quaternion.Euler(0, 0, bulletPatterns[index].currentRotation));
            spawn.transform.SetParent(transform);
            Rigidbody2D rb = spawn.GetComponent<Rigidbody2D>();

            if (bulletPatterns[index].rotationType == RotationType.Spawn)
                bulletPatterns[index].currentRotation = Mathf.Repeat(bulletPatterns[index].currentRotation + Mathf.Max(bulletPatterns[index].rotationPerSpawn, 0.1f), 360f);

            bulletPatterns[index].timeSinceLastBullet = 0;

            if (rb)
                rb.linearVelocity = spawn.transform.up * bulletPatterns[index].bulletspeed;

            AddModifier(spawn, index);
        }
    }

    private void AddModifier(GameObject toObject, int index)
    {
        for (int i = 0; i < bulletPatterns[index].bulletModifiers.Count; i++)
        {
            switch (bulletPatterns[index].bulletModifiers[i])
            {
                case BulletModifier.Sine:
                    Sinewave script = toObject.AddComponent<Sinewave>();
                    script.frequency = bulletPatterns[index].frequency;
                    script.amplitude = bulletPatterns[index].amplitude;
                    break;
            }
        }
    }

    public enum PatternType
    {
        None, Continouous, Burst
    }

    public enum RotationType
    {
        None, Spawn, Time
    }

    public enum BulletModifier
    {
        None, Sine
    }
}
