using System.Collections.Generic;
using UnityEngine;

namespace KingdomScratch
{
    public class CoinBag : MonoBehaviour
    {
        [SerializeField] private List<Coin> coins = new();
        [SerializeField] private int coinCount;

        private void Start()
        {
            coinCount = 0;
        }

        public void AddCoin(Coin coin)
        {
            coins.Add(coin);
            coinCount++;
        }

        public void RemoveCoin()
        {
            if (coinCount > 0)
            {                
                coins.RemoveAt(coins.Count - 1);
                ListClear();
                coinCount--;
            }

            else
            {
                //remove crown. death animation
            }
        }

        public void UseCoins(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Destroy(coins[i]);
                coins.Remove(coins[i]);
            }
        }

        private void ListClear()
        {
            coins.RemoveAll(item => item == null);
        }
    }
}
