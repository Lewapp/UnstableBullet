using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Serializable]
    public class BossStage
    {
        public string stage;
        public EnemySpawnSetUp[] enemyTypes;
        public Vector2 healthTrigger;
    }

    [Serializable]
    public class EnemySpawnSetUp
    {
        public bool attached;
        public GameObject type;
        public float cooldown;

        [HideInInspector]
        public GameObject spawnInstance;
        [HideInInspector]
        public float currentTick;
    }

    public BossStage[] bossStages;

    private IDamageable thisDamageable;

    private void Start()
    {
        thisDamageable = GetComponent<IDamageable>();
        SetCooldowns();
    }

    private void Update()
    {
        if (thisDamageable == null)
            return;

        for (int i = 0; i < bossStages.Length; i++)
        {
            float hp = thisDamageable.GetHealthPercent();
            if (hp < bossStages[i].healthTrigger.x || hp > bossStages[i].healthTrigger.y)
            {
                continue;
            }

            StageSpawns(i);
        }
    }

    private void StageSpawns(int stageIndex)
    {
        for (int i = 0; bossStages[stageIndex].enemyTypes.Length > i; i++)
        {
            if (bossStages[stageIndex].enemyTypes[i].spawnInstance || !bossStages[stageIndex].enemyTypes[i].type)
            {
                continue;
            }

            if (bossStages[stageIndex].enemyTypes[i].currentTick >= bossStages[stageIndex].enemyTypes[i].cooldown)
            {
                bossStages[stageIndex].enemyTypes[i].currentTick = 0f;
                bossStages[stageIndex].enemyTypes[i].spawnInstance = Instantiate(bossStages[stageIndex].enemyTypes[i].type, transform.position, Quaternion.identity);

                if (!bossStages[stageIndex].enemyTypes[i].attached)
                    SetUpRandomPosition(stageIndex, i);
            }

            bossStages[stageIndex].enemyTypes[i].currentTick += Time.deltaTime;
        }
    }

    private void SetUpRandomPosition(int stageIndex, int enemyIndex)
    {
        RandomPosition randomPosition = bossStages[stageIndex].enemyTypes[enemyIndex].spawnInstance.AddComponent<RandomPosition>();
        
        if (transform.position.x < 0)
        {
            randomPosition.outterBorderL = new Vector2(-28f, -15f);
            randomPosition.outterBorderU = new Vector2(-2f, 15f);
            randomPosition.innerBorderL = new Vector2(-22f, -6f);
            randomPosition.innerBorderU = new Vector2(-8f, 6f);
            return;
        }

        randomPosition.outterBorderL = new Vector2(28f, -15f);
        randomPosition.outterBorderU = new Vector2(2f, 15f);
        randomPosition.innerBorderL = new Vector2(22f, -6f);
        randomPosition.innerBorderU = new Vector2(8f, 6f);
    }

    private void SetCooldowns()
    {
        for (int i = 0; i < bossStages.Length; i++)
        {
            for (int j = 0; bossStages[i].enemyTypes.Length > j; j++)
            {
                bossStages[i].enemyTypes[j].currentTick = bossStages[i].enemyTypes[j].cooldown;
            }
        }
    }
}
