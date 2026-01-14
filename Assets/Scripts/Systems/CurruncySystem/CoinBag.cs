using System;
using System.Collections.Generic;
using UnityEngine;

namespace KingdomScratch
{
    public class CoinBag : MonoBehaviour
    {
        [SerializeField] private List<Coin> coins = new();


        // Observer pattern: other scripts listen to these
        public event Action<Coin> OnCoinAdded;
        public event Action<Coin> OnCoinRemoved;
        public event Action<int> OnCoinCountChanged;

        public int CoinCount => coins.Count;
        public static CoinBag Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
            }

        }
        public void AddCoin(Coin coin)
        {
            if (coin == null) return;

            coins.Add(coin);
            OnCoinAdded?.Invoke(coin);
            OnCoinCountChanged?.Invoke(coins.Count);
        }

        public void RemoveLastCoin()
        {
            if (coins.Count <= 0) return;

            var coin = coins[^1];
            coins.RemoveAt(coins.Count - 1);
            OnCoinRemoved?.Invoke(coin);
            OnCoinCountChanged?.Invoke(coins.Count);
        }

        public void UseCoins(int amount)
        {
            // Permanently consume coins for interaction cost
            for (int i = 0; i < amount; i++)
            {
                if (coins.Count <= 0)
                {
                    break;
                }
                var coin = coins[0];
                coins.RemoveAt(0);

                if (coin != null)
                {
                    Destroy(coin.gameObject);
                }

                OnCoinRemoved?.Invoke(coin);
            }

            ListClear();
            OnCoinCountChanged?.Invoke(coins.Count);
        }

        private void ListClear()
        {
            coins.RemoveAll(item => item == null);
        }
    }
}
