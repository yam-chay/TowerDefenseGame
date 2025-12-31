using KingdomScratch;
using UnityEngine;

namespace TDLogic
{
    public class UpgradeSystem : MonoBehaviour
    {
        public GameObject Tower1;
        public GameObject Tower2;
        public GameObject Tower3;
        public float SlowMotion;
        public float originTimeScale;
        public float spawnOffset;
        public Transform buildPoint;
        public bool isFree;
        private GameObject objectToSpawn;        
        void Start()
        {
            originTimeScale = Time.timeScale;
        }

        void Update()
        {
            if (gameObject.activeSelf)
            {
                Time.timeScale = SlowMotion;
            }
        }

        public void ButtonOne()
        {
            if (isFree)
            {
                Destroy(objectToSpawn);  
            }
            objectToSpawn = Instantiate(Tower1, buildPoint.position + (Vector3.up * spawnOffset), Quaternion.identity);
            var parentObject = buildPoint.GetComponentInParent<TowerSpot>();
            parentObject.transform.parent = objectToSpawn.transform; 
            gameObject.SetActive(false);
        }
        public void ButtonTwo()
        {
            if (isFree)
            {
                Destroy(objectToSpawn);
            }
            objectToSpawn = Instantiate(Tower2, buildPoint.position + (Vector3.up * spawnOffset), Quaternion.identity);
            var parentObject = buildPoint.GetComponentInParent<TowerSpot>();
            parentObject.transform.parent = objectToSpawn.transform;
            gameObject.SetActive(false);
        }
        public void ButtonThree()
        {
            if (isFree)
            {
                Destroy(objectToSpawn);
            }
            objectToSpawn = Instantiate(Tower3, buildPoint.position + (Vector3.up * spawnOffset), Quaternion.identity);
            var parentObject = buildPoint.GetComponentInParent<TowerSpot>();
            parentObject.transform.parent = objectToSpawn.transform;
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            if (!objectToSpawn)
            {
                isFree = true;
            }
        }

        private void OnDisable()
        {
            Time.timeScale = originTimeScale;
        }
    }
}
