using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPatterner : MonoBehaviour
{
    [HideInInspector]
    public bool isLeftSide;

    [Serializable]
    public class Pattern
    {
        [Header("Standard Variables")]
        public GameObject spawnObject;
        public List<BulletModifier> bulletModifiers = new List<BulletModifier>();
        public float bulletspeed;
        public PatternType shootingType;
            [HideInInspector]
            public float timeSinceLastBullet;
            [HideInInspector]
            public float currentRotation;

        [Header("Continouous Variables")]
        public float spawnDelay;

        [Header("Burst Variables")]
        public float amountPerBurst;
        public float delayPerBurst;
        public float intervalPerShot;
            [HideInInspector]
            public float timeSinceLastBurst;

        [Header("Rotation Settings")]
        public RotationType rotationType;
        public float rotationPerSpawn;
        public float rotationPerTick;
        public bool offsetApplied;
        [Range(0f, 360f)]
        public float playerOffset;
        public Vector2 randomRotRange;
        [Range(0f, 360f)]
        public float randomRotOffset;
        [Range(0f, 360f)]
        public float forwardOffset;

        [Header("Sine Modifier")]
        public float frequency;
        public float amplitude;
    }

    public Pattern[] bulletPatterns;

    private void Update()
    {
        if (!(bulletPatterns?[0].spawnObject))
            return;

        for (int i = 0; i < bulletPatterns.Length; i++)
        {
            switch (bulletPatterns[i].rotationType)
            {
                case RotationType.Time:
                    bulletPatterns[i].currentRotation = Mathf.Repeat(bulletPatterns[i].currentRotation + bulletPatterns[i].rotationPerTick * Time.deltaTime, 360f);
                    if (!bulletPatterns[i].offsetApplied)
                        bulletPatterns[i].currentRotation += transform.eulerAngles.z;
                    bulletPatterns[i].offsetApplied = true;
                    break;
                case RotationType.Player:
                    Vector2 thisPlayer = FocusedPlayer();

                    Vector3 direction = thisPlayer - (Vector2)transform.position;
                    bulletPatterns[i].currentRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - (90f + bulletPatterns[i].playerOffset);
                    break;
            }

            if (bulletPatterns[i].shootingType == PatternType.Continouous)
                ContinouousShooting(i);
            else if (bulletPatterns[i].shootingType == PatternType.Burst)
            {
                bulletPatterns[i].timeSinceLastBurst += Time.deltaTime;
                if (bulletPatterns[i].timeSinceLastBurst >= bulletPatterns[i].delayPerBurst)
                {
                    StartCoroutine(BurstShot(i));
                    bulletPatterns[i].timeSinceLastBurst = 0f;
                }
            }
        }
    }

    private Vector2 FocusedPlayer()
    {
        Vector2 player = GameObject.FindGameObjectWithTag("Player").transform.position;

        if (isLeftSide)
        {
            if (player.x < 0)
            {
                return player;
            }
            return new Vector2 (-player.x, player.y);
        }
        
        if (player.x > 0)
        {
            return player;
        }

        return new Vector2(-player.x, player.y);
    }

    private void ContinouousShooting(int index)
    {
        bulletPatterns[index].timeSinceLastBullet += Time.deltaTime;

        if (bulletPatterns[index].timeSinceLastBullet >= bulletPatterns[index].spawnDelay)
        {
            Shoot(index);
        }
    }

    private IEnumerator BurstShot(int index)
    {
        for (int i = 0; i < bulletPatterns[index].amountPerBurst; i++)
        {
            Shoot(index);
            yield return new WaitForSeconds(bulletPatterns[index].intervalPerShot);
        }
    }

    private void Shoot(int index)
    {
        switch (bulletPatterns[index].rotationType)
        {
            case RotationType.Spawn:
                bulletPatterns[index].currentRotation = Mathf.Repeat(bulletPatterns[index].currentRotation + Mathf.Max(bulletPatterns[index].rotationPerSpawn, 0.1f), 360f);
                if (!bulletPatterns[index].offsetApplied)
                    bulletPatterns[index].currentRotation += transform.eulerAngles.z;
                bulletPatterns[index].offsetApplied = true;
                break;
            case RotationType.Random:
                bulletPatterns[index].currentRotation = UnityEngine.Random.Range(bulletPatterns[index].randomRotRange.x, bulletPatterns[index].randomRotRange.y) + bulletPatterns[index].randomRotOffset;
                break;
            case RotationType.Forward:
                bulletPatterns[index].currentRotation = transform.eulerAngles.z + bulletPatterns[index].forwardOffset;
                break;
        }
           
        GameObject spawn = Instantiate(bulletPatterns[index].spawnObject, transform.position, Quaternion.Euler(0, 0, bulletPatterns[index].currentRotation));
        Rigidbody2D rb = spawn.GetComponent<Rigidbody2D>();
    

        bulletPatterns[index].timeSinceLastBullet = 0;

        if (rb)
            rb.linearVelocity = spawn.transform.up * bulletPatterns[index].bulletspeed;

        AddModifier(spawn, index);
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
        None, Spawn, Time, Player, Random, Forward
    }

    public enum BulletModifier
    {
        None, Sine
    }
}
