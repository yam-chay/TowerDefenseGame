using UnityEngine;

namespace KingdomScratch
{
    public class EffectTTl : MonoBehaviour
    {
        [SerializeField] private float ttl;

        private void Start()
        {
            Destroy(gameObject, ttl);
        }
    }
}
