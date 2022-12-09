using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveSpawner : MonoBehaviour
{
    [Header("Wave settings")]
    [SerializeField] private Wave[] waves;
    [SerializeField] private float timeBetweenWaves;
    
    [System.Serializable]
    struct Wave
    {
        public int waveIndex;
        public List<EnemiesInfo> enemy;
        public GameObject boss;
    }
    [System.Serializable]
    struct EnemiesInfo
    {
        public GameObject enemy;
        public int count;
    }
    private void OnValidate()
    {
        for (int i = 0; i < waves.Length; i++) waves[i].waveIndex = i;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
