using System;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [Serializable]
    public class BossStage
    {
        public string stage;
        public BulletSpawnSetUp[] bulletSpawns;
        public Vector2 healthTrigger;
    }


    [Serializable]
    public class BulletSpawnSetUp
    {
        public bool attached;
        public GameObject spawnType;
        public float spawnTick;
        public float lifeTime;
        public float spawnRotation;

        [HideInInspector]
        public GameObject spawnInstance;
        [HideInInspector]
        public float currentTick;
    }

    public bool isLeftSide;
    public BossStage[] stages;
    private IDamageable thisDamageable;

    private void Start()
    {
        thisDamageable = GetComponent<IDamageable>();
    }

    private void Update()
    {
        if (thisDamageable == null)
            return;

        for (int i = 0; i < stages.Length; i++)
        {
            float hp = thisDamageable.GetHealthPercent();
            if (hp < stages[i].healthTrigger.x || hp > stages[i].healthTrigger.y)
            {
                DestroySpawnInstances(stages[i]);
                continue;
            }

            StageSpawns(i);

        }
    }
    private void DestroySpawnInstances(BossStage stageInstance)
    {
        if (stageInstance == null)
            return;

        foreach (BulletSpawnSetUp spawnSetup in stageInstance.bulletSpawns)
        {
            if (!spawnSetup.spawnInstance)
                continue;

            Destroy(spawnSetup.spawnInstance);
        }
    }

    private void StageSpawns(int stageIndex)
    {
        for (int i = 0; stages[stageIndex].bulletSpawns.Length > i; i++)
        {
            bool newInstance = false;

            if (stages[stageIndex].bulletSpawns[i].spawnInstance || !stages[stageIndex].bulletSpawns[i].spawnType)
                continue;

            if (stages[stageIndex].bulletSpawns[i].currentTick >= stages[stageIndex].bulletSpawns[i].spawnTick)
            {
                stages[stageIndex].bulletSpawns[i].currentTick = 0f;
                stages[stageIndex].bulletSpawns[i].spawnInstance = Instantiate(stages[stageIndex].bulletSpawns[i].spawnType, transform.position, Quaternion.identity);
                AttachSpawnToThis(stageIndex, i);

                stages[stageIndex].bulletSpawns[i].spawnInstance.transform.localRotation = Quaternion.Euler(0f, 0f, stages[stageIndex].bulletSpawns[i].spawnRotation);

                BulletPatterner bulletPatterner = stages[stageIndex].bulletSpawns[i].spawnInstance.GetComponent<BulletPatterner>();
                if (bulletPatterner)
                    bulletPatterner.isLeftSide = isLeftSide;

                newInstance = true;
            }


            if (newInstance && stages[stageIndex].bulletSpawns[i].lifeTime > 0)
            {
                LifeTime life = stages[stageIndex].bulletSpawns[i].spawnInstance.AddComponent<LifeTime>();
                life.lifetime = stages[stageIndex].bulletSpawns[i].lifeTime;
            }

            stages[stageIndex].bulletSpawns[i].currentTick += Time.deltaTime;
        }
    }

    private void AttachSpawnToThis(int stageIndex, int spawnIndex)
    {
        if (stages[stageIndex].bulletSpawns[spawnIndex].attached)
        {
            stages[stageIndex].bulletSpawns[spawnIndex].spawnInstance.transform.SetParent(transform);
        }
    }

}
