using UnityEngine;

namespace TDLogic
{
    public class UpgradeSystem : MonoBehaviour
    {
        public GameObject EvolveOne;
        public GameObject EvolveTwo;
        public GameObject EvolveThree;
        public float SlowMotion;
        public float originTimeScale;
        
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
            var objectToSpawn = Instantiate(EvolveOne, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
        public void ButtonTwo()
        {
            var objectToSpawn = Instantiate(EvolveTwo, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
        public void ButtonThree()
        {
            var objectToSpawn = Instantiate(EvolveThree, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            Time.timeScale = originTimeScale;
        }
    }
}
