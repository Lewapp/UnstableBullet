using System.Collections.Generic;
using UnityEngine;

public class BulletPatterner : MonoBehaviour
{
    public PatternType shootingType;
    public List<GameObject> spawnObjects = new List<GameObject>();

    [Header("Standard Variables")]
    public float bulletspeed;

    [Header("Continouous Variables")]
    public RotationType rotationType;
    public float spawnDelay;
    private float timeSinceLastBullet;
    public float rotationPerSpawn;
    public float rotationPerTick;
    private float currentRotation;

    private void Update()
    {
        if (spawnObjects.Count <= 0)
            return;

        if (shootingType == PatternType.Continouous)
            ContinouousShooting();
    }

    private void ContinouousShooting()
    {
        timeSinceLastBullet += Time.deltaTime;

        if (rotationType == RotationType.Time)
            currentRotation = Mathf.Repeat(currentRotation + rotationPerTick * Time.deltaTime, 360f);

        if (timeSinceLastBullet >= spawnDelay)
        {
            GameObject spawn = Instantiate(spawnObjects[0], transform.position, Quaternion.Euler(0, 0, currentRotation));
            spawn.transform.SetParent(transform);
            Rigidbody2D rb = spawn.GetComponent<Rigidbody2D>();

            if (rotationType == RotationType.Spawn)
                currentRotation = Mathf.Repeat(currentRotation + Mathf.Max(rotationPerSpawn, 0.1f), 360f);

            timeSinceLastBullet = 0;

            if (rb)
                rb.linearVelocity = spawn.transform.up * bulletspeed;
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
}
