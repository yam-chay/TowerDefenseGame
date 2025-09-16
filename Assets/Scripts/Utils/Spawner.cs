using UnityEngine;

namespace TDLogic
{
    public class Spawner : MonoBehaviour
    {
        [Header("Spawner Settings")]
        public GameObject prefabToSpawn;
        public float spawnInterval = 2f;
        public int maxSpawns = 10;  //optional

        [Header("Movement options")]
        public GameObject targetToMove; //optional for movement

        private int spawnCount = 0;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Spawn();
            }
        }

        private void Spawn()
        {
            if (spawnCount >= maxSpawns) return;

            GameObject spawnling = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);

            spawnCount++;
        }
    }
}