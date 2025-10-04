using UnityEngine;

namespace TDLogic
{
    public class Soil : MonoBehaviour , IInteractable
    {
        public Tower currentTower;
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
