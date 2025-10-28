using UnityEngine;

namespace KingdomScratch
{
    public class Spawner : MonoBehaviour
    {
        [Header("Spawner Settings")]
        public GameObject prefabToSpawn;
        public float spawnInterval = 2f;
        public int maxSpawns = 10;  //optional
        public int spawnCount = 0;

        public static Spawner Instance { get; private set; }
        private void Awake()
        {
            Instance = this;
        }

        public void Spawn()
        {
            if (spawnCount >= maxSpawns) return;

            Instantiate(prefabToSpawn, transform.position, Quaternion.identity, this.transform);

            spawnCount++;
        }
    }
}