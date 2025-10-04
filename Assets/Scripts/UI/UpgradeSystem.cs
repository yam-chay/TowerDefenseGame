using UnityEngine;
using UnityEngine.UI;

namespace TDLogic
{
    public class UpgradeSystem : MonoBehaviour
    {
        public GameObject EvolveOne;
        public GameObject EvolveTwo;
        public GameObject EvolveThree;
        public float SlowMotion;
        public float originTimeScale;
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
            objectToSpawn = Instantiate(EvolveOne, buildPoint.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
        public void ButtonTwo()
        {
            if (isFree)
            {
                Destroy(objectToSpawn);
            }
            objectToSpawn = Instantiate(EvolveTwo, buildPoint.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
        public void ButtonThree()
        {
            if (isFree)
            {
                Destroy(objectToSpawn);
            }
            objectToSpawn = Instantiate(EvolveThree, buildPoint.position, Quaternion.identity);
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
