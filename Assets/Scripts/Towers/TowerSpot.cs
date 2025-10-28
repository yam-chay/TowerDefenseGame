using UnityEngine;

namespace KingdomScratch
{
    public class TowerSpot : MonoBehaviour , IInteractable
    {
        public bool isFree; 
        [SerializeField] GameObject upgradeMenu;

        public virtual void Interact(Transform interactor)
        {
            upgradeMenu.SetActive(true);
        }

        private void Start()
        {
            upgradeMenu.SetActive(false);
        }

        void Update()
        {
           
        }
    }
}
