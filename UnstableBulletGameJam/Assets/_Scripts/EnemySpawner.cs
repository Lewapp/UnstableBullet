using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Serializable]
    public class BossStage
    {
        public string stage;
        public GameObject[] enemyTypes;
        public Vector2 healthTrigger;
    }



    private void Update()
    {
        
    }
}
