using System.Collections.Generic;
using UnityEngine;

namespace KingdomScratch
{
    public class CoinBag : MonoBehaviour
    {
        [SerializeField] public List<Coin> coins = new();

        private void Start()
        {
        }

        public void AddCoin(Coin coin)
        {
            coins.Add(coin);
        }

        public void RemoveCoin()
        {
            if (coins.Count > 0)
            {                
                coins.RemoveAt(coins.Count - 1);
                ListClear();
            }
        }

        public void UseCoins(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                if (coins.Count > 0)
                {
                    Destroy(coins[i].gameObject);
                    coins.Remove(coins[i]);
                }
            }
        }

        private void ListClear()
        {
            coins.RemoveAll(item => item == null);
        }
    }
}
