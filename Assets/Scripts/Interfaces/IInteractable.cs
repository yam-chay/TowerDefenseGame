using System.Collections;
using UnityEngine;

namespace KingdomScratch
{
    public interface IInteractable
    {
        int RequiredCoins { get; }
        void OnCoinInserted(int index);
        void OnCoinRemoved(int index);
        void Interact(Transform interactor);

    }
}
