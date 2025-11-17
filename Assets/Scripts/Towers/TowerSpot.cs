using System.Collections;
using UnityEngine;

namespace KingdomScratch
{
    public class TowerSpot : MonoBehaviour , IInteractable
    {
        [SerializeField] private bool isFree; 
        public int RequiredCoins => requiredCoins;
        [SerializeField] private int requiredCoins = 3;
        [SerializeField] private GameObject upgradeMenu;
        [SerializeField] private Animator[] coinSlots; // slot fill anim
        private Coroutine detectRoutine;


        public void Interact(Transform interactor)
        {
            for (int i = 0; i < coinSlots.Length; i++)
            {
                coinSlots[i].gameObject.SetActive(false);
            }
            upgradeMenu.SetActive(true);
            isFree = false;
        }

        public void OnCoinInserted(int index)
        {
            if (index < coinSlots.Length)
            {
                coinSlots[index].SetTrigger("Fill");
            }
        }

        private void Start()
        {
            isFree = true;
            upgradeMenu.SetActive(false);
        }

        public void OnCoinRemoved(int index)
        {
            if (index < coinSlots.Length)
            {            
                coinSlots[index].SetTrigger("UnFill");
            }
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && isFree)
            {
                Debug.Log("detect slot");
                if (detectRoutine != null)
                {
                    StopCoroutine(detectRoutine);
                    detectRoutine = null;
                }
                detectRoutine = StartCoroutine(OnCoinDetect(true));
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("Undetect slot");
                if (detectRoutine != null)
                {
                    StopCoroutine(detectRoutine);
                    detectRoutine = null;
                }    
                detectRoutine = StartCoroutine(OnCoinDetect(false));
            }
        }

        public IEnumerator OnCoinDetect(bool state)
        {
            for (int i = 0; i < coinSlots.Length; i++)
            {
                yield return new WaitForSeconds(0.4f);
                coinSlots[i].gameObject.SetActive(state);
            }
        }
    }
}
