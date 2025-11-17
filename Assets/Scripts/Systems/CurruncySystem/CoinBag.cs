using System.Collections.Generic;
using UnityEngine;

namespace KingdomScratch
{
    public class CoinBag : MonoBehaviour
    {
        [SerializeField] public List<Coin> coins = new();
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
                if (coins.Count > 0)
                {
                    Destroy(coins[i].gameObject);
                    coins.Remove(coins[i]);
                }
                else
                {
                    Player.Instance.Die();
                }
            }
        }

        private void ListClear()
        {
            coins.RemoveAll(item => item == null);
        }
    }
}
